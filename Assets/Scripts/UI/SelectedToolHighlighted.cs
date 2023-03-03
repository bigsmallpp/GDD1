using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectedToolHighlighted : MonoBehaviour
{
    public GameObject player;
    private Image childImage;
    private int last_tool;

    private int firstTool = 0;

    [SerializeField] private List<GameObject> _customizableSlots;
    [SerializeField] private List<Image> _customizableSlotsCrosses;
    [SerializeField] private List<Sprite> _tool_images;

    public Color colorSelected;
    public Color colorIdle;
    
    public bool wheat_seeds_enabled = false;
    public bool carrot_seeds_enabled = false;
    public bool cauliflower_seeds_enabled = false;

    public enum ToolbarIndices
    {
        Wheat_Seeds = 1,
        Cauliflower_Seeds = 3,
        Carrot_Seeds = 2,
        Scissors = 4,
        Bucket = 5
    }

    // Start is called before the first frame update
    void Start()
    {
        last_tool = SceneLoader.Instance.lastSelectedTool;
        childImage = transform.GetChild(last_tool).gameObject.GetComponent<Image>();
        Debug.Log("START HIGHLIGHT LAST TOOL CALLED");
        HighlightTool(last_tool);
    }

    public void HighlightTool(int ToolNumb)
    {
        if (last_tool > 0 && last_tool < 4)
        {
            _customizableSlotsCrosses[last_tool - 1].enabled = false;
            _customizableSlotsCrosses[last_tool - 1].color = colorIdle;
        }
        
        childImage.color = colorIdle; //Old childImage color

        childImage = transform.GetChild(ToolNumb).gameObject.GetComponent<Image>();
        childImage.color = colorSelected;
        
        if (ToolNumb > 0 && ToolNumb < 4)
        {
            _customizableSlotsCrosses[ToolNumb - 1].enabled = !GetSeedsEnbaled(ToolNumb);
            _customizableSlotsCrosses[ToolNumb - 1].color = colorSelected;
        }
        
        last_tool = ToolNumb;
        SceneLoader.Instance.lastSelectedTool = last_tool;
    }

    public void UnlockTool(ToolbarIndices index)
    {
        if ((int)index <= 1)
        {
            Debug.LogError("Illegal Index");
            return;
        }
        
        transform.GetChild((int) index).gameObject.GetComponent<Image>().sprite = _tool_images[(int) index - 2];
    }

    public bool CheckUnlocked(ToolbarIndices index)
    {
        bool ret = false;
        switch (index)
        {
            case ToolbarIndices.Bucket:
                ret = transform.GetChild((int)index).gameObject.GetComponent<Image>().sprite != null;
                break;
            case ToolbarIndices.Scissors:
                ret = transform.GetChild((int)index).gameObject.GetComponent<Image>().sprite != null;
                break;
        }
        
        Debug.Log(index + " is unlocked? " + ret);
        return ret;
    }

    public void EnableSeeds(Item.ItemType seed_type, bool enable)
    {
        switch (seed_type)
        {
            case Item.ItemType.carrot_seed:
                carrot_seeds_enabled = enable;
                break;
            
            case Item.ItemType.cauliflower_seed:
                cauliflower_seeds_enabled = enable;
                break;
            
            case Item.ItemType.wheat_seed:
                wheat_seeds_enabled = enable;
                break;
            
            default:
                throw new NotImplementedException();
        }
        
        UpdateCrosses();
    }

    public bool GetSeedsEnbaled(int ToolIndex)
    {
        switch (ToolIndex)
        {
            case 1:
                return wheat_seeds_enabled;
            
            case 2:
                return carrot_seeds_enabled;
            
            case 3:
                return cauliflower_seeds_enabled;
            
            default:
                throw new NotImplementedException();
        }
    }

    public Item.ItemType GetSelectedSeedType(int ToolNumb)
    {
        switch (ToolNumb)
        {
            case 1:
                return Item.ItemType.wheat_seed;
            
            case 2:
                return Item.ItemType.carrot_seed;
            
            case 3:
                return Item.ItemType.cauliflower_seed;
            
            default:
                throw new NotImplementedException();
        }
    }

    public void UpdateCrosses()
    {
        for(int i = 1; i < 4; i++)
        {
            if (last_tool == i)
            {
                _customizableSlotsCrosses[i - 1].enabled = !GetSeedsEnbaled(i);
            }
        }
    }

    public int getLastTool()
    {
        return last_tool;
    }
}
