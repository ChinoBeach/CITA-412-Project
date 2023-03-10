using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloater : MonoBehaviour
{
    //how much the object will be incremeted in each axis
    [SerializeField] float fltXspeed;
    [SerializeField] float fltYspeed;
    [SerializeField] float fltZspeed;

    //where the object starts and ends at
    Vector3 vect3StartingPos;
    [SerializeField] Vector3 vect3EndingPos;

    
    /* Might come back to this. Was considering using it for turn around points.
    [SerializeField] float fltMaxX;
    [SerializeField] float fltMinX;
    [SerializeField] float fltMaxY;
    [SerializeField] float fltMinY;
    [SerializeField] float fltMaxZ;
    [SerializeField] float fltMinZ;
    */

    // Start is called before the first frame update
    void Start()
    {
        //get the starting postion 
        vect3StartingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(vect3StartingPos.x != vect3EndingPos.x)
        {
            MoveLeftRight();
        }
        if (vect3StartingPos.y != vect3EndingPos.y)
        {
            MoveUpDown();
        }
        if (vect3StartingPos.z != vect3EndingPos.z)
        {
            MoveInOut();
        }


    }
    //Called when the staring pos x is not equal to the ending 
    void MoveLeftRight()
    {
       
    }
    void MoveUpDown()
    {

    }

    void MoveInOut()
    {

    }

}
