using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float speed = 10f;

    [SerializeField]
    float Domage = 1f;

    GameObject target = null;

    public GameObject towerParent;

    void Update()
    {
        if(target != null)   
        {
            MoveTowardsTarget();
        }
    }

    public void Init(GameObject tower, GameObject target)
    {
        towerParent = tower;
        this.target = target;
    }

    public void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target)
        {
            target.GetComponent<Health>().Domage(Domage);
            Destroy(gameObject);
        }
    }
}
