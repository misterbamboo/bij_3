using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public event Action<int> WaveSpawned = delegate { };

    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    List<Wave> waves = new List<Wave>();

    int currentWaveIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.OnSecondElapse += CheckWave;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckWave(int currentTimeInSeconds)
    {
        foreach (Wave wave in waves)
        {
            if (wave.timeInSeconds == currentTimeInSeconds)
            {
                wave.StartWave();
                currentWaveIndex++;
                WaveSpawned(currentWaveIndex);
            }
        }
    }

    public int GetWaveCount()
    {
        return waves.Count;
    }
}
