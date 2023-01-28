using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wave
{
    public float timeInSeconds;
    
    public Spawner spawner;

    public List<GameObject> prefabCharacters = new List<GameObject>();

    public void Start()
    {
        
    }

    public void StartWave()
    {
        spawner.GetComponent<Spawner>().StartSpawn(prefabCharacters);
    }
}
