using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WolfTrap : MonoBehaviour
{
    [SerializeField]
    float domage = 100.0f;

    [SerializeField]
    float attackCooldownInSeconds = 10.0f;

    bool attackOnCooldown = false;

    List<GameObject> enemiesInRange = new List<GameObject>();

    void Start()
    {
        gameObject.GetComponentInChildren<DetectionZone>().EnterRange += AddInRange;
        gameObject.GetComponentInChildren<DetectionZone>().ExitRange += RemoveFromRange;
    }

    void Update()
    {
        if(attackOnCooldown)
            return;
        
        if(enemiesInRange.Count > 0)
        {
            enemiesInRange = FilterEnemiesInRange();
        }
        
        if(enemiesInRange.Count > 0)
        {           
            AttackZone();
            StartCoroutine(Cooldown());
        }
    }

    void AddInRange(GameObject enemy)
    {
        enemiesInRange.Add(enemy);
    }

    void RemoveFromRange(GameObject enemy)
    {
        var instanceId = enemy.GetInstanceID();
        var enemyToRemove = enemiesInRange.First(e => e.GetInstanceID() == instanceId);
        enemiesInRange.Remove(enemyToRemove);
    }

    void AttackZone()
    {
        enemiesInRange = FilterEnemiesInRange();

        foreach(var enemy in enemiesInRange)
        {
            var health = enemy.GetComponent<Health>();
            health.Damage(domage);
        }
    }

    IEnumerator Cooldown()
    {
        attackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldownInSeconds);
        attackOnCooldown = false;
    }

    List<GameObject> FilterEnemiesInRange()
    {
        return enemiesInRange.Where(e => e != null).ToList();
    }
}
