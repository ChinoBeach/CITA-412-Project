using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Spell : MonoBehaviour
{
    /* ----- Variables ----- */
    
    //spell data
    public SpellScriptableObject CurrentSpellCasting;

    //spell physics
    private SphereCollider spellCollider;
    private Rigidbody spellRigidbody;

    private void Awake()
    {
        //set the collider up
        spellCollider = GetComponent<SphereCollider>();
        spellCollider.radius = CurrentSpellCasting.flt_SpellRadius;
        //make it so that the collider can trigger to deal damage
        spellCollider.isTrigger = true;

        //set the rigidbody up
        spellRigidbody = GetComponent<Rigidbody>();
        spellRigidbody.isKinematic = true;
    }


    private void Update()
    {
        //if the spell has a movement speed
        if (CurrentSpellCasting.flt_movementSpeed > 0)
        {
            //move the spell forward
            transform.Translate(transform.forward * CurrentSpellCasting.flt_movementSpeed * Time.deltaTime;

        }

    }
}
