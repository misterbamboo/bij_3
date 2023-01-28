using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<float> HealthUpdate = delegate { };
    public event Action NoMoreHealth = delegate { };

    [SerializeField]
    float maxHealth = 100f;

    float currentHealth = 0f;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float amount)
    {
        currentHealth -= amount;        

        HealthUpdate(currentHealth);
        if(currentHealth <= 0)
        {
           NoMoreHealth();
        }
    }
}
