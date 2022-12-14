using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Fieldtransform
{
    [CreateAssetMenu(menuName = "Seeds", fileName = "New Seed")]
    public class Seeds : ScriptableObject
    {
        [field:SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public TileBase tile { get; private set; }

    }
}