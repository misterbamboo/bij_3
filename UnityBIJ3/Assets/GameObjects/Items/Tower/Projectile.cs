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
    
    GameObject holder;


    bool isReleased = false;

    public void Init(GameObject target, GameObject holder, Tower parent)
    {
        this.target = target;
        this.holder = holder;
        parent.Throw += Release;
    }

    void Update()
    {
        if(target == null)
        {
            return;
        }

        if(!isReleased)
        {
            transform.position = holder.transform.position;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Health>().Damage(Damage);
            Destroy(gameObject);
        }
    }

    public void Release()
    {
        isReleased = true;
    }
}
