using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] public GameObject menuInventory;

    // Start is called before the first frame update
    void Start()
    {
        menuInventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            menuInventory.SetActive(!menuInventory.activeSelf);
        }
    }
}
