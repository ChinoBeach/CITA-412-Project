using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class RespawnPoint : MonoBehaviour
{
    #region Variables
    // Variables.
    public float distToPlayer;
    private GameObject player;
    [SerializeField, Tooltip("The radius around the point ")] float radius;
    #endregion

    #region Unity Methods

    void Start()
    {
        player = PlayerController.Instance.gameObject;
    }

    private void Update()
    {
        distToPlayer = GetDistanceToPlayer();
    }

    private void OnDrawGizmos()
    {
       Gizmos.DrawLine(gameObject.transform.position, GameObject.FindObjectOfType<PlayerController>().gameObject.transform.position);
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
