using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    List<Wave> waves = new List<Wave>();

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
            }
        }
    }
}
