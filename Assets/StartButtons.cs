using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButtons : MonoBehaviour
{
  [SerializeField] GameObject startMenu;
  [SerializeField] GameObject characterMenu;
  [SerializeField] GameObject errorWindow;
  [SerializeField] GameObject continueButton;

  public void LoadGame()
  {
    if(SceneLoader.Instance.player_variant != 0)
    {
      SaveManager.Instance.ResetSaves();
      SceneManager.LoadScene("SampleScene");
    }
    else
    {
      Debug.Log("Please choose a character!");
      showErrorWindow();
    }
  }

  private void Start()
  {
    try
    {
      if (File.Exists("saves/save.json"))
      {
        Color full_alpha = continueButton.GetComponentInChildren<TextMeshProUGUI>().color;
        full_alpha.a = 255f;
        continueButton.GetComponentInChildren<TextMeshProUGUI>().color = full_alpha;
        continueButton.GetComponent<Button>().interactable = true;
      }
    }
    catch (Exception e)
    {
      Debug.Log(e);
    }
  }

  public void LoadSaveFiles()
  {
    SaveManager.Instance.LoadDataAndStartGame();
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
