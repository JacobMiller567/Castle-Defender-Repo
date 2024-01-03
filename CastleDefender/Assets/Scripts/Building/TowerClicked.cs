using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerClicked : MonoBehaviour
{
    [SerializeField] private UpgradeTower upgrade;
    [SerializeField] private TowerManager manage;
    [SerializeField] private GameObject upgradeCanvas;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject radiusDisplay;
    public bool canUpgrade = false;

    public BoxCollider2D towerCollider;
    public GameObject sellButton;
    public bool towerClicked = false;


    void Update() 
    {
        if(Input.GetMouseButtonDown(1) && manage.towerIsPurchased == true) 
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapPoint(mousePosition);

            if (hitCollider == towerCollider)
            {
                upgradeCanvas.SetActive(false);
                towerClicked = !towerClicked;
                sellButton.SetActive(towerClicked);
            }
        }
    }

    void OnMouseDown()
    {
        if (manage.MaxedLevel == true && upgradeButton.activeSelf == true)
        {
            upgradeButton.SetActive(false); 
        }
        if (canUpgrade && manage.towerIsPurchased == true)
        {
            sellButton.SetActive(false);         
            upgradeCanvas.SetActive(true);
            upgrade.DisplayUpgradeStats();
        }
        if (!canUpgrade && manage.towerIsPurchased == true)
        {
            upgradeCanvas.SetActive(false);
        }
        canUpgrade = !canUpgrade;
    }

    void OnMouseOver() 
    {
        radiusDisplay.SetActive(true);
        if (manage.MaxedLevel == true && upgradeButton.activeSelf == true)
        {
            upgradeButton.SetActive(false); 
        }
    }

    void OnMouseExit()
    {
        radiusDisplay.SetActive(false);
    }

}
