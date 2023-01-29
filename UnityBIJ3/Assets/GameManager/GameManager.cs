using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<int> OnSecondElapse = delegate { };

    int currentTimeInSeconds = 0;

    bool barnFound = false;

    void Start()
    {
        StartCoroutine(CalculTimeInSeconds());
    }

    void Update()
    {
        if (!barnFound)
        {
            var barn = GameObject.FindGameObjectWithTag("Barn");
            if (barn != null)
            {
                barn.GetComponentInChildren<Health>().NoMoreHealth += GameOver;
                barnFound = true;
            }
        }
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("HomeScreen", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
