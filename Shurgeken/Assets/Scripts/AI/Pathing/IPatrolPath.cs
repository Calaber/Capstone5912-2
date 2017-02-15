using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPatrolPath {

    Transform[] getPath();

    void setNumberOfWayPoints(int waypoints);

    void randamizePath();
}
