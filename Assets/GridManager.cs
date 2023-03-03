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
    [SerializeField] private Transform _displayTileParent;
    [SerializeField] private SpriteRenderer _tileDisplay;
    [SerializeField] private int _previewRange;
    [SerializeField] private LayerMask _rockLayer;
    [SerializeField] private bool _displayGrid =false;

    [SerializeField] private TileArray _tileArray;

    private List<SpriteRenderer> _displayTileList =new List<SpriteRenderer>();

    private Color _greenColor;
    private Color _redColor;
    private Tile _centerTile;

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
        _greenColor = new Color(0f, 1f, 0f, 0.05f);
        _redColor = new Color(1f, 0f, 0f, 0.05f);
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

        for (int x = 0; x <= 2*_previewRange+1; x++)
        {
            for (int y = 0; y <= 2 * _previewRange + 1; y++)
            {
                SpriteRenderer newDisplayTile = Instantiate(_tileDisplay, Vector3.zero, Quaternion.identity);
                newDisplayTile.transform.SetParent(_displayTileParent);
                newDisplayTile.transform.Rotate(new Vector3(-90, 0, 0));
                _displayTileList.Add(newDisplayTile);
            }
        }

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {

                if(_displayGrid)
                {
                    SpriteRenderer spawnedTile = Instantiate(_tileDisplay, new Vector3(x, 6, z) + transform.position, Quaternion.identity);
                    spawnedTile.transform.SetParent(_tileParent);
                    spawnedTile.transform.Rotate(new Vector3(-90, 0, 0));
                    RaycastHit hit;
                    Ray ray = new Ray((new Vector3(x, 12, z) + transform.position), Vector3.down);

                    if (Physics.Raycast(ray, out hit, 8f, _rockLayer))
                    {
                        spawnedTile.color = _greenColor;
                        _tileArray._tiles[x, z] = new Tile(x, z, true, false);
                    }
                    else
                    {
                        spawnedTile.color = _redColor;
                        _tileArray._tiles[x, z] = new Tile(x, z, false, false);
                    }
                }
                else
                {
                    RaycastHit hit;
                    Ray ray = new Ray((new Vector3(x, 12, z) + transform.position), Vector3.down);

                    if (Physics.Raycast(ray, out hit, 8f, _rockLayer))
                    {

                        _tileArray._tiles[x, z] = new Tile(x, z, true, false);
                    }
                    else
                    {
                        _tileArray._tiles[x, z] = new Tile(x, z, false, false);
                    }
                }



            }
        }
    }



    private void WorldPosToTileCoordinate(Vector3 worldPos, out int x,out int z)
    {
        x = Mathf.RoundToInt(worldPos.x);
        z = Mathf.RoundToInt(worldPos.z);
        Debug.Log($"x:{x} z:{z}");

        x -= (int)(transform.position.x);
        z -= (int)(transform.position.z);
    }

    public Tile GetTile(Vector3 worldPos)
    {
        int x = 0;
        int z = 0;
        WorldPosToTileCoordinate(worldPos, out x, out z);

        if(CheckIfXZInsideArray(x,z))
        {
            return _tileArray._tiles[x, z];
        }
        else
        {
            return null;
        }
        
    }

    public Tile GetNearbyTile(Tile tile, int xOffset,int zOffset)
    {
        int xWithOffset = tile._xPos + xOffset;
        int zWithOffset = tile._zPos + zOffset;

        if (CheckIfXZInsideArray(xWithOffset,zWithOffset))
        {
            return _tileArray._tiles[xWithOffset, zWithOffset];
        }
        else
        {
            return null;
        }
    }

    public bool CheckIfXZInsideArray(int x, int z)
    {
        if(x < _tileArray._tiles.GetLength(0) && x >= 0 && z < _tileArray._tiles.GetLength(1) && z >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DisplayPreview(Vector3 pos)
    {

        _centerTile = GetTile(pos);
        if (_centerTile == null) { return; }

        for (int x = -_previewRange; x <= _previewRange; x++)
        {
            for (int z = -_previewRange; z <= _previewRange; z++)
            {
                int index = (x + _previewRange) * (2 * _previewRange + 1) + (z + _previewRange);
                Tile nearbyTile = GetNearbyTile(_centerTile, x, z);
                if(nearbyTile!=null)
                {
                    MoveDisplayTile(nearbyTile, index);
                    SwitchColorDisplayTile(nearbyTile, index);
                }
                else
                {
                    MoveOutOfSight(index);
                }
            }
        }
    }


    public void HideOrShowPreview(bool value)
    {
        foreach (SpriteRenderer renderer in _displayTileList)
        {
            renderer.gameObject.SetActive(value);
        }
    }

    public void MoveDisplayTile(Tile tile,int index)
    {
        _displayTileList[index].transform.position = new Vector3(tile._xPos, 6, tile._zPos) +transform.position;
    }

    public void MoveOutOfSight(int index)
    {
        _displayTileList[index].transform.position = Vector3.one * 1000f;

    }

    public void SwitchColorDisplayTile(Tile tile, int index)
    {
        if (tile._isbuilDabble && !tile._hasBeenBuilt)
        {
            _displayTileList[index].color = _greenColor;
        }
        else
        {
            _displayTileList[index].color = _redColor;
        }
    }

    
    [Button]
    private void DestroyGrid()
    {
        for (int i = _tileParent.transform.childCount - 1; i >=0 ; i--)
        {
            DestroyImmediate(_tileParent.transform.GetChild(i).gameObject);
        }        
    }

    public bool CheckIfCanbuild()
    {
        return (_centerTile._isbuilDabble && !_centerTile._hasBeenBuilt);
    }

    public void  SetTileHasBeenBuilt()
    {
        _centerTile._hasBeenBuilt = true;
    }
}
