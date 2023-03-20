using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloater : MonoBehaviour
{
    //how much the object will be incremeted in each axis
    [SerializeField] float fltXspeed;
    [SerializeField] float fltYspeed;
    [SerializeField] float fltZspeed;
    

    //where the object starts and ends at, as well as the current position. 
    Vector3 vect3StartingPos;
    Vector3 vect3CurrentPos;
    [SerializeField] Vector3 vect3EndingPos;

    //the starting and ending points can be done as waypoints instead but for now this works.


    //how much of a variation is allowed in positions 
    //***this doesnt really work yet and idk what to make this number
    float acceptedDifference = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        //get the starting position 
        vect3StartingPos = transform.position;
        //get the current position
        vect3CurrentPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //get the current position 
        vect3CurrentPos = transform.position;

        //if the x,y,z of the current coordinates and the strating corrdiates are not equal to the ending posisition coordinates, move to match.
        if (vect3StartingPos.x != vect3EndingPos.x && vect3CurrentPos.x != vect3EndingPos.x)
        {
            MoveLeftRight();
        }
        if (vect3StartingPos.y != vect3EndingPos.y && vect3CurrentPos.y != vect3EndingPos.y)
        {
            MoveUpDown();
        }
        if (vect3StartingPos.z != vect3EndingPos.z && vect3CurrentPos.z != vect3EndingPos.z)
        {
            MoveInOut();
        }

        //when they reach the destination, turn around
        //if(ApporximateEqualVectror3(vect3CurrentPos, vect3EndingPos)) ***this is currently buggy and doesnt work
        if(vect3CurrentPos == vect3EndingPos)
        {
            //we are going to switch the starting and ending positions
            //save the starting position
            Vector3 tempSave = vect3StartingPos;

            //set the starting postion to the ending position
            vect3StartingPos = vect3EndingPos;

            //set the ending position to the previous starting positition
            vect3EndingPos = tempSave;

        }

    }
    //Called when the staring pos x, and the current pos x are not equal to the ending pos x
    void MoveLeftRight()
    {
        if (vect3CurrentPos.x < vect3EndingPos.x)
        {
            transform.Translate(Vector3.right * fltXspeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * fltXspeed * Time.deltaTime);
        }
    
    }

    //Called when the staring pos y, and the current pos y are not equal to the ending pos y
    void MoveUpDown()
    {
        if (vect3CurrentPos.y < vect3EndingPos.y)
        {
            transform.Translate(Vector3.up * fltXspeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * fltXspeed * Time.deltaTime);
        }
    }

    //Called when the staring pos Z, and the current pos z are not equal to the ending pos z
    void MoveInOut()
    {
        if (vect3CurrentPos.z < vect3EndingPos.z)
        {
            transform.Translate(Vector3.forward * fltXspeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.back * fltXspeed * Time.deltaTime);
        }
    }

    //Called when comparing two vector3s
    
    //***Currently Buggy dont use yet. ***
    public bool ApporximateEqualVectror3 (Vector3 vectCompare1, Vector3 vectCompare2 )
    {
        //get the differnce between the two vectors
        var difX = vectCompare1.x - vectCompare2.x;
        var dify = vectCompare1.y - vectCompare2.y;
        var difz = vectCompare1.z - vectCompare2.z;
        
        //check to see if the differnce is greater than that accepted
        if(difX > acceptedDifference || dify > acceptedDifference || difz > acceptedDifference)
        {
            //exit the method with false
            return false;
        }
        
        //else it must be an acceptable distance variaton so exit the method as true.
        return true;
        
    }

    //This method is called when the player steps onto the platform
    private void OnTriggerEnter(Collider other)
    {
        //parent the transform component
        other.transform.SetParent(transform);
    }
    
    //This method is called when the player steps off of the platform
    private void OnTriggerExit(Collider other)
    {
        //set the parent of the transform component back to empty(null)
        other.transform.SetParent(null);
    }

}
