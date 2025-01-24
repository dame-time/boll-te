using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private Rigidbody rigibody;

    [SerializeField]
    private float speed;

    private Vector2 moveDirection;

    [SerializeField]
    private InputAction playerControls;

    [SerializeField]
    private Animator playerAnimator;

    private void OnEnable()
    {
        playerControls.Enable();

    }
    private void OnDisable()
    {
        playerControls.Disable();
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
        rigibody.velocity = new Vector3(moveDirection.x * speed, 0, moveDirection.y * speed);
        playerAnimator.SetFloat("velocity", rigibody.velocity.magnitude);

    }
}
