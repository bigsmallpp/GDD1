using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantWorld : MonoBehaviour
{
    UIInteract uiInteract;
    private bool clickable = false;

    [SerializeField] private string interactText;
    public static PlantWorld  SpawnPlantWorld(Vector3 position,Item item)
    {
        Transform transform = Instantiate(ItemAssets.Instance.pfPlantWorld, position, Quaternion.identity );
        PlantWorld plantWorld = transform.GetComponent<PlantWorld>();
        plantWorld.SetItem(item);

        return plantWorld;
    }

    private SpriteRenderer spriteRenderer;
    private Item item;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetItem(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.GetSprite();
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
        
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public bool getClickableState()
    {
        return clickable;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            clickable = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            clickable = true;
        }
    }

}
