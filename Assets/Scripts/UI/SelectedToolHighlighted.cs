using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedToolHighlighted : MonoBehaviour
{
    public GameObject player;

    private int currentTool;
    private int currentToolOld;
    
    private Image childImage;

    public Color colorSelected;
    public Color colorIdle;

    // Start is called before the first frame update
    void Start()
    {
        currentTool = player.GetComponent<PlayerController>().currentToolNumb;
        childImage = transform.GetChild(currentTool).gameObject.GetComponent<Image>();
        currentToolOld = 0;
        HighlightTool(currentTool); //Highlight first tool
    }

    private void Update()
    {
        currentTool = player.GetComponent<PlayerController>().currentToolNumb;
        
        if (currentTool != currentToolOld)
        {
            HighlightTool(currentTool);
            currentToolOld = currentTool;
        }
        
    }

    private void HighlightTool(int ToolNumb)
    {
        childImage.color = colorIdle; //Old childImage color

        childImage = transform.GetChild(ToolNumb).gameObject.GetComponent<Image>();
        childImage.color = colorSelected;

    }

}
