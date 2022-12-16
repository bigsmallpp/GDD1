using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startscreen : MonoBehaviour
{
  public void LoadGame()
  {
    SceneManager.LoadScene("SampleScene");
  }

  public void ExitGame()
  {
    Application.Quit();
  }
}
