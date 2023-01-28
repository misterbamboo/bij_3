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
        var barn = GameObject.FindGameObjectWithTag("Barn");
        barn.GetComponentInChildren<Health>().NoMoreHealth += GameOver;
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

    void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
