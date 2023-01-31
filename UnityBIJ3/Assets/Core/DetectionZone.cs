using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    //[TagSelector]
    public List<string> TagsSelector;

    [SerializeField] List<string> TagsFilter;

    public event Action<GameObject> EnterRange = delegate { };

    public event Action<GameObject> ExitRange = delegate { };

    private void OnTriggerEnter(Collider other)
    {
        if (TagsSelector.Contains(other.gameObject.tag))
        {
            EnterRange(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (TagsSelector.Contains(other.gameObject.tag))
        {
            ExitRange(other.gameObject);
        }
    }
}
