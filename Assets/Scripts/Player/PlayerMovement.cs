using System;
using System.Collections;
using Clients;
using Clients.Orders;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [SerializeField]
    private Animator playerAnimator;

    public bool canInteractWihtClient=false;
    public Client clientRef;

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

    private void Awake()
    {
        _playerBackpack = GetComponent<PlayerBackpack>();
        _stations = FindObjectOfType<Stations>();
    }

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
        objectTest.transform.position = hardcodePosition.transform.position;
        objectTest.transform.SetParent(hardcodePosition.transform);
        objectinHandTest = true;
        playerAnimator.SetBool("Grabbed", true);
        StartCoroutine(ResetGrab());
    }

    IEnumerator ResetGrab()
    {
        yield return new WaitForSeconds(0.2f);
        playerAnimator.SetBool("Grabbed", false);
    }



    // Update is called once per frame
    void Update()
    {
        moveDirection = playerControls.ReadValue<Vector2>();


        if (moveDirection != Vector2.zero) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.y));

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }

    }

    private void FixedUpdate()
    {

        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("AN_Gigino_Grab"))
        {
            //print("playing animation");
            rigibody.velocity = new Vector3(moveDirection.x * speed, 0, moveDirection.y * speed);
            playerAnimator.SetFloat("velocity", rigibody.velocity.magnitude);

        }
        else
        {
            rigibody.velocity = Vector3.zero;
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("AN_Gigino_GrabLoop") && tempHold)
        {
            tempHold.transform.position = hardcodePosition.transform.position;
            tempHold.transform.SetParent(hardcodePosition.transform);
        }



    }

    private void Interact(InputAction.CallbackContext context)
    {
        //print("Interaction performed");
        // objectTest.transform.position = hardcodePosition.transform.position;
        // objectTest.transform.SetParent(hardcodePosition.transform);
        if (_stations.GetActiveStation() == null) return;

        var action = _stations.GetActiveStation().ExecuteAction();

        print($"call action = {action.ToString()}");
        if (action == ExecutedAction.None) return;
        
        playerAnimator.SetBool(action.ToString(), true);
        playerAnimator.SetFloat("velocity", 0);
        
        StartCoroutine(ExampleCoroutine(action));
    }

    public void Drop()
    {
        tempHold.SetActive(false);
        tempHold = null;
    }

    private void InteractionClient(InputAction.CallbackContext context)
    {
        print("performed client interaction");
        print("canInteractWihtClient " + canInteractWihtClient);
        print("clientRef: " + clientRef);
        if (canInteractWihtClient && clientRef && objectinHandTest)
        {
            print("all set to client");
            
            print("Object in hand is: " + objectTest.gameObject.name);
            print("Object client wants is: " + clientRef.currentThe.name);

            if (objectTest.gameObject.name.Equals(clientRef.currentThe.name))
            {
                print("good client is happy");
                print($"client gives to you {clientRef.currentThe.value} $ ");
            }
            else
            {
                print("bad client is angry");

            }
            clientRef.ClientLeave();
        }
        //controllo sono dal cliente
        // reference prefab the
        // controllo che prefab sia uguale a the cliente
        // 
    }

    IEnumerator ExampleCoroutine(ExecutedAction action)
    {
        if (action == ExecutedAction.Grabbed)
        {
            if (tempHold == null)
            {
                tempHold = Instantiate(_playerBackpack.objectHolded, hardcodePosition.transform.position, hardcodePosition.transform.rotation);
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
        print("TRIGEERRRRR");
        if (!other.CompareTag("Client")) return;

        canInteractWihtClient = true;
        Client foundComponent = other.gameObject.GetComponentInParent<Client>() ?? other.gameObject.GetComponentInChildren<Client>();
        if(foundComponent != null)
        {
            clientRef = foundComponent;
        }
    }
}
