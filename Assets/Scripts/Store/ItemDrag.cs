using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler
{
    private Vector3 initial_pos = new Vector3();
    private Vector3 grab_offset_to_center = new Vector3();
    private Camera UIcamera = null;
    private Canvas canvas = null;

    private Transform previousParent = null;

    [SerializeField] private PlayerController _player = null;

    [SerializeField] private bool _isInInventory = true;

    private float _storePosX = 895.0f;
    private float _storePosY = 70.0f;
    private float _storeHeight = 115.0f;
    private float _storeWidth = 95.0f;

    private float _inventoryPosX = 1090.0f;
    private float _inventoryPosY = 85.0f;
    private float _inventoryHeight = 100.0f;
    private float _inventoryWidth = 110.0f;
    
    private void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
        if (_isInInventory)
        {
            initial_pos = gameObject.transform.position;
        }
        else
        {
            initial_pos = gameObject.transform.parent.transform.position;
        }

        previousParent = transform.parent;
        canvas = transform.root.GetComponentInChildren<Canvas>();
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            UIcamera,
            out pos
        );

        transform.position = canvas.transform.TransformPoint(pos);// - grab_offset_to_center;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetUICamera();
        transform.parent = canvas.transform;
        grab_offset_to_center = UIcamera.ScreenToWorldPoint(Input.mousePosition) - initial_pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("We stopped dragging");

        if (_isInInventory && CheckInChest())
        {
            Debug.Log("Item dropped in Chest Area");
            Item item = GetComponent<InventoryInteraction>().GetItem();
            _player.GetPlayerInventory().RemoveItem(item);
            _player.GetChest().AddItem(gameObject);
            
            // Parent is updated in function above
            previousParent = gameObject.transform.parent;
            _isInInventory = false;
        }
        else if (!_isInInventory && CheckInInventory())
        {
            Debug.Log("Item dropped in Inventory Area");
            _player.GetChest().RemoveItem(gameObject);
            _player.GetPlayerInventory().TransferItem(gameObject);
            
            // Parent is updated in function above
            previousParent = gameObject.transform.parent;
            _isInInventory = true;
        }
        else
        {
            transform.parent = previousParent;
            gameObject.transform.position = initial_pos;
        }
    }

    private void SetUICamera()
    {
        Camera[] cams = Camera.allCameras;
        foreach (Camera c in cams)
        {
            if (c.tag == "UICam")
            {
                UIcamera = c;
            }
        }
    }

    private bool CheckInChest()
    {
        Vector3 pos = gameObject.transform.position;

        if (pos.x >= _storePosX && pos.x <= (_storePosX + _storeWidth) &&
            pos.y <= _storePosY && pos.y >= (_storePosY - _storeHeight))
        {
            return true;
        }

        return false;
    }
    
    private bool CheckInInventory()
    {
        Vector3 pos = gameObject.transform.position;

        if (pos.x >= _inventoryPosX && pos.x <= (_inventoryPosX + _inventoryWidth) &&
            pos.y <= _inventoryPosY && pos.y >= (_inventoryPosY - _inventoryHeight))
        {
            return true;
        }

        return false;
    }

    public void SetPreviousParent(Transform parent)
    {
        previousParent = parent;
    }

    public void SetInInventory(bool value)
    {
        _isInInventory = value;
    }
}
