using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AOEItem : MonoBehaviour
{
    [SerializeField]
    float attackSpeed = 1.0f;

    [SerializeField]
    float damage = 5.0f;

    [SerializeField]
    ParticleSystem particleSystem;

    List<GameObject> enemiesInRange = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<DetectionZone>().EnterRange += AddInRange;
        gameObject.GetComponentInChildren<DetectionZone>().ExitRange += RemoveFromRange;
        StartCoroutine(AttackZone());
    }

    IEnumerator AttackZone()
    {           
        particleSystem.Play();
        enemiesInRange = FilterEnemiesInRange();

        foreach(var enemy in enemiesInRange)
        {
            var health = enemy.GetComponent<Health>();
            health.Damage(damage);
        }

        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(AttackZone());
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

    List<GameObject> FilterEnemiesInRange()
    {
        return enemiesInRange.Where(e => e != null).ToList();
    }
}
