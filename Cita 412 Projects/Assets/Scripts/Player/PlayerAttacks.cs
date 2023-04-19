using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacks : MonoBehaviour
{


    /*----------Variables---------*/

    //point/postion where casting 
    [SerializeField] private Transform pos_castPoint;

    //is there a spell being cast currently
    private bool bol_isCasting = false;

    //player input system
    private PlayerInput playerInput;
    private InputAction castSpellInput;

    // Awake is called before Start
    private void Awake()
    {
        //connect the player input actions to the class
        playerInput = GetComponent<PlayerInput>();
        castSpellInput = playerInput.actions["Spell Cast"];

    }

 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //vairable for if the spell is being held down
        //bool bol_isHoldingCast = playerInput.controls

        //if the player is not casting a spell yet but is pressing the cast button
        if(!bol_isCasting && bol_isHoldingCast) 
        {
            bol_isCasting = true;
            
        }
    }
}
