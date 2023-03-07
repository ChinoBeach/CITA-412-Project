using System.Collections;
using System.Collections.Generic;
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

    // Input action vars
    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction moveAction;
    private InputAction dashAction;

    private float verticalVelocity;
    private Vector3 prevDir;

    [Space(10), Header("Speed Variables")]
    [SerializeField, Tooltip("Speed of player on the ground.")] private float groundSpeed = 2.0f;
    [SerializeField, Tooltip("Speed of player in the air.")] private float airSpeed = 2.0f;


    [Space(10), Header("Jump Variables")]
    [SerializeField, Tooltip("Amount of time before the player can jump again."), Range(0f, 2f)] private float jumpDelay = .2f;
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
    #endregion

    #region Unity Methods

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        jumpAction = playerInput.actions["Jump"];
        moveAction = playerInput.actions["Move"];
        dashAction = playerInput.actions["Dash"];

        multiJumpAmount = maxMultiJumps;
    }

    void Update()
    {
        if (dashAction.triggered && hasDash && Upgrade.Instance.ownDash)
        {

        }

        if (isDashing)
        {

        }
        // Apply movement.
        controller.Move(CalculateMovement() * Time.deltaTime);
    }

    #endregion

    #region Private Methods
    // Private Methods.

    // TODO: Check if the input is within a radius behind the player (eg. 170 - 190), if its in this range. do a turn around anim.
    // Probably do with the dot product (eg, if the normalized angle we are going dot product the new angle <= -.5)
    private Vector3 CalculateMovement()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        //TODO: Only set isPlayerGrounded if surface has less than a maximum slope

        // TODO: make this find if player is grounded with a raycast to check the slope of the floor by normal.
        bool isPlayerGrounded = controller.isGrounded;


        // If the player is in the air, use a different air speed, and give a bit of velocity so you dont just stop midair
        if (isPlayerGrounded)
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
            Debug.Log("Landed");
            // hit ground
            verticalVelocity = 0f;
        }

        // Apply gravity always to track down ramps better.
        verticalVelocity += gravity * Time.deltaTime;

        // Get directional input.
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move *= groundSpeed;


        // Make movement relative to the camera.
        var cam = Camera.main;

        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0;
        forward.Normalize();

        right.y = 0;
        right.Normalize();

        move = forward * move.z + right * move.x;

        // Face the direction we are pointing.
        // TODO: Make a smoot turn algorithem to this so it doesnt just snap directions.
        if (move.magnitude > 0.05f)
        {
            transform.forward = move;
        }

        // Handle Jumping.
        // TODO: Make it so the longer you press space the higher you go to a certain max.
        // This is to make it so you can do small hops or large leaps.
        if (jumpDelayTimer > 0)
        {
            jumpDelayTimer -= Time.deltaTime;
        }

        if (jumpAction.triggered)
        {
            Debug.Log("Jump input");
            HandleJump();
        }


        // Append velocity to move.
        move.y = verticalVelocity;

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
        if(jumpDelayTimer > 0)
        {
            return;
        }

        if (groundedTimer > 0)
        {
            Debug.Log("Jump");
            // When the player jumps on the ground they are no longer grounded, but can still double jump.
            groundedTimer = 0;
            jumpDelayTimer = .3f;
            // Physics dynamics formula for calculating jump velocity based on height and gravity.
            verticalVelocity = Mathf.Sqrt(jumpHeight * -3 * gravity);
        }

        else if (multiJumpAmount > 0)
        {
            Debug.Log("Double jump");
            multiJumpAmount--;

            // Physics dynamics formula for calculating jump velocity based on height and gravity.
            verticalVelocity = Mathf.Sqrt(multiJumpHeight * -3 * gravity);
        }
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
    #endregion

    #region Public Methods
    // Public Methods.

    #endregion
}
