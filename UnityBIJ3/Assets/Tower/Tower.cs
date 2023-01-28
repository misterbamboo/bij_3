using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField]
    GameObject projectile;
    
    [SerializeField]
    float cooldownFireInSeconds = 1f;

    bool isAvailable = true;

    List<GameObject> enemiesInRange = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<DetectionZone>().EnemyEnterRange += AddInRange;
        gameObject.GetComponentInChildren<DetectionZone>().EnemyOutOfRange += RemoveFromRange;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAvailable)
            return;

        
        if(enemiesInRange.Count > 0 && enemiesInRange[0] == null)
        {
            enemiesInRange.RemoveAt(0);
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
        newPojectile.Init(gameObject, target);
    }

    // wait for cooldown duration
    IEnumerator Cooldown()
    {
        isAvailable = false;
        yield return new WaitForSeconds(cooldownFireInSeconds);
        isAvailable = true;
    }
}
