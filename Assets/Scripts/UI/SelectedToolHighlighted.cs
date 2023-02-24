using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedToolHighlighted : MonoBehaviour
{
    public GameObject player;
    private Image childImage;

    private int firstTool = 0;

    public Color colorSelected;
    public Color colorIdle;

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
}
