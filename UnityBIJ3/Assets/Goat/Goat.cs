using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Goat : MonoBehaviour
{
    [SerializeField]
    float domage = 10f;

    [SerializeField]
    float speed = 10f;

    [SerializeField]
    float attackCooldownInSeconds = 1f;

    GameObject target = null;

    bool attackOnCooldown = false;

    List<GameObject> goatsBlocking = new List<GameObject>();

    void Start()
    {
        var detectionZones = GetComponentsInChildren<DetectionZone>();
        
        detectionZones.First(d => d.name == "DetectionZone").EnterRange += FocusTarget;
        detectionZones.First(d => d.name == "DetectionZone").ExitRange += UnfocusTarget;

        detectionZones.First(d => d.name == "GoatDetectionZone").EnterRange += BlockByGoat;
        detectionZones.First(d => d.name == "GoatDetectionZone").ExitRange += UnblockByGoat;

        gameObject.GetComponentInChildren<Health>().NoMoreHealth += Die;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Attack();
        }
        else if(goatsBlocking.Count == 0)
        {
            MoveForward();
        }
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    void FocusTarget(GameObject target)
    {
        this.target = target;
        this.target.GetComponent<Health>().NoMoreHealth += FocusDead;
    }

    void UnfocusTarget(GameObject target)
    {
        if(this.target == null)
        {
            return;
        }
            
        this.target.GetComponent<Health>().NoMoreHealth -= FocusDead;
        this.target = null;
    }

    void FocusDead()
    {
        if(target != null)
        {
            this.UnfocusTarget(target);
        }
    }

    
    void BlockByGoat(GameObject goat)
    {
        goatsBlocking.Add(goat);
    }

    void UnblockByGoat(GameObject goat)
    {
        var instanceId = goat.GetInstanceID();
        var enemyToRemove = goatsBlocking.First(e => e.GetInstanceID() == instanceId);
        goatsBlocking.Remove(enemyToRemove);
    }

    void Attack()
    {
        if(attackOnCooldown)
        {
            return;
        }

        target.GetComponent<Health>().Damage(domage);
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        attackOnCooldown = true;
        yield return new WaitForSeconds(attackCooldownInSeconds);
        attackOnCooldown = false;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    List<GameObject> FilterGoatsInRange()
    {
        return goatsBlocking.Where(e => e != null).ToList();
    }
}
