using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeeHive : MonoBehaviour
{
    [SerializeField]
    Bee bee;

    List<GameObject> enemiesInRange = new List<GameObject>();

    void Start()
    {
        gameObject.GetComponentInChildren<DetectionZone>().EnterRange += AddInRange;
        gameObject.GetComponentInChildren<DetectionZone>().ExitRange += RemoveFromRange;
    }

    void Update()
    {
        if(enemiesInRange.Count > 0)
        {
            FilterEnemiesInRange();
        }
        
        if(enemiesInRange.Count > 0 && bee.NeedNewTarget())
        {           
            OrderListByClosestToBees();
            SendBeeToTarget(enemiesInRange[0]);
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
        bee.FilterEnemiesToFarFromBeeHiveRange(enemiesInRange);
    }

    void SendBeeToTarget(GameObject target)
    {
        bee.SetTarget(target);
    }

    void FilterEnemiesInRange()
    {        
        enemiesInRange = enemiesInRange.Where(e => e != null).ToList();
    }

    void OrderListByClosestToBees()
    {
        enemiesInRange = enemiesInRange.OrderBy(e => Vector3.Distance(e.transform.position, bee.gameObject.transform.position)).ToList();
    }
}
