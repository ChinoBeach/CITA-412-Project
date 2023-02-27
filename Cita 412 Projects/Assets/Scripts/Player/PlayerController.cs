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
    private Vector2 moveInput = Vector2.zero;

    // Input action from PlayerInputActions assets
    private InputAction movementInput;

    private InputAction jumpInput;

    //assuming we only using the single camera:
    private Camera cam;

    #endregion Variables

    #region Unity Methods

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        movementInput = playerControls.Player.Move;
        movementInput.Enable();

        jumpInput = playerControls.Player.Jump;
        jumpInput.Enable();
        jumpInput.performed += Jump;
    }

    private void OnDisable()
    {
        movementInput.Disable();
        jumpInput.Disable();
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    private void Update()
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
        moveInput = movementInput.ReadValue<Vector2>();

        // gather lateral input control
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);

        //camera forward and right vectors:
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        move = forward * move.z + right * move.x;

        // scale by speed
        move *= playerSpeed;

        // only align to motion if we are providing enough input
        if (move.magnitude > 0.05f)
        {
            Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // inject Y velocity before we use it
        move.y = verticalVelocity;

        // call .Move() once only
        controller.Move(move * Time.deltaTime);
    }

    #endregion Unity Methods

    #region Private Methods

    // Private Methods.
    private void Jump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        // allow jump as long as the player is on the ground
        // must have been grounded recently to allow jump
        if (groundedTimer > 0)
        {
            // no more until we recontact ground
            groundedTimer = 0;

            // Physics dynamics formula for calculating jump up velocity based on height and gravity
            verticalVelocity = Mathf.Sqrt(jumpHeight * 2 * gravityValue);
        }
    }

    private Vector3 CalculateLateralMovement()
    {
        //camera forward and right vectors:
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0;
        forward.Normalize();

        right.y = 0;
        right.Normalize();

        var forwardRelative = moveInput.y * forward;
        var rightRelative = moveInput.x * right;

        var relativeMove = forwardRelative + rightRelative;

        return new Vector3(relativeMove.z, 0, relativeMove.x);
    }

    #endregion Private Methods

    #region Public Methods

    // Public Methods.

    #endregion Public Methods
}