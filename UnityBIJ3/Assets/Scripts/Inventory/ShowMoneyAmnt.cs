using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowMoneyAmnt : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI coinsDisplay;

    void Update()
    {
        // Get Amount of money from the money system
        var coinsAmt = MoneySys.GetMoney().ToString();

        coinsDisplay.text = coinsAmt;
    }
}
