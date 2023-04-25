using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]

public class SpellScriptableObject : ScriptableObject
{
    /*-----Variables-----*/
    public float flt_Damage = 10f;
    public float flt_ManaCost = 5f;
    public float flt_Duration = 2f;
    public float flt_movementSpeed = 15f;
    public float flt_SpellRadius = 0.5f;
  
}// end of class 
