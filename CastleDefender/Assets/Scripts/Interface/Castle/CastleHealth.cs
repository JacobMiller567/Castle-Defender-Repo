using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleHealth : MonoBehaviour
{
    public int castleHealth;
    [SerializeField] private GameObject bloodSplatter;
    [SerializeField] private CastleHealthbar healthbar;
    [SerializeField] private MainMenu mainMenu;

    void Start()
    {
        healthbar.DisplayMaxHealth(castleHealth);
        healthbar.DisplayHealth(castleHealth);
    }

    public void UpdateCastleHealthbar()
    {
        healthbar.DisplayMaxHealth(castleHealth);
        healthbar.DisplayHealth(castleHealth);
    }


    public void CastleBreach(int enemyHealth)
    {
        castleHealth -= enemyHealth;
        healthbar.DisplayHealth(castleHealth);
        Instantiate(bloodSplatter, transform.position, Quaternion.identity);

        if (castleHealth <= 0)
        {
            mainMenu.Lose();
        }
    }


}
