using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public bool isDifficultyNormal;
    public bool isStoryMode;
    public int GameLevel = 1;
    public float musicVolume = 0.1f;
    public bool gameSounds = true;
    public bool buttonSounds = true;
    public bool healthBarShown = true;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
