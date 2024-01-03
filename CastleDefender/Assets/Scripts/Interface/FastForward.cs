using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastForward : MonoBehaviour
{
    [SerializeField] private int fastForwardAmount = 1;
    [SerializeField] private GameObject twoTimesSpeed;
    [SerializeField] private GameObject fourTimesSpeed;

    public void ResumeGameSpeed()
    {
        Time.timeScale = fastForwardAmount;
    }

    public void ChangeGameSpeed()
    {
        if (fastForwardAmount == 1)
        {
            fastForwardAmount = 2;
            Time.timeScale = fastForwardAmount;
            twoTimesSpeed.SetActive(true);
            return;
        }
        if (fastForwardAmount == 2)
        {
            fastForwardAmount = 4;
            Time.timeScale = fastForwardAmount;
            fourTimesSpeed.SetActive(true);
            return;
        }
        if (fastForwardAmount == 4)
        {
            fastForwardAmount = 1;
            Time.timeScale = fastForwardAmount;
            twoTimesSpeed.SetActive(false);
            fourTimesSpeed.SetActive(false);
            return;
        } 
    } 
}
