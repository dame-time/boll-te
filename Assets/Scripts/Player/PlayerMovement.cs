using System.Collections;
using Clients;
using Clients.Orders;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public bool canGrab = false;
    
    [SerializeField]
    private Rigidbody rigibody;

    [SerializeField]
    private float speed;

    private Vector2 moveDirection;

    [SerializeField]
    private InputAction playerControls;
    [SerializeField]
    private InputAction interaction;
    [SerializeField]
    private InputAction clientInteraction;

    [SerializeField]
    private InputAction testHard;

    public Animator playerAnimator;

    public bool canInteractWihtClient=false;
    public Client clientRef;
    private ScoreAnimator _scoreAnimator;

    //test
    // [SerializeField]
    // private  GameObject objectTest;
    // [SerializeField]
    // private GameObject hardcodePosition;

    private bool canMove;
    private PlayerBackpack _playerBackpack;
    private Stations _stations;

    [SerializeField]
    private GameObject hardcodePosition;
    [SerializeField]
    private GameObject objectTest;
    private bool objectinHandTest = false;

    private GameObject tempHold;

    public Slider bubbleProgression;

    public AudioSource playerAudio;
    public AudioClip clientPay;
    public AudioClip grabAudio;
    public AudioClip dropAudio;

    private void Awake()
    {
        _playerBackpack = GetComponent<PlayerBackpack>();
        _stations = FindObjectOfType<Stations>();
        bubbleProgression.maxValue = 100;
        bubbleProgression.value = 0;
        _scoreAnimator = FindObjectOfType<ScoreAnimator>();
    }

    public AudioClip footstepSounds; // Array of footstep sounds
    public float footstepInterval = 0.5f; // Time between footsteps
    public float walkingSpeedThreshold = 0.1f; // Minimum speed to trigger footstep sounds
    private float footstepTimer = 0f;



    private void OnEnable()
    {
        playerControls.Enable();
        interaction.Enable();
        clientInteraction.Enable();
        testHard.Enable();
        testHard.performed += HardTest;
        interaction.performed += Interact;
        clientInteraction.performed += InteractionClient;

    }


    private void OnDisable()
    {
        playerControls.Disable();
        interaction.Disable();
        testHard.Disable();
        testHard.performed -= HardTest;
        interaction.performed -= Interact;
        clientInteraction.performed -= InteractionClient;
    }

    private void HardTest(InputAction.CallbackContext context)
    {
        objectTest.gameObject.SetActive(true);
        objectTest.transform.position = hardcodePosition.transform.position;
        objectTest.transform.SetParent(hardcodePosition.transform);
        objectinHandTest = true;
        playerAnimator.SetBool("Grabbed", true);
        StartCoroutine(ResetGrab("Grabbed"));
    }

    IEnumerator ResetGrab(string reset)
    {
        yield return new WaitForSeconds(0.2f);
        playerAnimator.SetBool(reset, false);
    }



    // Update is called once per frame
    void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();


        if (moveDirection != Vector2.zero)
        {
            footstepTimer += Time.deltaTime;

            // Play a footstep sound at the specified interval
            if (footstepTimer >= footstepInterval)
            {
                playerAudio.PlayOneShot(footstepSounds);
                footstepTimer = 0f; // Reset the timer
            }
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.y));

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
        else
        {
            footstepTimer = 0f;
        }


    }

    private void FixedUpdate()
    {

        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("AN_Gigino_Grab"))
        {
            rigibody.velocity = new Vector3(moveDirection.x * speed, 0, moveDirection.y * speed);
            playerAnimator.SetFloat("velocity", rigibody.velocity.magnitude);

        }
        else
        {
            rigibody.velocity = Vector3.zero;
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("AN_Gigino_Grab_Intro") && tempHold)
        {
            tempHold.transform.position = hardcodePosition.transform.position;
            tempHold.transform.SetParent(hardcodePosition.transform);
            tempHold.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            if (tempHold.gameObject.name.Contains("Cup"))
                tempHold.transform.localScale = new Vector3(.4f, .4f, .4f);
        }
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("AN_Gigino_Grab_Loop") && tempHold)
        {
            tempHold.transform.position = hardcodePosition.transform.position;
            tempHold.transform.SetParent(hardcodePosition.transform);
            tempHold.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            if (tempHold.gameObject.name.Contains("Cup"))
                tempHold.transform.localScale = new Vector3(.4f, .4f, .4f);
        }



    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (_stations.GetActiveStation() == null) return;

        var action = _stations.GetActiveStation().ExecuteAction();

        if (action == ExecutedAction.None) return;
        
        playerAnimator.SetBool(action.ToString(), true);
        playerAnimator.SetFloat("velocity", 0);
        
        StartCoroutine(ExampleCoroutine(action));
    }

    public void Drop()
    {
        playerAudio.PlayOneShot(dropAudio);
        Destroy(tempHold);
        tempHold = null;
    }

    private void InteractionClient(InputAction.CallbackContext context)
    {
        if (!canInteractWihtClient || !clientRef || !_playerBackpack.isHoldingObject) return;
        
        if (_playerBackpack.teaType == clientRef.currentThe.teaType 
            && _playerBackpack.bubbleType == clientRef.currentThe.bubbleType)
        {
            // GameManager.Instance.money += clientRef.currentThe.value;
            _scoreAnimator.AddScore(clientRef.currentThe.value);
            playerAudio.PlayOneShot(clientPay);
            clientRef.clientAnimator.SetBool("isHappy", true);
        }
        else
        {
            clientRef.clientAnimator.SetBool("isAngry", true);
        }

        //test
        // objectTest.gameObject.SetActive(false);
        // objectinHandTest = false;
        playerAnimator.SetBool("Dropped", true);
        StartCoroutine(ResetGrab("Dropped"));

        Destroy(tempHold);
        tempHold = null;
            
        _playerBackpack.Clear();
        clientRef.DeactivateUI();
        StartCoroutine(WaitClientGo());
        //controllo sono dal cliente
        // reference prefab the
        // controllo che prefab sia uguale a the cliente
        // 
    }

    IEnumerator WaitClientGo()
    {


        yield return new WaitForSeconds(2f);
        clientRef.ClientLeave();



    }


    IEnumerator ExampleCoroutine(ExecutedAction action)
    {
        if (action == ExecutedAction.Grabbed)
        {
            playerAudio.PlayOneShot(grabAudio);
            if (tempHold == null)
            {
                tempHold = Instantiate(_playerBackpack.objectHolded, hardcodePosition.transform.position, hardcodePosition.transform.rotation);
                tempHold.transform.localScale = new Vector3(10, 10, 10);
                tempHold.SetActive(true);
            }
            else
            {
                Destroy(tempHold);
                tempHold = Instantiate(_playerBackpack.objectHolded, hardcodePosition.transform.position, hardcodePosition.transform.rotation);
                tempHold.SetActive(true);
            }
            
            // tempHold.transform.localScale = new Vector3(_playerBackpack.transform.localScale.x, _playerBackpack.transform.localScale.y, _playerBackpack.transform.localScale.z);
            if (_playerBackpack.bubbleType != BubbleType.None && tempHold.GetComponent<Bubble>() == null)
                tempHold.AddComponent<Bubble>().bubbleType = _playerBackpack.bubbleType;
            if (_playerBackpack.teaSize != TeaSize.None && tempHold.GetComponent<Cup>() == null)
                tempHold.AddComponent<Cup>().teaSize = _playerBackpack.teaSize;
            if (_playerBackpack.teaType != TeaType.None && tempHold.GetComponent<Tea>() == null)
                tempHold.AddComponent<Tea>().teaType = _playerBackpack.teaType;
            _playerBackpack.objectHolded = tempHold;
        }

        yield return new WaitForSeconds(0.2f);
        playerAnimator.SetBool(action.ToString(), false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Client")) return;

        canInteractWihtClient = true;
        Client foundComponent = other.gameObject.GetComponentInParent<Client>() ?? other.gameObject.GetComponentInChildren<Client>();
        if(foundComponent != null)
            clientRef = foundComponent;
    }
}
