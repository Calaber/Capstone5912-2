using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPath : IPatrolPath
{
    private List<Transform> allWaypoints;
    private List<Transform> randomWaypoints;
    private int numWaypoints;
    private System.Random rand;

    public TestPath(Transform[] waypoints)
    {
        rand = new System.Random();
        numWaypoints = waypoints.Length;
        allWaypoints = new List<Transform>(waypoints);
        randamizePath();
    }

    public Transform[] getPath()
    {
        return randomWaypoints.ToArray();
    }

    public void randamizePath()
    {
        List<Transform> allWaypointsCopy = new List<Transform>(allWaypoints);
        randomWaypoints = new List<Transform>();

        for(int i = 0; i < numWaypoints; i++)
        {
            int rnd = rand.Next(0, allWaypointsCopy.Count);
            Transform randT = allWaypointsCopy[rnd];
            randomWaypoints.Add(randT);
            allWaypointsCopy.RemoveAt(rnd);
        }
    }

    public void setNumberOfWayPoints(int waypoints)
    {
        if (waypoints <= allWaypoints.Count)
        {
            numWaypoints = waypoints;
        }else
        {
            numWaypoints = allWaypoints.Count / 2;
        }
    }
}
