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
    [SerializeField, Tooltip("Speed of player on the ground.")] private float groundSpeed = 2.0f;
    private ControllerColliderHit groundData;


    [Space(10), Header("Sliding Variables")]
    [SerializeField, Tooltip("Speed of player sliding down a slope.")] private float slidingSpeed = 1.0f;
    private bool isSliding = false;
    // The hit data of the slope the controller is standing on.
    [SerializeField, Tooltip("Speed of player sliding down a slope.")] private float groundDetectionLength = 100f;


    [Space(10), Header("Air Variables")]
    [SerializeField, Tooltip("Amount of time before the player can jump again."), Range(0f, 2f)] private float jumpDelay = .2f;
    [SerializeField, Tooltip("Speed of player in the air.")] private float airSpeed = 2.0f;
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
    [SerializeField, Tooltip("Checking if the player is touching a wall")] private bool isTouchingWall = false;

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
    }

    private void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        // Reset the movement
        move = new Vector3();

        // Apply gravity then check if grounded.
        ApplyGravity();
        HandleGrounded();

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

        move = CalculateMovement(moveInput);
        MoveRelativeToCam();


        // Reduce the delay timer every frame
        if (jumpDelayTimer > 0)
        {
            jumpDelayTimer -= Time.deltaTime;
            return;
        }

        if (jumpAction.triggered)
        {
            //Debug.LogWarning("JumpInput");
            HandleJump();
            //Debug.Log(verticalVelocity);
            //Debug.Break();
        }

        // Doing the relative cam movement calulation sets y to 0, apply vertical velocity here.
        move.y = verticalVelocity;
        
        // Apply movement.
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
    private Vector3 CalculateMovement(Vector2 moveInput)
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
        // TODO: Make a smoot turn algorithem to this so it doesnt just snap directions.
        if (move.magnitude > 0.05f)
        {
            transform.forward = move;
        }

        return move;
    }

    // TODO: maybe make this a coroutine;
    // Stay in dash until it is canceled.
    private void HandleDash()
    {
        // While the dash is still going, Dont change the direction the dash is going.
        dashDir = transform.forward;
        dashVelocity = dashDir * dashSpeed * Time.deltaTime;
    }

    private void HandleJump()
    {
        // TODO: Make it so the longer you press space the higher you go to a certain max.
        Debug.Log("Handling jump");
        Debug.Log(verticalVelocity);

        if (groundedTimer > 0)
        {
            Debug.Log("Jump");
            // When the player jumps on the ground they are no longer grounded, but can still double jump.
            groundedTimer = 0;
            jumpDelayTimer = jumpDelay;
            // Physics dynamics formula for calculating jump velocity based on height and gravity.
            verticalVelocity = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }
        else if (multiJumpAmount > 0)
        {
            Debug.Log("Double jump");

            multiJumpAmount--;
            jumpDelayTimer = jumpDelay;
            // Physics dynamics formula for calculating jump velocity based on height and gravity.
            verticalVelocity = Mathf.Sqrt(multiJumpHeight * -3 * gravity);
        }
        // Player isnt on the ground and is out of double jumps
        Debug.Log(verticalVelocity);
    }

    private IEnumerator DashCoroutine()
    {
        yield return new WaitForEndOfFrame();
    }

    private void HandleWalljump()
    {
    }

    /*private void HandleTurnaround()
    {
        if (Vector3.Dot(prevDir.normalized, move.normalized))
    }*/

    private void HandleGrounded()
    {
        isPlayerGrounded = controller.isGrounded;

        // If the player is in the air, use a different air speed, and give a bit of velocity so you dont just stop midair
        if (isPlayerGrounded && !isSliding)
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
            //Debug.Log("Landed");
            // hit ground
            verticalVelocity = 0f;
        }
    }

    private void CheckSliding()
    {
        //var rayDistance = 100f;

        //if (Physics.Raycast(transform.position, -Vector3.up, out groundData, rayDistance))
        //{
        //    // Use ControllerColliderHit to get exact point where we are touching the ground
        //    //bool sliding |= (Vector3.Angle(hit.normal, Vector3.up) > controller.slideLimit && CanSlide() && hit.collider.tag != “NoSlide”);
        //    // TODO: Add ability to make the player slide regardless if a surface has a tag akin to "Slippery".
        //    if (angle >= controller.slopeLimit)
        //    {
        //        // TODO: set move equal to this so they slide down it
        //        isSliding = true;
        //        // TODO: make this only true if slippery slope.
        //        // Player is considered ungrounded if the slope is considered slippery
        //        groundedTimer = 0;
        //        multiJumpAmount = 0;
        //        return;
        //    }
        //}

        isSliding = false;
    }

    private Vector3 CalculateSlidingMovement()
    {
        // TODO: enable ability so
        return Vector3.ProjectOnPlane(new Vector3(0, slidingSpeed, 0), groundData.normal);
    }

    private void MoveRelativeToCam()
    {
        // Make movement relative to the camera.
        var cam = Camera.main;

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0;
        forward.Normalize();

        right.y = 0;
        right.Normalize();

        move = forward * move.z + right * move.x;
    }

    private void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
    }

    #endregion Private Methods
}