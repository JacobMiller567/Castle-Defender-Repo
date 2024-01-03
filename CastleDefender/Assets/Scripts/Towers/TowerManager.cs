using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public BoxCollider2D boxCollider;


    public int TowerLevel;
    public bool MaxedLevel;
    public int TowerCost;
    public int PreviousUpgradeCost;

    public float TowerRange;
    public float TowerSpeed;
    
    public bool isWisp;
    public bool isSlowTower;
    public bool isFlameTower;
    public bool isCatapultTower;
    public bool isLightningTower;
    public bool isSpike;

    public bool towerIsPurchased = false;


    public float BlessedRangeMultiplier;
    public float BlessedSpeedMultiplier;


    public BoxCollider2D GetCollider()
    {
        return boxCollider;
    }

    public void ChangeTurretTarget()
    {
        TowerDetection tower = GetComponentInChildren<TowerDetection>(true);
        if (tower != null)
        {
            tower.ChangeTarget();
        }      
    }

    public void UpdateTowerStats(float towerRange, float towerSpeed)
    {
        TowerRange = towerRange;
        TowerSpeed = towerSpeed;
    }


}
