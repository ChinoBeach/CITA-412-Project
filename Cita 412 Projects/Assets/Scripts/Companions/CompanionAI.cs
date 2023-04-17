using System.Collections;
using UnityEngine;
// Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
// This line should always be present at the top of scripts which use pathfinding
using Pathfinding;

[HelpURL("http://arongranberg.com/astar/documentation/stable/class_partial1_1_1_astar_a_i.php")]
public class CompanionAI : MonoBehaviour {
    const float MAX_PLAYER_DISTANCE = 75f;
    const float PLAYER_TELEPORT_DISTANCE = 100f;

    [SerializeField] float targetResetTimer;
    [SerializeField] float attackDistance;
    [SerializeField] float playerFollowDistance;

    [SerializeField] LayerMask interactableMask;
    [SerializeField] float interactionPointRadius;
    [SerializeField] Transform interactionPoint;

    private readonly Collider[] colliders =  new Collider[3];

    private bool isAttacking = false;
    private bool isGameOver = false;

    int layerMask;
    [SerializeField] private int numFound;

    Vector3 moveDir;

    public Transform targetPosition;
    public Transform playerPosition;

    private Seeker seeker;
    private CharacterController controller;
    private Animator animator;

    public Path path;

    public float speed = 2;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public float repathRate = 0.5f;
    private float lastRepath = float.NegativeInfinity;

    public bool reachedEndOfPath;

    Damageinator9000 damageinator;

    void OnEnable() {
        PlayerStats.onGameOver += OnGameOver;
    }

    void OnDisable() {
        PlayerStats.onGameOver -= OnGameOver;
    }

    public void Start () {
        damageinator = GetComponentInChildren<Damageinator9000>();
        seeker = GetComponent<Seeker>();
        // If you are writing a 2D game you can remove this line
        // and use the alternative way to move sugggested further below.
        controller = GetComponent<CharacterController>();

        animator = GetComponent<Animator>();

        // Bit shift layer mask to layer 9 to only check for hits on enemy layer
        layerMask = 1 << 9;
    }

    public void OnPathComplete (Path p) {
        // Path pooling. To avoid unnecessary allocations paths are reference counted.
        // Calling Claim will increase the reference count by 1 and Release will reduce
        // it by one, when it reaches zero the path will be pooled and then it may be used
        // by other scripts. The ABPath.Construct and Seeker.StartPath methods will
        // take a path from the pool if possible. See also the documentation page about path pooling.
        p.Claim(this);
        if (!p.error) {
            if (path != null) path.Release(this);
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        } else {
            p.Release(this);
        }
    }

    public void Update () {
        // If the distance between the player and companion is too much set target to player
        if (GetPlayerDistance() >= MAX_PLAYER_DISTANCE) SetTarget(playerPosition);
        if (GetPlayerDistance() >= PLAYER_TELEPORT_DISTANCE) TeleportToPlayer();
        
        // Do nothing if there is no target or the game is over
        if (!targetPosition
            || GetPlayerDistance() <= playerFollowDistance
            || isGameOver) return;

        numFound = Physics.OverlapSphereNonAlloc(
            interactionPoint.position,
            interactionPointRadius,
            colliders,
            interactableMask
        );

        GoToTarget();

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, attackDistance, layerMask) && !isAttacking) {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Attack();
        }
    }

    void OnTriggerStay(Collider collider) {
        // If the collided object is the player
        if (collider.gameObject.CompareTag("Enemy")) {
            // Stop the coroutine that removes the target object
            StopCoroutine(ResetTarget());

            // If there is not already a target object set the target to the collided object
            if (!targetPosition || targetPosition == playerPosition) SetTarget(collider.transform);
        }
    }

    void OnTriggerExit(Collider collider) {
        // If the exited object is the target object start timer to reset target variable
        if (collider.gameObject.tag == targetPosition.tag) StartCoroutine(ResetTarget());
    }

    // Sets the target transform
    public void SetTarget(Transform targetPosition) {
        this.targetPosition = targetPosition;
    }

    // Method to "forget" the target object by setting it to null after a set amount of time
    IEnumerator ResetTarget() {
        // Wait for a specified amount of time
        yield return new WaitForSeconds(targetResetTimer);
        
        // Set target object to player
        SetTarget(playerPosition);
    }

    void GoToTarget() {
        if (Time.time > lastRepath + repathRate && seeker.IsDone()) {
            lastRepath = Time.time;

            // Start a new path to the targetPosition, call the the OnPathComplete function
            // when the path has been calculated (which may take a few frames depending on the complexity)
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
        }

        if (path == null) {
            // We have no path to follow yet, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true) {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance) {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count) {
                    currentWaypoint++;
                } else {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            } else {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint/nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector3 velocity = dir * speed * speedFactor;

        // Move the agent using the CharacterController component
        // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
        controller.SimpleMove(velocity);

        // Look at target so raycast hits
        transform.LookAt(targetPosition);
    }

    void TeleportToPlayer() {
        this.transform.position = playerPosition.position;
    }

    float GetPlayerDistance() {
        return Vector3.Distance(playerPosition.position, transform.position);
    }

    void Attack() {
        isAttacking = true;
        animator.SetTrigger("Base_Attack");
        Debug.Log("Attack target");
    }

    // Resets isAttacking on last frame of attack animation
    void ResetIsAttacking() {
        isAttacking = false;
    }

    // Resets hasDoneDamage on last frame of attack animation
    void ResetHasDoneDamage() {
        damageinator.ResetHasDoneDamage();
    }

    // Called when onGameOver event is invoked
    void OnGameOver() {
        isGameOver = true;
    }

    void OnDrawGizmos() {
        // Draws the interact sphere
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}