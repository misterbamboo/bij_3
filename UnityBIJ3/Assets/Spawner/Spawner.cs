using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public void StartSpawn(IEnumerable<GameObject> prefabs)
    {
        StartCoroutine(SpawnAll(prefabs));
    }

    IEnumerator SpawnAll(IEnumerable<GameObject> prefabs)
    {
        foreach (GameObject prefab in prefabs)
        {
            yield return new WaitForSeconds(3);
            Instantiate(prefab, transform.position, transform.rotation);
        }        
    }
}
