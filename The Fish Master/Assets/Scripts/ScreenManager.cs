using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager instance;

    private GameObject _currentSceneGO;

    public GameObject endScreenGO;
    public GameObject gameScreenGO;
    public GameObject returnScreenGO;
    public GameObject mainScreenGO;

    public Button lengthButton;
    public Button strengthButton;
    public Button offlineEarningButton;

    public Text gameScreenMoney;
    public Text lengthCostText;
    public Text lengthValueText;
    public Text strengthCostText;
    public Text strengthValueText;
    public Text offlineEarningCostText;
    public Text offlineEarningValueText;
    public Text endScreenMoney;
    public Text returnScreenMoney;

    private int _gameCount;

    void Awake()
    {
        if (ScreenManager.instance)
            Destroy(base.gameObject);
        else
            ScreenManager.instance = this;

        _currentSceneGO = mainScreenGO;
    }


    void Start()
    {
        CheckIdles();
        UpdateTexts();
    }

    public void ChangeScreen(Screens screen)
    {
        _currentSceneGO.SetActive(false);

        switch(screen)
        {
            case Screens.MAIN:
                _currentSceneGO = mainScreenGO;
                UpdateTexts();
                CheckIdles();
                break;

            case Screens.GAME:
                _currentSceneGO = gameScreenGO;
                _gameCount++;
                break;

            case Screens.END:
                _currentSceneGO = endScreenGO;
                SetEndScreenMoney();
                break;

            case Screens.RETURN:
                _currentSceneGO = returnScreenGO;
                SetReturnScreenMoney();
                break;
        }

        _currentSceneGO.SetActive(true);
    }

    public void SetEndScreenMoney()
    {
        endScreenMoney.text = "$" + IdleManager.instance.totalGain;
    }

    public void SetReturnScreenMoney()
    {
        returnScreenMoney.text = "$" + IdleManager.instance.totalGain + " gained while not playing the game.";
    }

    public void UpdateTexts()
    {
        gameScreenMoney.text = "$" + IdleManager.instance.wallet;
        lengthCostText.text = "$" + IdleManager.instance.lengthCost;
        lengthValueText.text = -IdleManager.instance.length + "m";
        strengthCostText.text = "$" + IdleManager.instance.strengthCost;
        strengthValueText.text = IdleManager.instance.strength + " fishes";
        offlineEarningCostText.text = "$" + IdleManager.instance.offlineEarningCost;
        offlineEarningValueText.text = "$" + IdleManager.instance.offlineEarning + " /min";
    } 

    public void CheckIdles()
    {
        int lenghtCost = IdleManager.instance.lengthCost;
        int strengthCost = IdleManager.instance.strengthCost;
        int offlineEarningCost = IdleManager.instance.offlineEarningCost;
        int wallet = IdleManager.instance.wallet;

        if (wallet < lenghtCost)
            lengthButton.interactable = false;
        else
            lengthButton.interactable = true;

        if (wallet < strengthCost)
            strengthButton.interactable = false;
        else
            strengthButton.interactable = true;

        if (wallet < offlineEarningCost)
            offlineEarningButton.interactable = false;
        else
            offlineEarningButton.interactable = true;
    }
}
