using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FenceLink : MonoBehaviour
{
    void Start()
    {
        GetComponent<Health>().NoMoreHealth += DestroyFence;
    }

    void DestroyFence()
    {
        Destroy(gameObject);
    }
}

