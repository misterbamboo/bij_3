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
    private IMapDrawer mapDrawer;
    private int currentXIndex;
    private int currentZIndex;

    private Vector3 lastPoint = Vector3.zero;
    private Vector3 nextPoint = Vector3.zero;
    private float lastToNextPointT = 0;

    public List<Vector3> pathToBarn { get; private set; } = new List<Vector3>();

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
        mapDrawer = MapDrawer.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        ForceStayInMapBoundries();
        CleanDestoyedBlockingGoats();

        SearchForPathToBarn();

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
            MoveOnPathToBarn();
        }
        else
        {
            grassParticle.Stop();
            animator.SetBool("walking", false);
        }
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
        if (pos.x >= mapDrawer.MapDrawWidth)
        {
            pos.x = mapDrawer.MapDrawWidth - mapDrawer.HexWidth * 2;
            positionChanged = true;
        }
        if (pos.z < 0)
        {
            pos.z = 0;
            positionChanged = true;
        }
        if (pos.z >= mapDrawer.MapDrawHeight)
        {
            pos.z = mapDrawer.MapDrawHeight - mapDrawer.HexHeight * 2;
            positionChanged = true;
        }

        if (positionChanged)
        {
            var util = PlacingItemManager.Instance.PlacingItemUtils;
            var snapPosition = util.FindClosestSnapPosition(pos);
            currentXIndex = snapPosition.XIndex < 0 ? snapPosition.XIndex + 2 : snapPosition.XIndex;
            currentZIndex = snapPosition.ZIndex < 0 ? snapPosition.ZIndex + 2 : snapPosition.ZIndex;

            // Should respect HexGrid restriction
            if ((currentXIndex + currentZIndex) % 2 != 0)
            {
                currentZIndex++;
            }

            transform.position = new Vector3(snapPosition.Position.x, transform.position.y, snapPosition.Position.z);
        }
    }

    private void CleanDestoyedBlockingGoats()
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

    private void SearchForPathToBarn()
    {
        if (!pathToBarn.Any())
        {
            var pathToBarnInIndexes = map.FindPathToBarn(currentXIndex, currentZIndex);
            pathToBarn = pathToBarnInIndexes.Select(i => mapDrawer.GetWorldPos((int)i.x, (int)i.y)).ToList();
        }
    }

    void MoveOnPathToBarn()
    {
        if (!pathToBarn.Any()) return;

        if (nextPoint == Vector3.zero)
        {
            lastPoint = transform.position;
            var p = pathToBarn.First();
            p.y = transform.position.y;
            nextPoint = p;
            lastToNextPointT = 0;
        }

        lastToNextPointT += Time.deltaTime * speed;
        var newPos = Vector3.Lerp(lastPoint, nextPoint, lastToNextPointT);
        transform.position = newPos;

        var direction = (nextPoint - newPos);
        var targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.99f * Time.deltaTime * 5f);

        var distanceToNextPoint = direction.magnitude;
        if (distanceToNextPoint < 0.1 && pathToBarn.Count() > 1)
        {
            pathToBarn.RemoveAt(0);
            lastPoint = transform.position;
            var p = pathToBarn.First();
            p.y = transform.position.y;
            nextPoint = p;
            lastToNextPointT = 0;
        }
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
