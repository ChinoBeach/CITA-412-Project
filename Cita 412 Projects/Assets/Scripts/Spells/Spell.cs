using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -----Required compnents----- */
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

        //destroy the spell if the time is up (incase it didnt hit anything)
        Destroy(this.gameObject, CurrentSpellCasting.flt_Duration);
    }


    private void Update()
    {
        //if the spell has a movement speed
        if (CurrentSpellCasting.flt_movementSpeed > 0)
        {

            //move the spell forward
            transform.Translate(Vector3.forward * CurrentSpellCasting.flt_movementSpeed * Time.deltaTime);
            Debug.Log(transform.forward);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //If it hits an enemy
        if (other.gameObject.CompareTag("Enemy")) // ||boss enemy :)
        {
            //get the enemies health
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();

            //deal damage
            enemyHealth.DealDamage(CurrentSpellCasting.flt_Damage);

        }
        

        //destroy the object when it hits something
        Destroy(this.gameObject);
    }
}
