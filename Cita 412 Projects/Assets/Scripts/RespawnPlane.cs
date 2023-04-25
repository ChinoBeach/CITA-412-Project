 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlane : MonoBehaviour
{
    #region Variables
    // Variables.
    [SerializeField] private LayerMask mask;
    [SerializeField, Tooltip("The gameobject to instantiate as a respawn point")] private GameObject respawnPoint;
    [SerializeField, HideInInspector] private List<GameObject> respawnPoints;
    [SerializeField] private float respawnTime = 5f;
    [SerializeField, Tooltip("The amount of time before attempting to update the respawn position of the player.")] private float checkTime = 3f;
    private CharacterController player;
    private GameObject nearestPoint;
    float nearestPointDist = -1f;

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (respawnPoints.Count <= 0)
        {
            // Well this is awkward
            Destroy(gameObject);
            return;
        }

        ValidateList();
        nearestPoint = respawnPoints[0];

        player = PlayerController.Instance.controller;

        var coroutine = GetNearestRespawnPoint();

        StartCoroutine(coroutine);
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    foreach (var point in respawnPoints)
    //    {
    //        if (point != nearestPoint) Gizmos.DrawLine(point.transform.position, GameObject.FindObjectOfType<PlayerController>().transform.position);
    //    }

    //    Gizmos.color = Color.green;

    //    Gizmos.DrawLine(nearestPoint.transform.position, GameObject.FindObjectOfType<PlayerController>().transform.position);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Invoke to has a delay before crossfade.
            Invoke("RespawnPlayer", respawnTime);
        }
    }

    #endregion

    #region Private Methods
    // Private Methods.

    private void ValidateList()
    {
        List<GameObject> pointsToDelete = new List<GameObject>();

        foreach (var point in respawnPoints)
        {
            if (point == null)
            {
                pointsToDelete.Add(point);
            }
        }

        foreach (var item in pointsToDelete)
        {
            respawnPoints.Remove(item);
        }
    }

    private void RegenerateNames()
    {
        int i = 0;

        foreach (var point in respawnPoints)
        {
            i++;
            point.name = $"Respawn Point {i}";
        }
    }

    private IEnumerator GetNearestRespawnPoint()
    {
        // Need to wait for a frame for values to be initialized which is dumb.
        yield return new WaitForEndOfFrame();

        while (true)
        {
            // Make sure the player is grounded.
            if (PlayerController.Instance.groundData.collider == null || !PlayerController.Instance.isPlayerGrounded)
            {
                Debug.Log("Unable to update point");
                yield return new WaitForEndOfFrame();
                continue;
            }

            var layer = PlayerController.Instance.groundData.gameObject.layer;

            // Check if the player is on a surface that will update the respawn position.
            if (mask != (mask | (1 << layer)))
            {
                yield return new WaitForSeconds(checkTime);
                continue;
            }

            Debug.Log("Updating point");

            foreach (var point in respawnPoints)
            {
                float dist = point.GetComponent<RespawnPoint>().distToPlayer;

                if (dist < nearestPointDist)
                {
                    nearestPoint = point;
                    nearestPointDist = dist;
                }
            }

            yield return new WaitForSeconds(checkTime);
        }
    }
    #endregion

    #region Public Methods
    // Public Methods.

    public void RespawnPlayer()
    {
        // Do a cross fade anim first
        // Have this be invoked to have a small delay with a crossfade.

        // This is the single dumbest thing I've ever had to do.
        //player.enabled = false;
        Debug.Log("Respawning");
        player.transform.position = nearestPoint.transform.position;
        //player.enabled = true;
    }

    /// <summary>
    /// Creates a new respawn point.
    /// </summary>
    /// <returns>Gameobject just created.</returns>
    public GameObject AddNewRespawnPoint()
    {
        var instance = Instantiate(respawnPoint, transform);
        respawnPoints.Add(instance);
        RegenerateNames();
        ValidateList();
        return instance;
    }

    public void RemoveNewestRespawnPoint()
    {
        // Validate list to not try and remove a null entry.
        ValidateList();

        // Check if the list already has no points.
        if (respawnPoints.Count <= 0)
        {
            // There are no points to delete.
            return;
        }

        var obj = respawnPoints[respawnPoints.Count - 1];
        DestroyImmediate(obj);
        respawnPoints.Remove(obj);

        // Check if the list has no points before trying to regenerate names.
        if (respawnPoints.Count <= 0)
        {
            // There are no points to rename.
            return;
        }

        RegenerateNames();
    }

    public void ClearAllRespawnPoints()
    {
        List<GameObject> children = new List<GameObject>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

        foreach (var child in children)
        {
            DestroyImmediate(child);
        }

        respawnPoints.Clear();
    }
    #endregion
}
