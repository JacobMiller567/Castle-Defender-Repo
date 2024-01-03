using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    
    [SerializeField] AudioSource music;
    public Slider musicSlider;
    public float musicSliderValue;

    [SerializeField] Toggle sound;
    [SerializeField] Toggle gameSound;

    [SerializeField] AudioClip arrowSnd;
    [SerializeField] AudioClip slighshotSnd;
    [SerializeField] AudioClip catapultSnd;
    [SerializeField] AudioClip flameSnd;
    [SerializeField] AudioClip lightningSnd;
    [SerializeField] AudioClip slomoSnd;
    [SerializeField] AudioClip upgradeSnd;
    
    [SerializeField] AudioSource buttonSound;
    public bool playSound;
    private bool active = false;
    
    public GameObject settingsMenu;
    public FastForward fastForward;
    private SaveData data;


    void Start()
    {
        data = FindObjectOfType<SaveData>();
        musicSlider.value = data.GetComponent<SaveData>().musicVolume;
        sound.isOn = data.GetComponent<SaveData>().buttonSounds;
        gameSound.isOn = data.GetComponent<SaveData>().gameSounds;

        MuteTowerSounds();
        MuteUISound();
    }
    

    void Update()
    {
        AdjustMusicVolume();
        
        if (Input.GetKeyDown(KeyCode.Escape) && active == false) 
        {
            settingsMenu.SetActive(true);
            active = true;
            Time.timeScale = 0;

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && active == true) 
        {
            settingsMenu.SetActive(false);
            active = false;
            fastForward.ResumeGameSpeed(); // Reset game speed to fast forward value
        }
    }

    public void AdjustMusicVolume()
    {
        musicSliderValue = musicSlider.value;
        music.volume = musicSliderValue;
        data.musicVolume = music.volume; // Store volume for next scenes
    }

    public void PlayButtonSound()
    {
        buttonSound.Play();
    }

    public void MuteTowerSounds() // Mute all tower sounds
    {

        if (gameSound.isOn == false) 
        {
            MuteProjectileSounds(true);
            MuteUpgradeSounds(true);
            data.gameSounds = false;                   
        }

        else 
        {
            MuteProjectileSounds(false);
            MuteUpgradeSounds(false);
            data.gameSounds = true;
        }
    }

    public void MuteUISound() // Mute UI sounds "buttons"
    {

        if (sound.isOn == false) 
        {
            buttonSound.mute = true;
            data.buttonSounds = false;
        }

        else 
        {
            buttonSound.mute = false;
            data.buttonSounds = true;
        }
    }

    public void MuteProjectileSounds(bool muted)
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.clip == arrowSnd)
            {
                audioSource.mute = muted;
            }
            if (audioSource.clip == slighshotSnd)
            {
                audioSource.mute = muted;
            }
            if (audioSource.clip == catapultSnd)
            {
                audioSource.mute = muted;
            }
            if (audioSource.clip == lightningSnd)
            {
                audioSource.mute = muted;
            }
            if (audioSource.clip == flameSnd)
            {
                audioSource.mute = muted;
            }
            if (audioSource.clip == slomoSnd)
            {
                audioSource.mute = muted;
            }
        }
    }

    public void MuteUpgradeSounds(bool muted)
    {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.clip == upgradeSnd)
            {
                audioSource.mute = muted;
            }
        }
    }

}
