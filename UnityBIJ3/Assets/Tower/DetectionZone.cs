using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
	public event Action<GameObject> EnemyEnterRange = delegate { };

	public event Action<GameObject> EnemyOutOfRange = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyEnterRange(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyOutOfRange(other.gameObject);
        }
    }
}
