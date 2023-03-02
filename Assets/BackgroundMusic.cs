using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic Instance;
    [SerializeField] private AudioSource background_Music;

    [SerializeField] private AudioSource ambient_Sound;
    
    [SerializeField] private AudioSource ambient_Sound_field;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneLoader.Instance.currentScene == SceneLoader.Scene.Stable)
        {
            ambient_Sound.Pause();
        }
        else
        {
            ambient_Sound.UnPause();
        }

        if (SceneLoader.Instance.currentScene == SceneLoader.Scene.Field)
        {
            if (!ambient_Sound_field.isPlaying)
            {
                ambient_Sound_field.Play();
            }
            
        }
        else
        {
            ambient_Sound_field.Stop();
        }
    }

    public void stopMusic()
    {
        background_Music.Stop();
        ambient_Sound.Stop();
    }
}
