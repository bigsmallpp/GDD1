using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonSpriteHolder : MonoBehaviour
{
    [SerializeField] private List<Sprite> _season_images_;
    [SerializeField] private Image _current_image;

    public void SetSeasonSprite(Utils.Season season)
    {
        _current_image.sprite = _season_images_[(int)season];
    }
}
