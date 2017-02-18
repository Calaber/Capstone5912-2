using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface DataControllerInter {
    void writeData(PhotonStream writer);
    void readData(PhotonStream reader);
}
