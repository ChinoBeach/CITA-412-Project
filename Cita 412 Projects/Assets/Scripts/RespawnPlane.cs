using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlane : MonoBehaviour
{
    #region Variables
    // Variables.
    [SerializeField, Tooltip("The gameobject to instantiate as a respawn point")] private GameObject respawnPoint;
    [SerializeField] private List<GameObject> respawnPoints;// = new List<GameObject>();
    [SerializeField] private CharacterController player;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        player = FindObjectOfType<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RespawnPlayer(GetNearestRespawnPoint());
        }
    }

    #endregion

    #region Private Methods
    // Private Methods.

    #endregion

    #region Public Methods
    // Public Methods.
    public GameObject GetNearestRespawnPoint()
    {
        GameObject nearestPoint = respawnPoints[0];
        foreach (var point in respawnPoints)
        {
            nearestPoint = point;
        }
        return nearestPoint;
    }

    public void RespawnPlayer(GameObject respawnPoint)
    {
        // Do a cross fade anim first

        // This is the single dumbest thing I've ever had to do.
        player.enabled = false;
        player.transform.position = respawnPoint.transform.position;
        player.enabled = true;
    }

    public void AddNewRespawnPoint()
    {
        var instance = Instantiate(respawnPoint, transform);
        instance.name = $"Respawn Point {respawnPoints.Count + 1}";
        respawnPoints.Add(instance);
    }
    #endregion
}
