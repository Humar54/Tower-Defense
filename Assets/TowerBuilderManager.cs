using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

public class TowerBuilderManager : MonoBehaviour
{
    private bool _isPlacingTower;
    [SerializeField] private Camera _camera;
    [SerializeField] private Material _greenMat;
    [SerializeField] private Material _redMat;


    [SerializeField] private float _refHeight = 6f;
    [Tag] public string _rockLayer;
    public LayerMask _planeLayer;

    [SerializeField] private Transform _towerParent;

    private Tower _newTower;
    private List<MeshRenderer> _allTowerMesh;
    private List<Material> _baseTowerMaterial = new List<Material>();
    private GridManager _gridManager;
    

    private void Start()
    {
        PlaceTowerBtn._onPlaceTower += TowerPlacement;
        _gridManager = GridManager._instance;
    }

    private void TowerPlacement(Tower newTower)
    {
        _baseTowerMaterial.Clear();
        _isPlacingTower = true;
        _newTower = Instantiate(newTower, Vector3.zero, Quaternion.identity);
        _newTower.transform.SetParent(_towerParent);
        _gridManager.HideOrShowPreview(true);
        _allTowerMesh = _newTower.GetComponentsInChildren<MeshRenderer>().ToList();
        
        foreach (Renderer item in _allTowerMesh)
        {
            _baseTowerMaterial.Add(item.material);
            item.material = _greenMat;
        }
    }



    private void Update()
    {
        if (_isPlacingTower)
        {
            Vector3 mousePosition = Input.mousePosition;

            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out hit, 10000f, _planeLayer))
            {
                _gridManager.DisplayPreview(hit.point);
                _newTower.transform.position = new Vector3(Mathf.Round(hit.point.x), _refHeight, Mathf.Round(hit.point.z));
            }

            if (Input.GetButton("Fire1"))
            {
                _isPlacingTower = false;
                _gridManager.HideOrShowPreview(false);

       

                if (_gridManager.CheckIfCanbuild())
                {
                    _gridManager.SetTileHasBeenBuilt();
                    for (int i = 0; i < _allTowerMesh.Count; i++)
                    {
                        _allTowerMesh[i].material = _baseTowerMaterial[i];
                    }
                }
                else
                {

                    Destroy(_newTower.gameObject);
                    Debug.Log("Destroy!");
                }
            }
        }



    }

    private void CheckIfCanPlaceAndMove(Vector3 pixelPos, Transform objectToMove)
    {

    }


    private void ColorPreviewTower(bool canBePlaced)
    {
        /*
        if (canBePlaced)
        {
            foreach (Renderer item in _allTowerMesh)
            {
                item.material = _greenMat;
            }
        }
        else
        {
            foreach (Renderer item in _allTowerMesh)
            {
                item.material = _redMat;
            }
        }
        */
    }

    private void ColorSprite(bool IsBuildable, SpriteRenderer renderer)
    {
        if (IsBuildable)
        {
            renderer.color = Color.green;
        }
        else
        {
            renderer.color = Color.red;
        }
    }
}
