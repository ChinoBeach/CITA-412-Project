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
        //get the starting position 
        vect3StartingPos = transform.position;
        //get the curreny position
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


    }
    //Called when the staring pos x is not equal to the ending 
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

}
