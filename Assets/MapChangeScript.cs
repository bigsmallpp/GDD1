using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChangeScript : MonoBehaviour
{
    public Sprite defaultMap;
    public Sprite[] mapSeasons;
    public int choose_test;
    private SpriteRenderer _sprite_renderer;
    void Awake()
    {
        _sprite_renderer = GetComponent<SpriteRenderer>();
        chooseMap();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void chooseMap()
    {
        switch (choose_test)
        {
            case 1:
            _sprite_renderer.sprite = mapSeasons[0];
            break;
            case 2:
            break;
            case 3:
            break;
            case 4:
            break;
            default:
            _sprite_renderer.sprite = defaultMap;
            break;
        }
    }
}
