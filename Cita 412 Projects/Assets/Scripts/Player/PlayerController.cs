using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Reworked by @kurtdekker so that it jumps reliably in modern Unity versions.
/// https://forum.unity.com/threads/how-to-correctly-setup-3d-character-movement-in-unity.981939/#post-6379746
/// </summary>
        // TODO, figure out how to make movement relative to the camera +- 15 since its at an angle

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    // Variables.
    private CharacterController controller;
    private float verticalVelocity;
    private float groundedTimer;        // to allow jumping when going down ramps
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = 9.81f;
    [SerializeField] private float rotationSpeed = 3f;

    // Input Actions asset
    public PlayerInputActions playerControls;

    // Movement input direction vector
    Vector2 moveDirection = Vector2.zero;

    // Input action from PlayerInputActions assets
    private InputAction movementInput;
    private InputAction jumpInput;
    #endregion

    #region Unity Methods

    void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    void OnEnable()
    {
        movementInput = playerControls.Player.Move;
        movementInput.Enable();

        jumpInput = playerControls.Player.Jump;
        jumpInput.Enable();
        jumpInput.performed += Jump;
    }

    void OnDisable()
    {
        movementInput.Disable();
        jumpInput.Disable();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            // cooldown interval to allow reliable jumping even whem coming down ramps
            groundedTimer = 0.2f;
        }
        if (groundedTimer > 0)
        {
            groundedTimer -= Time.deltaTime;
        }

        // slam into the ground
        if (groundedPlayer && verticalVelocity < 0)
        {
            // hit ground
            verticalVelocity = 0f;
        }

        // apply gravity always, to let us track down ramps properly
        verticalVelocity -= gravityValue * Time.deltaTime;

        // Get movement input vector2
        moveDirection = movementInput.ReadValue<Vector2>();

        // gather lateral input control
        Vector3 move = new Vector3(moveDirection.x, 0, moveDirection.y);

        // scale by speed
        move *= playerSpeed;

        // only align to motion if we are providing enough input
        if (move.magnitude > 0.05f)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // allow jump as long as the player is on the ground
        // if (Input.GetButtonDown("Jump"))
        // {
        //     // must have been grounded recently to allow jump
        //     if (groundedTimer > 0)
        //     {
        //         // no more until we recontact ground
        //         groundedTimer = 0;

        //         // Physics dynamics formula for calculating jump up velocity based on height and gravity
        //         verticalVelocity = Mathf.Sqrt(jumpHeight * 2 * gravityValue);
        //     }
        // }

        // inject Y velocity before we use it
        move.y = verticalVelocity;

        // call .Move() once only
        controller.Move(move * Time.deltaTime);
    }

    #endregion

    #region Private Methods
    // Private Methods.
    private void Jump(InputAction.CallbackContext context) {
        Debug.Log("Jump");
    }
    
    #endregion

    #region Public Methods
    // Public Methods.
    
    #endregion
}
