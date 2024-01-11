using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

  public void PlayGame()
  {
    SceneManager.LoadScene("Game Level 1");
  }

  public void PlayInfinite()
  {
    SceneManager.LoadScene("Game Level 1");
  }

  public void LoadLevelTwo()
  {
    SceneManager.LoadScene("Game Level 2");
  }
  public void LoadLevelThree()
  {
    SceneManager.LoadScene("Game Level 3");
  }


  public void QuitGame()
  {
    Application.Quit();
  }
  public void Win()
  {
    SceneManager.LoadScene("WinGame");
  }
  public void Lose()
  {
    SceneManager.LoadScene("LoseGame");
  }
  public void LoadMenu()
  {
    SceneManager.LoadScene("MainMenu");
  }
  
}