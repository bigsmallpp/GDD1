using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIInteract : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerController _player;
    [SerializeField] private TextMeshProUGUI interactText;

    public void Show(PlantBaseClass plant)
    {
        containerGameObject.SetActive(true);
        interactText.text = plant.getInteractText();
    }

    public void Hide()
    {
        containerGameObject.SetActive(false);
    }

    private void Update()
    {
        if (_player == null)
        {
            return;
        }
        
        if(_player.GetInteractableObject() != null)
        {
            Show(_player.GetInteractableObject());
        }
        else
        {
            Hide();
        }
    }

    public void UpdatePlayer(PlayerController player)
    {
        _player = player;
    }
}
