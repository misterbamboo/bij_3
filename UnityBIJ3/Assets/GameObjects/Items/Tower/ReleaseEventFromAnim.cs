using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseEventFromAnim : MonoBehaviour
{
    public void Release()
    {
        GetComponentInParent<Tower>().ReleaseProjectile();
    }
}
