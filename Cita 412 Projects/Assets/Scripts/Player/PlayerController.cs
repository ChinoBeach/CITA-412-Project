using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Custom player controller using character controller and unity input action system.
/// Credit to Kurt-Dekker for base code since unity documentation sucks sometimes.
/// https://forum.unity.com/threads/how-to-correctly-setup-3d-character-movement-in-unity.981939/#post-6379746
/// </summary>
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    #region Variables

    // Variables.
    private CharacterController controller;

    // Input action vars.
    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction moveAction;
    private InputAction dashAction;

    // Movement Vars
    private float verticalVelocity;
    private Vector3 prevDir;
    private Vector3 move;
    private bool isPlayerGrounded;

    [Space(10), Header("Ground Variables")]
    [SerializeField, Tooltip("Speed of player on the ground.")] private float groundSpeed = 2f;
    // The hit data of the ground the controller is standing on.
    private ControllerColliderHit groundData = null;


    [Space(10), Header("Sliding Variables")]
    [SerializeField, Tooltip("Speed of player sliding down a slope."), Min(0f)] private float slidingSpeed = 10f;
    [SerializeField, Tooltip("The slope of the ground below the player before they player will slide down it.")] private float slideSlopeLimit = 60f;
    [SerializeField, Tooltip("The slope of the ground where the player loses the ability to jump out of the slope.")] private float maxSlopeLimit = 80f;
    private bool isSliding = false;


    [Space(10), Header("Air Variables")]
    [SerializeField, Tooltip("Amount of time before the player can jump again."), Range(0f, 2f)] private float jumpDelay = .2f;
    [SerializeField, Tooltip("Speed of player in the air.")] private float airSpeed = 2f;
    [SerializeField, Tooltip("Height of jump off the ground."), Min(0f)] private float jumpHeight = 2.0f;
    [SerializeField, Tooltip("Amount of jumps in the air off the ground."), Min(0)] private int maxMultiJumps = 1;
    [SerializeField, Tooltip("Height of jumps in midair."), Min(0)] private float multiJumpHeight = 1.0f;
    [SerializeField, Tooltip("How fast the player is pulled down.")] private float gravity = -9.81f;
    // The current jumps you have.
    private int multiJumpAmount;
    // The current timer counting down before the player can jump.
    private float jumpDelayTimer = 0f;
    // ground timer so the player can go down slopes and stairs without being off the ground.
    private float groundedTimer = 0f;

    [Space(10), Header("Dash Variables")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private Vector3 dashDir;
    [SerializeField] private Vector3 dashVelocity;
    [SerializeField] private bool hasDash = true;
    [SerializeField] private bool isDashing = false;


    [Space(10), Header("Wall Jump Variables")]
    [SerializeField, Tooltip("The strength after jumping off a wall")] private float jumpForce = 5;
    [SerializeField, Tooltip("The amount of degrees off 90 degrees will still count as being able to wall slide.")] private float angleTolerance = 5;
    //Check to see if player is touching a wall, maybe begin a wall slide.
    private bool isTouchingWall = false;

    #endregion Variables

    #region Unity Methods

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        jumpAction = playerInput.actions["Jump"];
        moveAction = playerInput.actions["Move"];
        dashAction = playerInput.actions["Dash"];

        multiJumpAmount = maxMultiJumps;
        groundData = new ControllerColliderHit();
    }

    private void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        // Reset the movement
        move = new Vector3();

        // Apply gravity then check if grounded.
        ApplyGravity();
        CheckIfGrounded();

        // First check if player is sliding
        // If sliding, make them start going down the hill at a sliding speed
        // Give the player one double jump when sliding so they can save themselves
        // If the slope is tagged slippery, make it so jump height is halved or so.

        // Add something so a ground dash works a bit different (LATER!!!!!!)
        if (dashAction.triggered && hasDash && Upgrade.Instance.ownDash)
        {
        }

        if (isDashing)
        {
        }

        // Reduce the delay timer every frame
        if (jumpDelayTimer > 0)
        {
            jumpDelayTimer -= Time.deltaTime;
        }

        if (jumpAction.triggered)
        {
            HandleJump();
        }

        CheckIfSliding();

        // TODO: implement a way to make characters move slower depending on a slope till a point where they will slide down it
        if (isSliding)
        {
            move = CalculateSlidingMovement();
        }
        else
        {
            move = CalculateGroundMovement(moveInput);
            MoveRelativeToCam();
            // Doing the relative cam movement calulation sets y to 0, apply vertical velocity here.
            move.y = verticalVelocity;
        }

        // Apply movement.
        Debug.DrawLine(transform.position, transform.position + move);
        controller.Move(move * Time.deltaTime);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        groundData = hit;
    }

    #endregion Unity Methods

    #region Private Methods

    // Private Methods.

    // TODO: Check if the input is within a radius behind the player (eg. 170 - 190), if its in this range. do a turn around anim.
    // Probably do with the dot product (eg, if the normalized angle we are going dot product the new angle <= -.5)

    // TODO: make this into a calculate x and z and calculate Y so that sliding can skip lateral movement so you can just go down the slope.
    // Slope movement could be able to be nudged left and right.
    private Vector3 CalculateGroundMovement(Vector2 moveInput)
    {
        // Get directional input.
        move = new Vector3(moveInput.x, 0, moveInput.y);

        if (isPlayerGrounded)
        {
            move *= groundSpeed;
        }
        else
        {
            move *= airSpeed;
        }

        // Face the direction we are pointing.
        // TODO: Make a smooth turn algorithem to this so it doesnt just snap directions.
        if (move.magnitude > 0.05f)
        {
            transform.forward = move;
        }

        return move;
    }

    private Vector3 CalculateSlidingMovement()
    {
        // TODO: also add something like the grounded timer for jumping so the player will slide down slopes with even small sections of non steep slope so the player cant glitch out the detection.
        Debug.Log($"Slope: {groundData.normal}");
        Vector3 slideVelocity = Vector3.ProjectOnPlane(new Vector3(0, -slidingSpeed, 0), groundData.normal);
        slideVelocity.y += verticalVelocity;
        return slideVelocity;
    }

    // TODO: maybe make this a coroutine;
    // Stay in dash until it is canceled.
    private void HandleDash()
    {
        // While the dash is still going, Dont change the direction the dash is going.
        dashDir = transform.forward;
        dashVelocity = dashDir * dashSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Handles checking if you can and executing jumps.
    /// </summary>
    private void HandleJump()
    {
        // TODO: Make it so the longer you press space the higher you go to a certain max.

        // When the player jumps, the slope no longer affects them.
        // The easiest way i found to do this is to remove hit from ground data so the slide checker retruns false.
        groundData = new ControllerColliderHit();

        if (jumpDelayTimer > 0)
        {
            // Can't jump yet.
            return;
        }

        if (groundedTimer > 0)
        {
            // When the player jumps on the ground they are no longer grounded, but can still double jump.
            groundedTimer = 0;
            jumpDelayTimer = jumpDelay;
            // Physics dynamics formula for calculating jump velocity based on height and gravity.
            verticalVelocity = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }
        else if (multiJumpAmount > 0)
        {
            multiJumpAmount--;
            jumpDelayTimer = jumpDelay;
            // Physics dynamics formula for calculating jump velocity based on height and gravity.
            verticalVelocity = Mathf.Sqrt(multiJumpHeight * -3 * gravity);
        }

        // Player isnt on the ground and is out of double jumps
    }

    private IEnumerator DashCoroutine()
    {
        yield return new WaitForEndOfFrame();
    }

    private void HandleWalljump()
    {
        throw new System.NotImplementedException();
        // If the player is grounded, they cant do a wall jump.
        // Maybe there can be a little wall hug animation.
    }

    /*private void HandleTurnaround()
    {
        if (Vector3.Dot(prevDir.normalized, move.normalized))
    }*/

    /// <summary>
    /// Handles checking if the player is grounded, as well as handling a few things around touching the ground.
    /// </summary>
    private void CheckIfGrounded()
    {
        isPlayerGrounded = controller.isGrounded;

        if (isPlayerGrounded) //&& !isSliding)
        {
            // cooldown interval to allow reliable jumping even whem coming down ramps
            groundedTimer = 0.2f;
            multiJumpAmount = maxMultiJumps;
        }

        if (groundedTimer > 0)
        {
            groundedTimer -= Time.deltaTime;
        }

        // slam into the ground
        if (isPlayerGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }
    }

    /// <summary>
    /// Handles processes with detecting if the player should be sliding down a slope.
    /// </summary>
    private void CheckIfSliding()
    {
        // TODO: implement that slopes above 80 degreese should be considered very slippery and remove jumps.
        if (groundData.collider == null)
        {
            isSliding = false;
            return;
        }
        // Check if the surface is too steep to stand on and that its tag says the slope wont make the character slip
        // or if the tag says the surface is slippery / very slippery, always slip.
        // Very slippery is a more advanced version of slippery where you cant jump out of it either.
        float angle = Vector3.Angle(groundData.normal, Vector3.up);

        isSliding = (angle > slideSlopeLimit
            && !groundData.collider.CompareTag("NonSlippery"))
            || groundData.collider.CompareTag("Slippery") 
            || groundData.collider.CompareTag("VerySlippery");
        
        // If the ground is very slippery, remove the ability to jump as well so the player continues sliding
        if (groundData.collider.CompareTag("VerySlippery") || angle > maxSlopeLimit)
        {
            // Removing jumps and double jumps;
            multiJumpAmount = 0;
            groundedTimer = 0;
        }
    }
    /// <summary>
    /// Make movement relative to the camera by getting the cameras forward and right.
    /// </summary>
    private void MoveRelativeToCam()
    {
        var cam = Camera.main;

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0;
        forward.Normalize();

        right.y = 0;
        right.Normalize();

        move = forward * move.z + right * move.x;
    }

    /// <summary>
    /// Applies gravity. Simple as that.
    /// </summary>
    private void ApplyGravity()
    {
        // For some reason, this will pull far too hard unless multiplied by Time.deltaTime.
        // However, changing gravity itself messes with the jump function as it refrences gravity for its calculation.
        verticalVelocity += gravity * Time.deltaTime;
    }

    #endregion Private Methods
}