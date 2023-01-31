using System;
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

    [SerializeField]
    int moneyValue = 50;

    [SerializeField]
    Animator animator;

    [SerializeField]
    ParticleSystem bloodParticle;

    [SerializeField]
    ParticleSystem grassParticle;

    GameObject target = null;

    bool attackOnCooldown = false;

    List<GameObject> goatsBlocking = new List<GameObject>();
    private Map map;

    void Start()
    {
        var detectionZones = GetComponentsInChildren<DetectionZone>();

        detectionZones.First(d => d.name == "DetectionZone").EnterRange += FocusTarget;
        detectionZones.First(d => d.name == "DetectionZone").ExitRange += UnfocusTarget;

        detectionZones.First(d => d.name == "GoatDetectionZone").EnterRange += BlockByGoat;
        detectionZones.First(d => d.name == "GoatDetectionZone").ExitRange += UnblockByGoat;

        var health = gameObject.GetComponentInChildren<Health>();
        health.NoMoreHealth += Die;
        health.HealthUpdate += BloodParticle;

        map = MapGenerator.Instance.GetMap();
    }

    private void ForceStayInMapBoundries()
    {
        var pos = transform.position;
        bool positionChanged = false;
        if (pos.x < 0)
        {
            pos.x = 0;
            positionChanged = true;
        }
        if (pos.x >= MapDrawer.Instance.MapDrawWidth)
        {
            pos.x = MapDrawer.Instance.MapDrawWidth - MapDrawer.Instance.HexWidth * 2;
            positionChanged = true;
        }
        if (pos.z < 0)
        {
            pos.z = 0;
            positionChanged = true;
        }
        if (pos.z >= MapDrawer.Instance.MapDrawHeight)
        {
            pos.z = MapDrawer.Instance.MapDrawHeight - MapDrawer.Instance.HexHeight * 2;
            positionChanged = true;
        }

        if (positionChanged)
        {
            var util = PlacingItemManager.Instance.PlacingItemUtils;
            var snapPosition = util.FindClosestSnapPosition(pos);
            transform.position = new Vector3(snapPosition.Position.x, transform.position.y, snapPosition.Position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ForceStayInMapBoundries();
        CleanDestoyedBlocking();

        if (target != null)
        {
            animator.SetBool("walking", false);
            grassParticle.Stop();
            Attack();
        }
        else if (goatsBlocking.Count == 0)
        {
            if (animator.GetBool("walking") == false)
            {
                animator.SetBool("walking", true);
                grassParticle.Play();
            }
            MoveForward();
        }
        else
        {
            grassParticle.Stop();
            animator.SetBool("walking", false);
        }
    }

    private void CleanDestoyedBlocking()
    {
        List<GameObject> toRemove = new List<GameObject>();
        foreach (var goatBlocking in goatsBlocking)
        {
            if (goatBlocking == null)
            {
                toRemove.Add(goatBlocking);
            }
        }

        foreach (var remove in toRemove)
        {
            goatsBlocking.Remove(remove);
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
        if (this.target == null)
        {
            return;
        }

        this.target.GetComponent<Health>().NoMoreHealth -= FocusDead;
        this.target = null;
    }

    void FocusDead()
    {
        if (target != null && target.gameObject.tag != "Barn")
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
        if (attackOnCooldown)
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
        bloodParticle.Play();
        MoneySys.AddMoneyToBank(moneyValue);
        Destroy(gameObject);
    }

    void BloodParticle(float health)
    {
        bloodParticle.Play();
    }

    List<GameObject> FilterGoatsInRange()
    {
        return goatsBlocking.Where(e => e != null).ToList();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
