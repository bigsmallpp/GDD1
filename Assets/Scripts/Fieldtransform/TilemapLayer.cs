using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Fieldtransform
{
    public class TilemapLayer : MonoBehaviour
    {
        protected Tilemap tilemap_ { get; private set; }
        protected void Awake()
        {
            tilemap_ = GetComponent<Tilemap>();
        }
    }
}