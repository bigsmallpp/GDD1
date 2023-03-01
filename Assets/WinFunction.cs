using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinFunction : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject WinText;
    public GameObject LoseText;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setWinState(SceneLoader.WinningState state)
    {
        switch (state)
        {
            case SceneLoader.WinningState.won:
                WinText.SetActive(true);
                break;
            case SceneLoader.WinningState.lost:
                LoseText.SetActive(true);
                break;
        }
    }
}
