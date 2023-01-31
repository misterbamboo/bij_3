using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField]
    GameObject fencePrefab;

    void Start()
    {
        var health = GetComponent<Health>();
        health.NoMoreHealth += DestroyFence;
    }

    void DestroyFence()
    {
        GetComponent<BoxCollider>().enabled = false;
        Destroy(fencePrefab);
    }
}
