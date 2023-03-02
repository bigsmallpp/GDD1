using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinFunction : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject WinText;
    public GameObject LoseText;
    
    [SerializeField] private AudioSource win_Sound;
    [SerializeField] private AudioSource lose_Sound;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setWinState(SceneLoader.WinningState state)
    {
        switch (state)
        {
            case SceneLoader.WinningState.won:
                BackgroundMusic.Instance.stopMusic();
                WinText.SetActive(true);
                win_Sound.Play();
                break;
            case SceneLoader.WinningState.lost:
                BackgroundMusic.Instance.stopMusic();
                LoseText.SetActive(true);
                lose_Sound.Play();
                break;
        }
    }
}
