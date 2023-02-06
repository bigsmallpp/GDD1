using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtons : MonoBehaviour
{
  [SerializeField] GameObject startMenu;
  [SerializeField] GameObject characterMenu;
  [SerializeField] GameObject errorWindow;

  public void LoadGame()
  {
    if(SceneLoader.Instance.player_variant != 0)
    {
      SceneManager.LoadScene("SampleScene");
    }
    else
    {
      Debug.Log("Please choose a character!");
      showErrorWindow();
    }
    
  }

  public void ExitGame()
  {
    Application.Quit();
  }

  public void openCharacterSelector()
  {
    errorWindow.SetActive(false);
    startMenu.SetActive(false);
    characterMenu.SetActive(true);
  }

  public void backToStartMenu()
  {
    errorWindow.SetActive(false);
    characterMenu.SetActive(false);
    startMenu.SetActive(true);
    SceneLoader.Instance.setPlayerVariant(0);
  }

  private void showErrorWindow()
  {
    errorWindow.SetActive(true);
    characterMenu.SetActive(false);
  }
}
