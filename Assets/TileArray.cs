using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(menuName = "My Assets/TileArray")]
public class TileArray : ScriptableObject
{
    [SerializeReference] public Tile[,] _tiles;

    [Button]
    private void DebugLength()
    {
        Debug.Log(_tiles.Length);
    }
}
