using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [TagSelector]
    public string TagSelector = "";

	public event Action<GameObject> EnterRange = delegate { };

	public event Action<GameObject> ExitRange = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == TagSelector)
        {
            EnterRange(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == TagSelector)
        {
            ExitRange(other.gameObject);
        }
    }
}
