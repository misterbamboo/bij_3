using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public event Action<int> OnSecondElapse = delegate { };

    int currentTimeInSeconds = 0;

    void Start()
    {
        StartCoroutine(CalculTimeInSeconds());
    }

    // Coroutine augment currenTimeInSeconds at each seconds
    IEnumerator CalculTimeInSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            currentTimeInSeconds++;
            OnSecondElapse(currentTimeInSeconds);
        }
    }
}
