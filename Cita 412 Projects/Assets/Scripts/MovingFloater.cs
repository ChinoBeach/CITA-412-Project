using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloater : MonoBehaviour
{
    //The path that the platform moves across
    [SerializeField] WayPointDirector waypointPath;
    int indexOfTargetWaypoint;
    Transform nextWaypoint;
    Transform previousWaypoint;

    //How much the object will be incremeted in each axis
    [SerializeField] float fltSpeed;

    //Time Variables, how long until hitting the next waypoint and how long has passed
    float timeToNextPos;
    float timePassed;

    /*END OF VARIABLES/START OF METHODS*/
  
    // Start is called before the first frame update
    void Start()
    {
        SelectNextWayPoint();
       
    }//end of start method

    // Update is called once per frame
    void FixedUpdate()
    {
      

    }//end of (fixed)update method

    // SelectNextWayPoint is called in start to initalize the path
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
    

}//end of Moving Floater class
