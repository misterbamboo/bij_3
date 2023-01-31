using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WavesCounter : MonoBehaviour
{
    [SerializeField]
    WaveManager waveManager;
    
    [SerializeField] 
    TextMeshProUGUI counterLabel;

    // Start is called before the first frame update
    void Start()
    {
        waveManager.WaveSpawned += UpdateCounter;
        counterLabel.text = formatCounter(0);
    }

    void UpdateCounter(int currentWaveIndex)
    {
        counterLabel.text = formatCounter(currentWaveIndex);
    }

    string formatCounter(int currentWaveIndex)
    {
        return $"{currentWaveIndex}/{waveManager.GetWaveCount()}";
    }
}
