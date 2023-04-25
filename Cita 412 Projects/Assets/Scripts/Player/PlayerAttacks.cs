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
    private PlayerInputActions playerInput;
    private InputAction castSpellInput;

    //spell casting timers
    [SerializeField] private float flt_maxSpellTime = .25f;
    private float flt_currentTimeCasting; 

    // Awake is called before Start
    private void Awake()
    {
        //connect the player input actions to the class
        playerInput = new PlayerInputActions();
        castSpellInput = playerInput.FindAction("Spell Cast");

    }// end awake method

    // OnEnable is called to turn on player input actions
    private void OnEnable()
    {
        playerInput.Enable();
        
    }// end OnEnable method

    //OnDisable is called to turn off player input actions
    private void OnDisable()
    {
        playerInput.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        //vairable for if the spell is being held down
        bool bol_isHoldingCast = castSpellInput.ReadValue<float>() > 0.1;

        //if the player is not casting a spell yet but is pressing the cast button
        if(!bol_isCasting && bol_isHoldingCast) 
        {
            //start casting
            bol_isCasting = true;
            //set the time casting to 0 because we just started the cast
            flt_currentTimeCasting = 0;
            
        }
        //if you are casting
        if(bol_isCasting)
        {
            //keep track of how long youve been casting the spell
            flt_currentTimeCasting += Time.deltaTime;
            
            //if you reach the end of max time casting
            if(flt_currentTimeCasting > flt_maxSpellTime)
            {
                //stop the cast
                bol_isCasting = false;
            }
        }

    }// end of Update method

    // Cast Spell is called when the player presses the spell attack button
    void CastSpell()
    {
        //cast the spell

    }//end castspell method

}
