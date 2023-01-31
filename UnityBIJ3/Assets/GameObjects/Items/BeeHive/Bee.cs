using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bee : MonoBehaviour
{
    [SerializeField]
    GameObject beeHive;

    [SerializeField]
    float attackSpeed = 1.0f;

    [SerializeField]
    float Damage = 5.0f;

    [SerializeField]
    float speed = 10.0f;

    GameObject target;

    List<GameObject> enemiesInRange = new List<GameObject>();

    public bool isActive = false;

    void Start()
    {
        gameObject.GetComponentInChildren<DetectionZone>().EnterRange += AddInRange;
        gameObject.GetComponentInChildren<DetectionZone>().ExitRange += RemoveFromRange;
        StartCoroutine(AttackZone());
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, beeHive.transform.position, speed * Time.deltaTime);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public bool NeedNewTarget()
    {
        return target == null;
    }

    public void FilterEnemiesToFarFromBeeHiveRange(List<GameObject> enemiesInBeeHiveRange)
    {
        enemiesInRange = enemiesInRange.Where(e => enemiesInBeeHiveRange.Any(b => b.GetInstanceID() == e.GetInstanceID())).ToList();

        if (target == null)
        {
            target = null;
        }
        else
        {
            if (!enemiesInRange.Any(e => e.GetInstanceID() != target.GetInstanceID()))
            {
                target = null;
            }
        }
    }

    void AddInRange(GameObject enemy)
    {
        enemiesInRange.Add(enemy);
    }

    void RemoveFromRange(GameObject enemy)
    {
        var instanceId = enemy.GetInstanceID();
        var enemyToRemove = enemiesInRange.FirstOrDefault(e => e.GetInstanceID() == instanceId);
        if (enemyToRemove != null)
        {
            enemiesInRange.Remove(enemyToRemove);
        }
    }

    IEnumerator AttackZone()
    {
        if (isActive)
        {
            enemiesInRange = FilterEnemiesInRange();

            foreach (var enemy in enemiesInRange)
            {
                var health = enemy.GetComponent<Health>();
                health.Damage(Damage);
            }
        }

        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(AttackZone());
    }

    List<GameObject> FilterEnemiesInRange()
    {
        return enemiesInRange.Where(e => e != null).ToList();
    }
}
