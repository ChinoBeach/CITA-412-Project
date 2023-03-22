using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloater : MonoBehaviour
{
    #region Variables
    //The path that the platform moves across
    [SerializeField] WayPointDirector waypointPath;
    int indexOfTargetWaypoint;
    Transform nextWaypoint;
    Transform previousWaypoint;

    //How much the object will be incremeted in each axis
    [SerializeField] float fltSpeed;
    public Vector3 floaterVelocity;

    //Time Variables, how long until hitting the next waypoint and how long has passed
    float timeToNextPos;
    float timePassed;

    //Variable for if the player is on the platform. 
    bool isPlayerTouching;

    /*END OF VARIABLES/START OF METHODS*/
    #endregion
    #region UnityMethods
    // Start is called before the first frame update
    void Start()
    {
        //get the path started
        SelectNextWayPoint();

        //default player does not start on a platform
        isPlayerTouching = false;
       
    }//end of start method

    // Update is called once per frame
    void FixedUpdate()
    {
        //variable for the percentage of the path that has been completed
        float percentPathCompletion;
        
        /*-------------------------------------------------------------------------------*/

        //increment time passed
        timePassed += Time.deltaTime;
        //calculate how much of the path from one waypoint to the next is completed
        percentPathCompletion = timePassed / timeToNextPos;
        //smooth out the movement closer to the starting and ending points (slows down at the start and end)
        percentPathCompletion = Mathf.SmoothStep(0, 1, percentPathCompletion);

        //set the velocity
        floaterVelocity = Vector3.Lerp(previousWaypoint.position, nextWaypoint.position, percentPathCompletion);

        //if a player is on the platform. 
        if (isPlayerTouching)
        {
            //before moving the platform, disable the player. It will move with the player with it because it is parented.
            
        }

        //move the platform to the next point(smoothly) 
        transform.position = floaterVelocity;

        //if a player is on the platform
        if (isPlayerTouching)
        {
            //after the platform is moved, re enable the player. 
        }
            //rotate the platform to match the points better
            transform.rotation = Quaternion.Lerp(previousWaypoint.rotation, nextWaypoint.rotation, percentPathCompletion);
        //check if the floater has made it to the position(the path between has been completed)
        if (percentPathCompletion >= 1)
        {
            //get the next waypoint on the path
            SelectNextWayPoint();
        }

    }//end of (fixed)update method
    #endregion
    #region WayPoint/FloaterMovement
    // SelectNextWayPoint is called in start to initalize the path, and also in update whenever the floater reaches a waypoint.
    void SelectNextWayPoint()
    {
        //variable for the distance between waypoints
        float distBtwnPnts;
        /*-------------------------------------------------------------------------------*/

        //mark the last met waypoint as the current waypoint
        previousWaypoint = waypointPath.GetCurWaypointIndex(indexOfTargetWaypoint);
        //reset the target waypoint index to the index of the next waypoint in the path
        indexOfTargetWaypoint = waypointPath.GetNextWaypointIndex(indexOfTargetWaypoint);
        //reset the next waypoint to the new next waypoint in the list.
        nextWaypoint = waypointPath.GetCurWaypointIndex(indexOfTargetWaypoint);

        //reset the time passed
        timePassed = 0;

        //calculate the new distance between waypoints
        distBtwnPnts = Vector3.Distance(previousWaypoint.position, nextWaypoint.position);

        //calculate the time to get to the next waypoint postion
        timeToNextPos = distBtwnPnts / fltSpeed;

    }//end of SelectNextWayPoint method
    #endregion
    #region PlayerMovementOnFloater
    // OnTriggerEnter is called when the player steps onto the platform
    private void OnTriggerEnter(Collider other)
    {
        //parent the transform component so that the player will move relative to the floater.
        other.transform.SetParent(transform);

        //turn the players gravity off
       // PlayerController.Instance.gravity = 0;

        //tell the script that the player is on the floater.
        isPlayerTouching = true;

    }//end of OnTriggerEnter method 
    
    // OnTriggerExit is called when the player steps off of the platform
    private void OnTriggerExit(Collider other)
    {
        //set the parent of the transform component back to empty(null) so that it stops moving with the floater.
        other.transform.SetParent(null);

        //tell the script that the player is no longer on the floater.
        isPlayerTouching = false;

    }//end of OnTriggerExitMethod 
    #endregion
}//end of Moving Floater class
