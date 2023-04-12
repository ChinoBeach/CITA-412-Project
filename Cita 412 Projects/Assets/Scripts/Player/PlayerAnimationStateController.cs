using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationStateController : MonoBehaviour
{
    #region Variables
    // Variables.
    Animator animator;
    #endregion

    #region Unity Methods

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
    
    #endregion
}
