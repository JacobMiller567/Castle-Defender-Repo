using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    [Header("Level 1")]
    public Transform[] Waypoints;
    [Header("Level 2")]
    public Transform[] TopPath;
    public Transform[] MiddlePath;
    public Transform[] BottomPath;
    [Header("Level 3")]
    public Transform[] LeftPath;
    public Transform[] RightPath;

    private void Awake()
    {
        main = this;
    }
}
