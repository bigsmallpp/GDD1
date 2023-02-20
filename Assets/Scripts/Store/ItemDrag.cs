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

    private PlayerController _player = null;

    [SerializeField] private bool _isInInventory = true;

    private float _storePosX = 895.0f;
    private float _storePosY = 70.0f;
    private float _storeHeight = 115.0f;
    private float _storeWidth = 95.0f;

    private float _inventoryPosX = 1090.0f;
    private float _inventoryPosY = 85.0f;
    private float _inventoryHeight = 80.0f;
    private float _inventoryWidth = 110.0f;

    public void SetPlayer(PlayerController player)
    {
        _player = player;
    }
    private void Start()
    {
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
            // TODO Transfer Item To Chest
            // TODO Set previousParent transform to Inventory
        }
        else if (!_isInInventory && CheckInInventory())
        {
            // TODO Transfer Item To Inventory
            // TODO Set previousParent transform to Inventory
        }
        else
        {
            transform.parent = previousParent;
            gameObject.transform.position = initial_pos;
            _isInInventory = true;
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
}
