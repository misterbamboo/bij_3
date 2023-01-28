using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float Damage = 25.0f;

    [SerializeField]
    float speed = 10.0f;

    GameObject target;

    public void Init(GameObject target)
    {
        this.target = target;
    }

    void Update()
    {
        if(target == null)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Health>().Damage(Damage);
        }
    }
}
