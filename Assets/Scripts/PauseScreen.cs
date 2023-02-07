using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
  public void BackToMenu()
  {
    //TODO: Cleanup Scene before loading Main Menu
    //SceneManager.LoadScene("StartScreen");
    SceneLoader.Instance.loadScene(0);
  }

  public void CheckControls()
  {

  }

  public void ExitGame()
  {
    Application.Quit();
  }
}
