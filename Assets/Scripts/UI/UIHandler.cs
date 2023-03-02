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
    private bool itemIsBeingDragged = false;
    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            LoadItemsInToolbar();
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

    public void SetItemBeingDragged(bool val)
    {
        itemIsBeingDragged = val;
    }

    public bool CheckItemBeingDragged()
    {
        return itemIsBeingDragged;
    }

    public void UnlockTool(SelectedToolHighlighted.ToolbarIndices index)
    {
        _toolHighlight.UnlockTool(index);
    }

    public void LoadItemsInToolbar()
    {
        // TODO Set amount of Seeds for each type
        StoreDataStore store = SaveManager.Instance.LoadStoreData();

        if (!store.bucketAvailable)
        {
            UnlockTool(SelectedToolHighlighted.ToolbarIndices.Bucket);
        }

        if (!store.scissorAvailable)
        {
            UnlockTool(SelectedToolHighlighted.ToolbarIndices.Scissors);
        }
    }

    public void UpdateSeedsInToolbar(Item.ItemType type, bool is_present)
    {
        _toolHighlight.EnableSeeds(type, is_present);
    }
}
