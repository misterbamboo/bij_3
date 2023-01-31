using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField]
    Canvas canvas;
    
    [SerializeField]
    Slider slider;

    bool coroutineStarted = false;

    public void Start()
    {
        var health = GetComponentInParent<Health>();
        health.HealthUpdate += UpdateBar;

        canvas.enabled = false;
    }

    public void UpdateBar(float value)
    {
        canvas.enabled = true;
        slider.value = (value / 100);

        if (!coroutineStarted)
        {
            StartCoroutine(HideBar());
        }
        else
        {
            StopCoroutine(HideBar());
            StartCoroutine(HideBar());
        }
    }

    IEnumerator HideBar()
    {
        coroutineStarted = true;
        yield return new WaitForSeconds(1.0f);
        canvas.enabled = false;
    }

    // update slider color
    public void Update()
    {
        if (slider.value > 0.75f)
        {
            slider.fillRect.GetComponent<Image>().color = Color.green;
        }
        else if (slider.value > 0.5f)
        {
            slider.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            slider.fillRect.GetComponent<Image>().color = Color.red;
        }
    }
}
