using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class TowerStatsDisplay : MonoBehaviour
{

    [SerializeField] private UpgradeTower tower;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI damageText;
    [SerializeField] TextMeshProUGUI SpeedText;
    [SerializeField] TextMeshProUGUI RangeText;

    public void DisplayStats(int cost, int damage, float range, float speed)
    {
        costText.text = "Cost: $"+cost;
        costText.text = "Damage: "+damage;
        costText.text = "Range : "+range;
        costText.text = "Speed: "+speed;
    }
}
