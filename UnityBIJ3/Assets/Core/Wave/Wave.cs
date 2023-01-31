using System;
using UnityEngine;

[Serializable]
public class Wave
{
    [SerializeField] public float timeInSeconds;

    [SerializeField] public Spawner spawner;

    [SerializeField] public GameObject prefab;

    [SerializeField] public int spawnNumber;

    public void StartWave()
    {
        spawner.GetComponent<Spawner>().StartSpawn(prefab, spawnNumber);
    }
}
