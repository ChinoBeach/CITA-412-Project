using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RespawnPoint : MonoBehaviour
{
    #region Variables
    // Variables.
    float distToPlayer;
    GameObject player;
    [SerializeField, Tooltip("The radius around the point ")] float radius;
    #endregion

    #region Unity Methods

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    private void Update()
    {
        distToPlayer = GetDistanceToPlayer();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(gameObject.transform.position, player.transform.position);
    }

    #endregion

    #region Public Methods
    // Public Methods.
    public float GetDistanceToPlayer()
    {
        return Vector3.Distance(gameObject.transform.position, player.transform.position);
    }
    #endregion
}
