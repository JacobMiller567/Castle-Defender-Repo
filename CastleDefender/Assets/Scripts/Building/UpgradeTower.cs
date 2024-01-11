using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class UpgradeTower : MonoBehaviour
{
    public GameObject[] towers;
    public GameObject[] bullets; 
    public TowerManager manager;
    public PlayerStats playerInfo;
    public BuildManager build;

    [SerializeField] private int upgradeCost;
    [SerializeField] AudioSource upgradeSound;

    /// Towers ///
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI rangeText;

    /// Lightning Tower ///
    [SerializeField] TextMeshProUGUI bounceText;

    /// Slomo Tower ///
    [SerializeField] TextMeshProUGUI freezeText;
    [SerializeField] TextMeshProUGUI freezeTimeText;

    /// Flame Tower ///
    [SerializeField] TextMeshProUGUI burnTimeText;

    /// Catapult Tower ///
    [SerializeField] TextMeshProUGUI splashRadiusText;
    [SerializeField] TextMeshProUGUI splashDamageText;

    /// Wisp Tower ///
    [SerializeField] TextMeshProUGUI blessingRadiusText;
    [SerializeField] TextMeshProUGUI blessingSpeedText;


    private void Start()
    {
        playerInfo = FindObjectOfType<PlayerStats>();
        build = FindObjectOfType<BuildManager>();
    }

    public void DisplayUpgradeStats() // Displays towers current stats and upgraded stats
    {
        if (manager.isSlowTower)
        {
            if (manager.TowerLevel == 0)
            {
                costText.text = "Cost: $"+upgradeCost;
                freezeText.text = "Freeze: (" + towers[manager.TowerLevel].GetComponent<TowerSlowMotion>().slowAmount + ")" + " -" + "0.1";
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")" + " +" + (Mathf.Round(((towers[manager.TowerLevel + 1].GetComponent<TowerSlowMotion>().reloadSpeed)- manager.TowerSpeed)*100f)/100f) ;
                rangeText.text = "Range: "+ "(" +manager.TowerRange +")" + " +" + (Mathf.Round((( towers[manager.TowerLevel + 1].GetComponent<TowerSlowMotion>().freezeRadius) - manager.TowerRange)*100f)/100f);
                freezeTimeText.text = "Time: "+ "(" +towers[manager.TowerLevel].GetComponent<TowerSlowMotion>().freezeTime +")" + " +" + ((towers[manager.TowerLevel + 1].GetComponent<TowerSlowMotion>().freezeTime) - towers[manager.TowerLevel].GetComponent<TowerSlowMotion>().freezeTime);
            }
            if (manager.TowerLevel == 1)
            {
                costText.text = "Cost: $"+upgradeCost;
                freezeText.text = "Freeze: (" + towers[manager.TowerLevel].GetComponent<TowerSlowMotion>().slowAmount + ")" + " -" + "0.1";
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")" + " +" + (Mathf.Round(((towers[manager.TowerLevel + 1].GetComponent<TowerSlowMotion>().reloadSpeed) - manager.TowerSpeed)*100f)/100f);
                rangeText.text = "Range: "+ "(" +manager.TowerRange +")" + " +" + (Mathf.Round((( towers[manager.TowerLevel + 1].GetComponent<TowerSlowMotion>().freezeRadius) - manager.TowerRange)*100f)/100f);
                freezeTimeText.text = "Time: "+ "(" +towers[manager.TowerLevel].GetComponent<TowerSlowMotion>().freezeTime +")" + " +" + ((towers[manager.TowerLevel + 1].GetComponent<TowerSlowMotion>().freezeTime) - towers[manager.TowerLevel].GetComponent<TowerSlowMotion>().freezeTime); //Mathf.Round (f * 100.0f) * 0.01f;
            }
            if (manager.TowerLevel == 2)
            {
                costText.text = "Cost: Max Level";
                freezeText.text = "Freeze: (" + towers[manager.TowerLevel].GetComponent<TowerSlowMotion>().slowAmount + ")";
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")";
                rangeText.text = "Range: (" +manager.TowerRange +"m)";
                freezeTimeText.text = "Time: "+ "(" +towers[manager.TowerLevel].GetComponent<TowerSlowMotion>().freezeTime +")";
            }
        }

        else if (manager.isFlameTower)
        {
            if (manager.TowerLevel == 0)
            {
                costText.text = "Cost: $"+upgradeCost;
                int currentDamage = towers[manager.TowerLevel].GetComponent<TowerFlame>().burnDamage;
                damageText.text = "Damage: (" + towers[manager.TowerLevel].GetComponent<TowerFlame>().burnDamage + ")" + " +" + (( towers[manager.TowerLevel + 1].GetComponent<TowerFlame>().burnDamage) - currentDamage);
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")" + " +" + (Mathf.Round(((towers[manager.TowerLevel + 1].GetComponent<TowerFlame>().reloadSpeed) - manager.TowerSpeed)*100f)/100f);
                rangeText.text = "Range: "+ "(" +manager.TowerRange +")" + " +" + (Mathf.Round((( towers[manager.TowerLevel + 1].GetComponent<TowerFlame>().flameRadius) - manager.TowerRange)*100f)/100f);
                burnTimeText.text = "Time: "+ "(" +towers[manager.TowerLevel].GetComponent<TowerFlame>().burnTime +")" + " +" + ((towers[manager.TowerLevel + 1].GetComponent<TowerFlame>().burnTime) - towers[manager.TowerLevel].GetComponent<TowerFlame>().burnTime);
            }
            if (manager.TowerLevel == 1)
            {
                costText.text = "Cost: $"+upgradeCost;
                int currentDamage = towers[manager.TowerLevel].GetComponent<TowerFlame>().burnDamage;
                damageText.text = "Damage: (" + towers[manager.TowerLevel].GetComponent<TowerFlame>().burnDamage + ")" + " +" + (( towers[manager.TowerLevel + 1].GetComponent<TowerFlame>().burnDamage) - currentDamage);
                speedText.text = "Speed: "+ "(" +(Mathf.Round(manager.TowerSpeed*100f)/100f) +")" + " +" + (Mathf.Round(((towers[manager.TowerLevel + 1].GetComponent<TowerFlame>().reloadSpeed) - manager.TowerSpeed)*100f)/100f);//((towers[manager.TowerLevel + 1].GetComponent<TowerFlame>().reloadSpeed) - manager.TowerSpeed);
                rangeText.text = "Range: "+ "(" +(Mathf.Round(manager.TowerRange*100f)/100f) +")" + " +" + (Mathf.Round(((towers[manager.TowerLevel + 1].GetComponent<TowerFlame>().flameRadius) - manager.TowerRange )*100f)/100f);
                burnTimeText.text = "Time: (" + towers[manager.TowerLevel].GetComponent<TowerFlame>().burnTime +")" + " +" + ((towers[manager.TowerLevel + 1].GetComponent<TowerFlame>().burnTime) - towers[manager.TowerLevel].GetComponent<TowerFlame>().burnTime);
            }
            if (manager.TowerLevel == 2)
            {
                costText.text = "Cost: Max Level";
                int currentDamage = towers[manager.TowerLevel].GetComponent<TowerFlame>().burnDamage;
                damageText.text = "Damage: (" + towers[manager.TowerLevel].GetComponent<TowerFlame>().burnDamage + ")";
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")";
                rangeText.text = "Range: "+ "(" +manager.TowerRange +"m)";
                burnTimeText.text = "Time: (" + towers[manager.TowerLevel].GetComponent<TowerFlame>().burnTime +")";
            }
        }
        else if (manager.isCatapultTower)
        {
            if (manager.TowerLevel == 0)
            {
                costText.text = "Cost: $"+upgradeCost;
                int currentDamage = bullets[manager.TowerLevel].GetComponent<RockExplosion>().damage;
                damageText.text = "Damage: (" + bullets[manager.TowerLevel].GetComponent<RockExplosion>().damage + ")" + " +" + ((bullets[manager.TowerLevel + 1].GetComponent<RockExplosion>().damage) - currentDamage);
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")" + " -" + (Mathf.Round((manager.TowerSpeed -(towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().reloadSpeed))*100f)/100f);// TEST
                rangeText.text = "Range : "+ "(" +manager.TowerRange +")" + " +" + (Mathf.Round((( towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().detectionRange) - manager.TowerRange)*100f)/100f); //TEST
                splashRadiusText.text = "Splash: (" + bullets[manager.TowerLevel].GetComponent<RockExplosion>().explosionRange +")" + " +" + ((bullets[manager.TowerLevel + 1].GetComponent<RockExplosion>().explosionRange) - bullets[manager.TowerLevel].GetComponent<RockExplosion>().explosionRange);
            }
            if (manager.TowerLevel == 1)
            {
                costText.text = "Cost: $"+upgradeCost;
                int currentDamage = bullets[manager.TowerLevel].GetComponent<RockExplosion>().damage;
                damageText.text = "Damage: (" + bullets[manager.TowerLevel].GetComponent<RockExplosion>().damage + ")" + " +" + ((bullets[manager.TowerLevel + 1].GetComponent<RockExplosion>().damage) - currentDamage);
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")" + " -" + (Mathf.Round((manager.TowerSpeed - (towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().reloadSpeed))*100f)/100f);
                rangeText.text = "Range : "+ "(" +manager.TowerRange +")" + " +" + (Mathf.Round((( towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().detectionRange) - manager.TowerRange)*100f)/100f);
                splashRadiusText.text = "Splash: (" + bullets[manager.TowerLevel].GetComponent<RockExplosion>().explosionRange +")" + " +" + ((bullets[manager.TowerLevel + 1].GetComponent<RockExplosion>().explosionRange) - bullets[manager.TowerLevel].GetComponent<RockExplosion>().explosionRange);
            }
            if (manager.TowerLevel == 2)
            {
                costText.text = "Cost: Max Level";
                int currentDamage = bullets[manager.TowerLevel].GetComponent<RockExplosion>().damage;
                damageText.text = "Damage: (" + bullets[manager.TowerLevel].GetComponent<RockExplosion>().damage + ")";
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")";
                rangeText.text = "Range: "+ "(" +manager.TowerRange +"m)";
                splashRadiusText.text = "Splash: (" + bullets[manager.TowerLevel].GetComponent<RockExplosion>().explosionRange +"m)";
            }
        }
        else if (manager.isLightningTower)
        {
            if (manager.TowerLevel == 0)
            {
                costText.text = "Cost: $"+upgradeCost;
                int currentDamage = bullets[manager.TowerLevel].GetComponent<LightningBolt>().lightningDamage;
                damageText.text = "Damage: (" + bullets[manager.TowerLevel].GetComponent<LightningBolt>().lightningDamage + ")" + " +" + ((bullets[manager.TowerLevel + 1].GetComponent<LightningBolt>().lightningDamage) - currentDamage);
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")" + " -" + (Mathf.Round(((manager.TowerSpeed - (towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().reloadSpeed)))*100f)/100f); //TEST
                rangeText.text = "Range: "+ "(" +manager.TowerRange +")" + " +" + (Mathf.Round(((towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().detectionRange) - manager.TowerRange)*100f)/100f);
                bounceText.text = "Richochet: "+ "(" + bullets[manager.TowerLevel].GetComponent<LightningBolt>().maxHits +")" + " +" + ((bullets[manager.TowerLevel + 1].GetComponent<LightningBolt>().maxHits) -  bullets[manager.TowerLevel].GetComponent<LightningBolt>().maxHits);
            }
            if (manager.TowerLevel == 1)
            {
                costText.text = "Cost: $"+upgradeCost;
                int currentDamage = bullets[manager.TowerLevel].GetComponent<LightningBolt>().lightningDamage;
                damageText.text = "Damage: (" + bullets[manager.TowerLevel].GetComponent<LightningBolt>().lightningDamage + ")" + " +" + ((bullets[manager.TowerLevel + 1].GetComponent<LightningBolt>().lightningDamage) - currentDamage);
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")" + " -" + (Mathf.Round(((manager.TowerSpeed - (towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().reloadSpeed)))*100f)/100f); //TEST
                rangeText.text = "Range: "+ "(" +manager.TowerRange +")" + " +" + (Mathf.Round(((towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().detectionRange) - manager.TowerRange)*100f)/100f);
                bounceText.text = "Richochet: "+ "(" +bullets[manager.TowerLevel].GetComponent<LightningBolt>().maxHits +")" + " +" + ((bullets[manager.TowerLevel + 1].GetComponent<LightningBolt>().maxHits) - bullets[manager.TowerLevel].GetComponent<LightningBolt>().maxHits);
            }
            if (manager.TowerLevel == 2)
            {
                costText.text = "Cost: Max Level";
                int currentDamage = bullets[manager.TowerLevel].GetComponent<LightningBolt>().lightningDamage;
                damageText.text = "Damage: (" + bullets[manager.TowerLevel].GetComponent<LightningBolt>().lightningDamage + ")";
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")";
                rangeText.text = "Range: "+ "(" +manager.TowerRange +"m)";
                bounceText.text = "Richochet: "+ "(" +bullets[manager.TowerLevel].GetComponent<LightningBolt>().maxHits +")";
            }
        }
        else if (manager.isWisp)
        {
            if (manager.TowerLevel == 0)
            {
                costText.text = "Cost: $"+upgradeCost;
                rangeText.text = "Range: "+ "(" +manager.TowerRange +")" + " +" + (( towers[manager.TowerLevel + 1].GetComponentInChildren<WispBlessing>().blessingRadius) - manager.TowerRange);
                blessingRadiusText.text = "Radius: "+  "(10%)" + " +" + (Mathf.Round(((towers[manager.TowerLevel + 1].GetComponentInChildren<WispBlessing>().rangeBlessing) - towers[manager.TowerLevel].GetComponentInChildren<WispBlessing>().rangeBlessing)*10f)/10f);
                blessingSpeedText.text = "Speed: "+ "(10%)" + " +" + ((towers[manager.TowerLevel + 1].GetComponentInChildren<WispBlessing>().speedBlessing) - towers[manager.TowerLevel].GetComponentInChildren<WispBlessing>().speedBlessing);
            }
            if (manager.TowerLevel == 1)
            {
                costText.text = "Cost: $"+upgradeCost;
                rangeText.text = "Range: "+ "(" +manager.TowerRange +")" + " +" + (( towers[manager.TowerLevel + 1].GetComponentInChildren<WispBlessing>().blessingRadius) - manager.TowerRange);
                blessingRadiusText.text = "Radius: "+ "(20%)" + " +" + (Mathf.Round(((towers[manager.TowerLevel + 1].GetComponentInChildren<WispBlessing>().rangeBlessing) - towers[manager.TowerLevel].GetComponentInChildren<WispBlessing>().rangeBlessing)*100f)/100f); //TEST
                blessingSpeedText.text = "Speed: "+ "(20%)" + " +" + ((towers[manager.TowerLevel + 1].GetComponentInChildren<WispBlessing>().speedBlessing) - towers[manager.TowerLevel].GetComponentInChildren<WispBlessing>().speedBlessing);
            }
            if (manager.TowerLevel == 2)
            {
                costText.text = "Cost: Max Level";
                rangeText.text = "Range: "+ "(" +manager.TowerRange +"m)";
                blessingRadiusText.text = "Radius: "+ "(25%)";
                blessingSpeedText.text = "Speed: "+ "(25%)";
            }
        }
        else // Archer Tower, Crossbow Tower, Slingshot Tower
        {
            if (manager.TowerLevel == 0)
            {
                costText.text = "Cost: $"+upgradeCost;
                int currentDamage = bullets[manager.TowerLevel].GetComponent<Arrow>().damage;
                damageText.text = "Damage: (" + bullets[manager.TowerLevel].GetComponent<Arrow>().damage + ")" + " +" + ((bullets[manager.TowerLevel + 1].GetComponent<Arrow>().damage) - currentDamage);
                //speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")" + " -" + ( manager.TowerSpeed - (towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().reloadSpeed * manager.BlessedSpeedMultiplier));
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")" + " -" + (Mathf.Round((manager.TowerSpeed - (towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().reloadSpeed))*100f)/100f);
                rangeText.text = "Range: "+ "(" +manager.TowerRange +")" + " +" +  (Mathf.Round((( towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().detectionRange) - manager.TowerRange)*100f)/100f);
            }
            if (manager.TowerLevel == 1)
            {
                costText.text = "Cost: $"+upgradeCost;
                int currentDamage = bullets[manager.TowerLevel].GetComponent<Arrow>().damage;
                damageText.text = "Damage: (" + bullets[manager.TowerLevel].GetComponent<Arrow>().damage + ")" + " +" + ((bullets[manager.TowerLevel + 1].GetComponent<Arrow>().damage) - currentDamage);
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")" + " -" + (Mathf.Round((manager.TowerSpeed - (towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().reloadSpeed))*100f)/100f);
                rangeText.text = "Range: "+ "(" +manager.TowerRange +")" + " +" + (Mathf.Round((( towers[manager.TowerLevel + 1].GetComponentInChildren<TowerDetection>().detectionRange) - manager.TowerRange)*100f)/100f);
            }
            if (manager.TowerLevel == 2)
            {
                costText.text = "Cost: Max Level";
                int currentDamage = bullets[manager.TowerLevel].GetComponent<Arrow>().damage;
                damageText.text = "Damage: (" + bullets[manager.TowerLevel].GetComponent<Arrow>().damage + ")";
                speedText.text = "Speed: "+ "(" +manager.TowerSpeed +")";
                rangeText.text = "Range: "+ "(" +manager.TowerRange +"m)";
            } 
        }   

    }


    public void UpgradeTowerLevel()
    {
        if (playerInfo.PlayerMoney >= upgradeCost && manager.MaxedLevel == false)
        {
            if (manager.TowerLevel >= 2) // Prevents towers from being upgraded more than 2 times!
            {
                Debug.Log("Max Level");
                manager.MaxedLevel = true;
                return;
            }
            manager.PreviousUpgradeCost += upgradeCost;
            playerInfo.PlayerMoney -= upgradeCost;
            Destroy(towers[manager.TowerLevel]); // Destroy tower to prevent values being used in other GetChildren functions!
            upgradeSound.Play();

            manager.TowerLevel += 1;
            towers[manager.TowerLevel].SetActive(true); // Show new tower base

        
            if (playerInfo.HardMode == true) // Make hardmode cost slightly more for max upgrade
            {
                upgradeCost = Mathf.RoundToInt(upgradeCost * 3f); 
            }
            if (playerInfo.NormalMode == true)
            {
                upgradeCost = Mathf.RoundToInt(upgradeCost * 2.5f);
            }
            DisplayUpgradeStats();
        }

        else if (playerInfo.PlayerMoney < upgradeCost)
        {
            Debug.Log("Upgrade Costs: $"+upgradeCost);
        }
    }

    public void SellTower()
    {
        int towerInitialCost = Mathf.RoundToInt(manager.TowerCost * 0.65f); // Sell tower for 65% of initial cost
        int towerUpgradesCost = Mathf.RoundToInt(manager.PreviousUpgradeCost * 0.40f); // Receive 40% money spent on all upgrades
        playerInfo.PlayerMoney += (towerInitialCost + towerUpgradesCost); 
        Debug.Log("Tower Sold");
        if (gameObject.GetComponent<TowerManager>().isWisp == true) // If tower is a Wisp tower
        {
            build.boughtWisp = false;
        }
        build.placedTowersColliders.Remove(gameObject.GetComponent<TowerManager>().GetCollider()); // Remove tower from collider list
        Destroy(gameObject);
    }

    public float GiveTowerRadius() // Tower radius circle
    {
        if (manager.TowerLevel == 0)
        {
            if (manager.isFlameTower == true)
            {
                float dist = towers[0].GetComponent<TowerFlame>().GetFlameRadius();
                return dist;

            }
            if (manager.isSlowTower == true)
            {
                float dist = towers[0].GetComponent<TowerSlowMotion>().GetFreezeRadius();
                return dist;

            }
            if (manager.isWisp == true)
            {
                float dist = towers[0].GetComponent<WispBlessing>().GetBlessingRadius();
                return dist;
            }
            else
            {
                float dist = towers[0].GetComponentInChildren<TowerDetection>().GetTowerRadius();
                return dist;
            }
        }

        if (manager.TowerLevel == 1)
        {
            if (manager.isFlameTower == true)
            {
                float dist = towers[1].GetComponent<TowerFlame>().GetFlameRadius();
                return dist;
            }
            if (manager.isSlowTower == true)
            {
                float dist = towers[1].GetComponent<TowerSlowMotion>().GetFreezeRadius();
                return dist;
            }
            if (manager.isWisp == true)
            {
                float dist = towers[1].GetComponent<WispBlessing>().GetBlessingRadius();
                return dist;
            }
            else
            {
                float dist = towers[1].GetComponentInChildren<TowerDetection>().GetTowerRadius();
                return dist;
            }
        }

        if (manager.TowerLevel == 2)
        {
            if (manager.isFlameTower == true)
            {
                float dist = towers[2].GetComponent<TowerFlame>().GetFlameRadius();
                return dist;
            }
            if (manager.isSlowTower == true)
            {
                float dist = towers[2].GetComponent<TowerSlowMotion>().GetFreezeRadius();
                return dist;
            }
            if (manager.isWisp == true)
            {
                float dist = towers[2].GetComponent<WispBlessing>().GetBlessingRadius();
                return dist;
            }
            else
            {
                float dist = towers[2].GetComponentInChildren<TowerDetection>().GetTowerRadius();
                return dist;
            }
        }
        return 0f;
    }   
}
