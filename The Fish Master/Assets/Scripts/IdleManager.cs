using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleManager : MonoBehaviour
{
    [HideInInspector]
    public int length;

    [HideInInspector]
    public int strength;

    [HideInInspector]
    public int offlineEarning;

    [HideInInspector]
    public int lengthCost;

    [HideInInspector]
    public int strengthCost;

    [HideInInspector]
    public int offlineEarningCost;

    [HideInInspector]
    public int wallet;

    [HideInInspector]
    public int totalGain;

    private int[] _costs = new int[]
    {
        100, 150, 300, 500, 750, 1000, 1500, 2000, 5000, 10000, 15000, 20000, 30000, 50000, 75000, 100000, 125000, 250000, 500000
    };

    public static IdleManager instance;

    void Awake()
    {
        if (IdleManager.instance)
            UnityEngine.Object.Destroy(gameObject);
        else
            IdleManager.instance = this;

        length = -PlayerPrefs.GetInt("Length", 30);
        strength = PlayerPrefs.GetInt("Strength", 3);
        offlineEarning = PlayerPrefs.GetInt("Offline Earning", 3);

        lengthCost = _costs[-length / 10 - 3];
        strengthCost = _costs[strength - 3];
        offlineEarningCost = _costs[offlineEarning - 3];

        wallet = PlayerPrefs.GetInt("Wallet", 0);
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            DateTime now = DateTime.Now;
            PlayerPrefs.SetString("Date", now.ToString());
            MonoBehaviour.print(now.ToString());
        }

        else
        {
            string @string = PlayerPrefs.GetString("Date", string.Empty);

            if(@string != string.Empty)
            {
                DateTime d = DateTime.Parse(@string);
                totalGain = (int)((DateTime.Now - d).TotalMinutes * offlineEarning + 1.0);
                ScreenManager.instance.ChangeScreen(Screens.RETURN);
            }
        }
    }

    private void OnApplicationQuit()
    {
        OnApplicationPause(true);
    }

    public void BuyLength()
    {
        length -= 10;
        wallet -= lengthCost;
        lengthCost = _costs[-length / 10 - 3];
        PlayerPrefs.SetInt("Length", -length);
        PlayerPrefs.SetInt("Wallet", wallet);

        ScreenManager.instance.ChangeScreen(Screens.MAIN);
    }

    public void BuyStrength()
    {
        strength++;
        wallet -= strengthCost;
        strengthCost = _costs[strength - 3];
        PlayerPrefs.SetInt("Strength", strength);
        PlayerPrefs.SetInt("Wallet", wallet);

        ScreenManager.instance.ChangeScreen(Screens.MAIN);
    }

    public void BuyOfflineEarning()
    {
        offlineEarning++;
        wallet -= offlineEarningCost;
        offlineEarningCost = _costs[offlineEarning - 3];
        PlayerPrefs.SetInt("Offline Earning", offlineEarning);
        PlayerPrefs.SetInt("Wallet", wallet);

        ScreenManager.instance.ChangeScreen(Screens.MAIN);
    }

    public void CollectMoney()
    {
        wallet += totalGain;
        PlayerPrefs.SetInt("Wallet", wallet);

        ScreenManager.instance.ChangeScreen(Screens.MAIN);
    }

    public void CollectDoubleMoney()
    {
        wallet += totalGain * 2;
        PlayerPrefs.SetInt("Wallet", wallet);

        ScreenManager.instance.ChangeScreen(Screens.MAIN);
    }
}
