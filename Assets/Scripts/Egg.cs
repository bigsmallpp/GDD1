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
        _egg = new Item();
        _egg.amount = 1;
        _egg.prize = 300;
        _egg.itemType = Item.ItemType.egg;
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
            Destroy(gameObject);
        }
    }
}
