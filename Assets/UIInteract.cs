using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInteract : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerController player;
    [SerializeField] private TextMeshProUGUI interactText;

    private void Show(PlantWorld plant)
    {
        containerGameObject.SetActive(true);
        interactText.text = plant.GetInteractText();
    }

    private void Hide()
    {
        containerGameObject.SetActive(false);
    }

    private void Update()
    {
        if(player.GetInteractableObject() != null)
        {
            Show(player.GetInteractableObject());
        }
        else
        {
            Hide();
        }
    }

}
