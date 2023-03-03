using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class GridManager : MonoBehaviour
{

    public static  GridManager _instance;
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _tileParent;
    [SerializeField] private SpriteRenderer _tileDisplay;

    [SerializeField] private LayerMask _rockLayer;

    [SerializeField] private TileArray _tileArray;

    private SpriteRenderer _currentTile;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        //DestroyGrid();
        GenerateGrid();
    }
    [Button]
    private void Test()
    {
        Debug.Log(_tileArray._tiles.Length);
    }

    [Button]
    private void GenerateGrid()
    {
        _tileArray._tiles = new Tile[_width,_height];

        _currentTile = Instantiate(_tileDisplay, Vector3.zero, Quaternion.identity);
        _currentTile.transform.Rotate(new Vector3(-90, 0, 0));

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                SpriteRenderer spawnedTile = Instantiate(_tileDisplay, new Vector3(x, 6, z)+transform.position, Quaternion.identity);
                spawnedTile.transform.SetParent(_tileParent);
                spawnedTile.transform.Rotate(new Vector3(-90, 0, 0));

                RaycastHit hit;
                Ray ray = new Ray((new Vector3(x, 12, z) + transform.position), Vector3.down);

                if (Physics.Raycast(ray, out hit, 10000f, _rockLayer))
                {
                    spawnedTile.color = Color.green;
                    _tileArray._tiles[x, z] = new Tile(true, false);
                }
                else
                {
                    spawnedTile.color = Color.red;
                    _tileArray._tiles[x, z] = new Tile(false, false);
                }
            }
        }
    }

    [Button]
    private void UpdateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {

                SpriteRenderer spawnedTile = Instantiate(_tileDisplay, new Vector3(x, 6, z) + transform.position, Quaternion.identity);
                spawnedTile.transform.SetParent(_tileParent);
                spawnedTile.transform.Rotate(new Vector3(-90, 0, 0));
                spawnedTile.name = $"Tile ({x},{z}) ";
            }
        }
    }

    public Tile GetTile(Vector3 worldPos)
    {
        int x_Pos = Mathf.FloorToInt(worldPos.x);
        int z_Pos = Mathf.FloorToInt(worldPos.z);
        Debug.Log($"x:{x_Pos} z:{z_Pos}");

        _currentTile.transform.position = new Vector3(x_Pos, 6, z_Pos);

        x_Pos -= (int)(transform.position.x);
        z_Pos -= (int)(transform.position.z);

        if(_tileArray._tiles[x_Pos, z_Pos]._isbuilDabble)
        {
            _currentTile.color = Color.green;
        }
        else
        {
            _currentTile.color = Color.red;
        }


        return _tileArray._tiles[x_Pos, z_Pos];
    }

    
    [Button]
    private void DestroyGrid()
    {
        for (int i = _tileParent.transform.childCount - 1; i >=0 ; i--)
        {
            DestroyImmediate(_tileParent.transform.GetChild(i).gameObject);
        }        
    }
}
