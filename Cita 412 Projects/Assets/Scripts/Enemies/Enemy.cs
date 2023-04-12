using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    AstarAI AIScript;

    [SerializeField] float speed;
    [SerializeField] float targetResetTimer;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        AIScript = GetComponent<AstarAI>();
    }

    void OnTriggerStay(Collider collider) {
        // If the collided object is the player
        if (collider.gameObject.CompareTag("Player")) {
            // Stop the coroutine that removes the target object
            StopCoroutine(ResetTarget());

            AIScript.enabled = true;
            // If there is not already a target object set the target to the collided object
            if (!AIScript.targetPosition) SetTarget(collider.transform);
        }
    }

    void OnTriggerExit(Collider collider) {
        // If the exited object is the target object start timer to reset target variable
        if (collider.gameObject.tag == AIScript.targetPosition.tag) StartCoroutine(ResetTarget());
    }

    // Sets the target transform
    public void SetTarget(Transform target) {
        AIScript.targetPosition = target;
    }

    // Method to "forget" the target object by setting it to null after a set amount of time
    IEnumerator ResetTarget() {
        // Wait for a specified amount of time
        yield return new WaitForSeconds(targetResetTimer);
        
        // Set target object to null to stop following
        SetTarget(null);
        AIScript.enabled = false;
    }
}
