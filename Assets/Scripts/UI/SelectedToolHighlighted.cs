using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedToolHighlighted : MonoBehaviour
{
    public GameObject player;
    private Image childImage;

    private int firstTool = 0;

    [SerializeField] private List<GameObject> _customizableSlots;
    [SerializeField] private List<Sprite> _tool_images;

    public Color colorSelected;
    public Color colorIdle;

    public enum ToolbarIndices
    {
        Wheat_Seeds = 1,
        Cauliflower_Seeds = 2,
        Carrot_Seeds = 3,
        Scissors = 4,
        Bucket = 5
    }

    // Start is called before the first frame update
    void Start()
    {
        childImage = transform.GetChild(firstTool).gameObject.GetComponent<Image>();
        HighlightTool(firstTool); //Highlight first tool
    }

    public void HighlightTool(int ToolNumb)
    {
        childImage.color = colorIdle; //Old childImage color

        childImage = transform.GetChild(ToolNumb).gameObject.GetComponent<Image>();
        childImage.color = colorSelected;
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
}
