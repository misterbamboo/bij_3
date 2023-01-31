using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    GameObject projectile;
    
    [SerializeField]
    Animator animator;

    [SerializeField]
    float attackCooldownInSeconds = 1f;

    [SerializeField]
    GameObject projectileHolder;

    public event Action Throw = delegate { };

    bool attackOnCooldown = false;

    List<GameObject> enemiesInRange = new List<GameObject>();

    public bool isActive = false;

    Projectile LastSpawnProjectile;

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

    public void ReleaseProjectile()
    {
        LastSpawnProjectile.Release();
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
        if(isActive)
        {
            transform.LookAt(target.transform);
            LastSpawnProjectile = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
            LastSpawnProjectile.Init(target, projectileHolder, this);
            animator.SetBool("Throw", true);
        }
    }

    IEnumerator Cooldown()
    {
        attackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldownInSeconds);
        attackOnCooldown = false;
        animator.SetBool("Throw", false);
    }

    List<GameObject> FilterEnemiesInRange()
    {        
        return enemiesInRange.Where(e => e != null).ToList();
    }
}
