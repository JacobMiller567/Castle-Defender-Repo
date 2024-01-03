using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int PlayerMoney;
    [SerializeField] TextMeshProUGUI moneyText;
    public bool NormalMode;
    public bool HardMode;
  
    void Update()
    {
        moneyText.text = "$" + PlayerMoney.ToString();   
    }
}
