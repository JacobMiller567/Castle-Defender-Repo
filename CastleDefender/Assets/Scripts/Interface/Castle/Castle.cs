using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Castle : MonoBehaviour
{
    public int castleLevel;
    public GameObject[] castleTurrets;
    [SerializeField] private GameObject upgradeCanvas;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] TextMeshProUGUI costText;

    public CastleHealth castleHealth;
    public PlayerStats playerInfo;
    public bool canUpgrade = false;
    private bool castleMaxed;
    public int upgradeCost = 300;


    void OnMouseDown()
    {
        if (castleMaxed == true)
        {
            upgradeCanvas.SetActive(false); 
        }
        else if (canUpgrade)
        {
            upgradeCanvas.SetActive(true);
            ShowCastleUpgradeCost();
        }
        else if (!canUpgrade)
        {
            upgradeCanvas.SetActive(false);
        }
        canUpgrade = !canUpgrade;
    }

    public void UpgradeCastle()
    {
        if (playerInfo.PlayerMoney >= upgradeCost && castleMaxed == false)
        {
            if (castleLevel >= 2)
            {
                castleMaxed = true;
                upgradeCanvas.SetActive(false); 
                return;
            }
            playerInfo.PlayerMoney -= upgradeCost;
            castleTurrets[castleLevel].SetActive(true);
            castleLevel += 1;
            if (playerInfo.HardMode == true)
            {
                upgradeCost = Mathf.RoundToInt(upgradeCost * 2.5f); 
            }
            if (playerInfo.NormalMode == true)
            {
                upgradeCost = Mathf.RoundToInt(upgradeCost * 2f);            
            }
            ShowCastleUpgradeCost();
        }
    }

    public void ShowCastleUpgradeCost()
    {
        if (castleLevel >= 2)
        {
            castleMaxed = true;
            upgradeCanvas.SetActive(false); 
            return;
        }
        costText.text = "Cost: $"+upgradeCost;
    }


}
