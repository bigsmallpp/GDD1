using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private static Chest _instance = null;
    public static Chest Instance => _instance;
    
    [SerializeField] private Inventory _inventory;
    [SerializeField] private List<Item> _items_in_store;

    private void Start()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OpenOrCloseChestUI()
    {
        if (SceneLoader.Instance.currentScene != SceneLoader.Scene.Outside)
        {
            return;
        }
        
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            
        }
    }
}
