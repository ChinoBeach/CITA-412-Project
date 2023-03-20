using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointDirector : MonoBehaviour
{
    //get the current waypoint that the floater is at
    public Transform GetCurWaypointIndex(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    //get the next waypoint in the path
    public int GetNextWaypointIndex(int curWaypointIndex)
    {
        int nextWaypointIndex = curWaypointIndex + 1;

        if(nextWaypointIndex == transform.childCount)
        {
            nextWaypointIndex = 0;
        }

        return nextWaypointIndex;
    }
}

