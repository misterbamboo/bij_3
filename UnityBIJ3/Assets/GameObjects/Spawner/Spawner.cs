using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    int circleRadius = 200;

    [SerializeField]
    float changePositionInSeconds = 1;

    List<Vector3> allPositionInCircleArroundBarn = new List<Vector3>();

    bool barnFound = false;

    private void Update()
    {
        if (!barnFound)
        {
            var barn = GameObject.FindGameObjectWithTag("Barn");
            if (barn != null)
            {
                var barnPosition = barn.transform.position;
                CalculateCirclePositions(barnPosition, circleRadius, circleRadius / 2);
                StartCoroutine(MoveSpawner(barnPosition));
                barnFound = true;
            }
        }
    }

    public void StartSpawn(GameObject prefabs, int count)
    {
        StartCoroutine(SpawnAll(prefabs, count));
    }

    IEnumerator SpawnAll(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(SideEffectManager.Instance.GetSpawnInSecs());
            var instance = Instantiate(prefab, transform.position, transform.rotation);

            var goat = instance.GetComponent<Goat>();
            if (goat != null)
            {
                goat.SetSpeed(SideEffectManager.Instance.GetSheepSpeed());
            }
        }
    }

    void CalculateCirclePositions(Vector3 center, float radius, int count)
    {
        float angle = 360f / count;

        for (int i = 0; i < count; i++)
        {
            Vector3 point = Vector3.zero;
            point.x = center.x + radius * Mathf.Sin(angle * i * Mathf.Deg2Rad);
            point.z = center.z + radius * Mathf.Cos(angle * i * Mathf.Deg2Rad);
            allPositionInCircleArroundBarn.Add(point);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var position in allPositionInCircleArroundBarn)
        {
            Gizmos.DrawSphere(position, 1);
        }
    }

    IEnumerator MoveSpawner(Vector3 barnPosition)
    {
        while (true)
        {
            yield return new WaitForSeconds(changePositionInSeconds);
            transform.position = KeepDefautltHeight(allPositionInCircleArroundBarn[Random.Range(0, allPositionInCircleArroundBarn.Count)]);
            transform.LookAt(KeepDefautltHeight(barnPosition));
        }
    }

    Vector3 KeepDefautltHeight(Vector3 position)
    {
        return new Vector3(position.x, transform.position.y, position.z);
    }
}
