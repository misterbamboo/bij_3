using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySys : MonoBehaviour
{
    private static MoneySys _instance;

    //Balance courante (BAA coins)...get it ? :D
    public int baaCoins;

    public float saveInterval;

    private static MoneySys instance
    {
        get
        {
            if (_instance == null)
            {
                if (GameObject.Find("MoneySys"))
                {
                    GameObject g = GameObject.Find("MoneySys");
                    if (g.GetComponent<MoneySys>())
                    {
                        _instance = g.GetComponent<MoneySys>();
                    }
                    else
                    {
                        _instance = g.AddComponent<MoneySys>();
                    }
                }
                else
                {
                    GameObject g = new GameObject();
                    g.name = "MoneySys";
                    _instance = g.AddComponent<MoneySys>();
                }
            }

            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    void Start()
    {
        gameObject.name = "MoneySys";

        _instance = this;

        AddMoneyToBank(PlayerPrefs.GetInt("MoneySave", 0));

        StartCoroutine("SaveMoneyToBank");
    }

    public IEnumerator SaveMoneyToBank()
    {
        while (true)
        {
            yield return new WaitForSeconds(saveInterval);
            PlayerPrefs.SetInt("MoneySave", instance.baaCoins);
        }
    }

    public static bool BuyItem(int cost)
    {
        if (instance.baaCoins - cost >= 0)
        {
            instance.baaCoins -= cost;
            return true;
        }
        else
        {
            return false;
        }
    }

    public static int GetMoney()
    {
        return instance.baaCoins;
    }

    public static void AddMoneyToBank(int montant)
    {
        instance.baaCoins += montant;
    }
}
