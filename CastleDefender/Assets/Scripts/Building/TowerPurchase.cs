using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPurchase : MonoBehaviour
{
    public BuildManager build;
    public PlayerStats stats;
   
   
    public void BuySpikes() 
    {
        if (stats.PlayerMoney >= 5 && build.currentSpikeCount <= build.maxSpikeCount)
        {
            stats.PlayerMoney -= 5;
            build.towerPurchased = true;
            build.SpawnBuilding(0);
        }

    }

    public void BuyArcherTower() 
    {
        if (stats.PlayerMoney >= 15)
        {
            stats.PlayerMoney -= 15;
            build.towerPurchased = true;
            build.SpawnBuilding(1);
        }

    }

    public void BuySlingshotTower() 
    {
        if (stats.PlayerMoney >= 35)
        {
            stats.PlayerMoney -= 35;
            build.towerPurchased = true;
            build.SpawnBuilding(2);
        }

    }

    public void BuyLightningTower() 
    {
        if (stats.PlayerMoney >= 65)
        {
            stats.PlayerMoney -= 65;
            build.towerPurchased = true;
            build.SpawnBuilding(3);
        }

    }

    public void BuyCatapultTower() 
    {
        if (stats.PlayerMoney >= 110)
        {
            stats.PlayerMoney -= 110;
            build.towerPurchased = true;
            build.SpawnBuilding(4);
        }

    }

    public void BuySlowmoTower() 
    {
        if (stats.PlayerMoney >= 150)
        {
            stats.PlayerMoney -= 150;
            build.towerPurchased = true;
            build.SpawnBuilding(5);
        }
    }

    public void BuyFlameTower()
    {
        if (stats.PlayerMoney >= 120)
        {
            stats.PlayerMoney -= 120;
            build.towerPurchased = true;
            build.SpawnBuilding(6);
        }
    }

    public void BuyCrossbowTower() 
    {
        if (stats.PlayerMoney >= 100)
        {
            stats.PlayerMoney -= 100;
            build.towerPurchased = true;
            build.SpawnBuilding(7);
        }
    }
    public void BuyWisp() 
    {
        if (stats.PlayerMoney >= 350 && build.boughtWisp == false)
        {
            stats.PlayerMoney -= 350;
            build.towerPurchased = true;
            build.SpawnBuilding(8);
            build.boughtWisp = true;
        }
    }
}
