using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Simply sets the follow cam, as well as making it a singleton.
/// </summary>
public class SetFollowCam : MonoBehaviour
{
    #region Variables
    // Variables.
    public static SetFollowCam Instance { get; private set; }
    [SerializeField] CinemachineVirtualCamera vcam;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Singleton moment.

        if (Instance != null && Instance != this)
        {
            // That ain't Drake.
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Invoke so the game has a slight amount of time to process deletion of the character and reloading.
        Invoke("SetVcam", .1f);
    }

    #endregion

    #region Private Methods
    
    private void SetVcam()
    {
        vcam.Follow = PlayerController.Instance.gameObject.transform;
        vcam.LookAt = PlayerController.Instance.gameObject.transform;
    }

    #endregion
}
