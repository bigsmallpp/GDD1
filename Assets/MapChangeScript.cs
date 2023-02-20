using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChangeScript : MonoBehaviour
{
    public Sprite defaultMap;
    public Sprite[] mapSeasons;
    public int current_season = 1;
    private SpriteRenderer _sprite_renderer;
    void Awake()
    {
        current_season = SceneLoader.Instance.current_season;
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
        switch (current_season)
        {
            case 1:
            _sprite_renderer.sprite = mapSeasons[0];
            break;
            case 2:
            _sprite_renderer.sprite = mapSeasons[1];
            break;
            case 3:
            _sprite_renderer.sprite = mapSeasons[2];
            break;
            case 4:
            _sprite_renderer.sprite = mapSeasons[3];
            break;
            default:
            _sprite_renderer.sprite = defaultMap;
            break;
        }
    }
}
