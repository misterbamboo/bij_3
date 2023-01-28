using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    float maxHealth = 100f;

    float currentHealth = 0f;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Domage(float amount)
    {
        currentHealth -= amount;        
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
