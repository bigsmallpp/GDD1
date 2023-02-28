using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenMap : MonoBehaviour
{
    public Sprite defaultMap;
    public Sprite defaultMapLayerAbove;
    public Sprite[] mapSeasons;
    public Sprite[] mapSeasonsLayerAbove;
    public int current_season;
    public GameObject Map;
    public GameObject LayerAbove;
    private SpriteRenderer _sprite_renderer_map;
    private SpriteRenderer _sprite_renderer_layer_above;
    public FarmerShow player;
    void Awake()
    {
        _sprite_renderer_map = Map.GetComponent<SpriteRenderer>();
        _sprite_renderer_layer_above = LayerAbove.GetComponent<SpriteRenderer>();
        int season = Random.Range(0, 4);
        chooseMap(season);
        //random player model
        int playerModel = Random.Range(1, 5);
        player.GetComponent<Animator>().SetInteger("variant", playerModel);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void chooseMap(int season)
    {
        switch (season)
        {
            case 0:
                _sprite_renderer_map.sprite = mapSeasons[0];
                _sprite_renderer_layer_above.sprite = mapSeasonsLayerAbove[0];
                break;
            case 1:
                _sprite_renderer_map.sprite = mapSeasons[1];
                _sprite_renderer_layer_above.sprite = mapSeasonsLayerAbove[1];
                break;
            case 2:
                _sprite_renderer_map.sprite = mapSeasons[2];
                _sprite_renderer_layer_above.sprite = mapSeasonsLayerAbove[2];
                break;
            case 3:
                _sprite_renderer_map.sprite = mapSeasons[3];
                _sprite_renderer_layer_above.sprite = mapSeasonsLayerAbove[3];
                break;
            default:
                _sprite_renderer_map.sprite = defaultMap;
                _sprite_renderer_layer_above.sprite = defaultMapLayerAbove;
                break;
        }
    }
}
