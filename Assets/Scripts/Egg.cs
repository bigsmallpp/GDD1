using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public int value = 10;
    private Item _egg;

    // Start is called before the first frame update
    void Start()
    {
        _egg = new Item(Item.ItemType.egg, 1, 300);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();

        if(player != null)
        {
            player.GetPlayerInventory().AddItem(_egg);
            SaveManager.Instance.RemoveEgg(transform.position);
            Destroy(gameObject);
        }
    }
}
