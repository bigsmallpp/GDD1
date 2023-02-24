using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    private static UIHandler _instance = null;
    public static UIHandler Instance => _instance;
    
    [SerializeField] private SelectedToolHighlighted _toolHighlight;
    [SerializeField] private UIInteract _uiInteract;
    [SerializeField] private PlayerInventory _uiInventory;
    [SerializeField] private Chest _uiChest;
    void Awake()
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
        DontDestroyOnLoad(gameObject);
    }

    public UIInteract GetUIInteract()
    {
        return _uiInteract;
    }
    
    public SelectedToolHighlighted GetSelectedTool()
    {
        return _toolHighlight;
    }
    
    public PlayerInventory GetPlayerInventory()
    {
        return _uiInventory;
    }
    
    public Chest GetChest()
    {
        return _uiChest;
    }
}
