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

    

    void Start()
    {
        var barnPosition = GameObject.FindGameObjectWithTag("Barn").transform.position;
        CalculateCirclePositions(barnPosition, circleRadius, circleRadius / 2);      
        StartCoroutine(MoveSpawner(barnPosition));  
    }

    public void StartSpawn(IEnumerable<GameObject> prefabs)
    {
        StartCoroutine(SpawnAll(prefabs));
    }

    IEnumerator SpawnAll(IEnumerable<GameObject> prefabs)
    {
        foreach (GameObject prefab in prefabs)
        {
            yield return new WaitForSeconds(changePositionInSeconds);
            Instantiate(prefab, transform.position, transform.rotation);
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
            transform.position =  KeepDefautltHeight(allPositionInCircleArroundBarn[Random.Range(0, allPositionInCircleArroundBarn.Count)]);
            transform.LookAt(KeepDefautltHeight(barnPosition));
        }
    }

    Vector3 KeepDefautltHeight(Vector3 position)
    {
        return new Vector3(position.x, transform.position.y, position.z);
    }
}
