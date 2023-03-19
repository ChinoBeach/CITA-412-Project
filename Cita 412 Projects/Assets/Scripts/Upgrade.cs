using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: replace this script with a text file to be refrenced for atributes.
public class Upgrade : MonoBehaviour
{
    #region Variables
    // Variables.
    public static Upgrade Instance { get; private set; }

    // This probably isn't the best way to do this.
    // Too bad!
    public bool ownDash = false;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Singleton moment.

        if (Instance != null && Instance != this)
        {
            // That ain't Drake.
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        
    }

    #endregion

    #region Private Methods
    // Private Methods.
    
    #endregion

    #region Public Methods
    // Public Methods.
    public void UnlockBetterMovement()
    {

    }
    #endregion
}
