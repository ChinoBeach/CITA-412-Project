using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject target;

    [SerializeField] float speed;
    [SerializeField] float targetResetTimer;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        FollowTarget();
    }

    void OnTriggerStay(Collider collider) {
        // If the collided object is the player
        if (collider.gameObject.tag == "Player") {
            // Stop the coroutine that removes the target object
            StopCoroutine(ResetTarget());

            // If there is not already a target object set the target to the collided object
            if (!target) SetTarget(collider.gameObject);
        }
    }

    void OnTriggerExit(Collider collider) {
        // If the exited object is the target object start timer to reset target variable
        if (collider.gameObject.tag == target.tag) StartCoroutine(ResetTarget());
    }

    void FollowTarget() {
        // Return if there is no target
        if (!target) return;

        // Step speed of the enemy moving towards the target
        float step = speed * Time.deltaTime;

        // Move the object towards the target position
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }

    // Sets the target Game Object
    public void SetTarget(GameObject target) {
        this.target = target;
    }

    // Method to "forget" the target object by setting it to null after a set amount of time
    IEnumerator ResetTarget() {
        // Wait for a specified amount of time
        yield return new WaitForSeconds(targetResetTimer);
        
        // Set target object to null to stop following
        SetTarget(null);
    }
}
