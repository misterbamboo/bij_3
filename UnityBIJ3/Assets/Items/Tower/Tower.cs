using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    GameObject projectile;
    
    [SerializeField]
    float attackCooldownInSeconds = 1f;

    bool attackOnCooldown = false;

    List<GameObject> enemiesInRange = new List<GameObject>();

    void Start()
    {
        gameObject.GetComponentInChildren<DetectionZone>().EnterRange += AddInRange;
        gameObject.GetComponentInChildren<DetectionZone>().ExitRange += RemoveFromRange;
    }

    // Update is called once per frame
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
            Attack(enemiesInRange[0]);
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

    void Attack(GameObject target)
    {
        var newPojectile = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        newPojectile.Init(target);
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
