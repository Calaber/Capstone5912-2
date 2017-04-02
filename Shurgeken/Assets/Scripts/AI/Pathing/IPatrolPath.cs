using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrolPath {

    Transform[] getPath();

    Transform getCurrentWaypoint();

    void advanceWaypoint();

    void setNumberOfWayPoints(int waypoints);

    void randamizePath();
}
