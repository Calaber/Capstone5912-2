using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSpawner : MonoBehaviour {

    public GameObject FlagPrefab;
    static GameObject Flag;
    static List<FlagSpawner> Instances;
    
	void Start () {
        if (Instances == null){ Instances = new List<FlagSpawner>(); }
        Instances.Add(this);
        Flag = FlagPrefab;
    }

    public static GameObject SpawnFlag() {
        Transform random_transform = Instances[Random.Range(0, Instances.Count)].transform;
        return GameObject.Instantiate(Flag, random_transform);
    }
}
