using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        var detectionZone = GetComponentInChildren<DetectionZone>();
        gameObject.GetComponentInChildren<DetectionZone>().EnterRange += FocusTarget;
        gameObject.GetComponentInChildren<DetectionZone>().ExitRange += UnfocusTarget;
        gameObject.GetComponentInChildren<Health>().NoMoreHealth += Die;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Attack();
        }
        else
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
    }

    void UnfocusTarget(GameObject target)
    {
        this.target = null;
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
}
