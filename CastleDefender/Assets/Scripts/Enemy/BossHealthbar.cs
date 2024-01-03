using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthbar : MonoBehaviour
{
    public Slider slider;
    public EnemyVitals enemy;

    public void DisplayMaxHealth(int hp)
    {
      slider.maxValue = hp;
      slider.value = hp;
    }

    public void DisplayHealth(int hp)
    {
      slider.value = hp;
    }

}
