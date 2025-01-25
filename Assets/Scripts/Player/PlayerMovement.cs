using System;
using System.Collections;
using Player;
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
    private Animator playerAnimator;

    //test
    // [SerializeField]
    // private  GameObject objectTest;
    // [SerializeField]
    // private GameObject hardcodePosition;

    private bool canMove;
    private PlayerBackpack _playerBackpack;
    private Stations _stations;
    
    private void Awake()
    {
        _playerBackpack = GetComponent<PlayerBackpack>();
        _stations = FindObjectOfType<Stations>();
    }
    
    private void OnEnable()
    {
        playerControls.Enable();
        interaction.Enable();
        interaction.performed += Interact;

    }


    private void OnDisable()
    {
        playerControls.Disable();
        interaction.Disable();
        interaction.performed -= Interact;
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



    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (_playerBackpack.isHoldingObject || !canGrab) return;
        
        //print("Interaction performed");
        playerAnimator.SetBool("grab", true);
        playerAnimator.SetFloat("velocity", 0);
        // objectTest.transform.position = hardcodePosition.transform.position;
        // objectTest.transform.SetParent(hardcodePosition.transform);
        StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        playerAnimator.SetBool("grab", false);
        _stations.GetActiveStation().GrabItem();
    }
}
