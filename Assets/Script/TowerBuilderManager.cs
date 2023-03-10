using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

using UnityEngine.EventSystems;
using System;

public class TowerBuilderManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public static Action<bool> _onEnterExit;
    [Tag] public string _rockLayer;

    [SerializeField] private Camera _camera;
    [SerializeField] private Material _greenMat;
    [SerializeField] private Material _redMat;
    [SerializeField] private Transform _towerParent;
    [SerializeField] private float _refHeight = 6f;

    private Tower _newTower;
    private List<MeshRenderer> _allTowerMesh;
    private List<Material> _baseTowerMaterial = new List<Material>();
    private GridManager _gridManager;


    public LayerMask _planeLayer;

    private bool _isPlacingTower;

    private bool _previousCanBuild;

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

            bool canBuild = false;

            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out hit, 10000f, _planeLayer))
            {
                _gridManager.DisplayPreviewGrid(hit.point);
                canBuild = _gridManager.CheckIfCanbuild();

                _newTower.transform.position = new Vector3(Mathf.Round(hit.point.x), _refHeight, Mathf.Round(hit.point.z));

                if (_previousCanBuild != canBuild)
                {
                    if (canBuild)
                    {
                        for (int i = 0; i < _allTowerMesh.Count; i++)
                        {
                            _allTowerMesh[i].material = _greenMat;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < _allTowerMesh.Count; i++)
                        {
                            _allTowerMesh[i].material = _redMat;
                        }
                    }
                    _previousCanBuild = canBuild;
                }
            }

            if (Input.GetButton("Fire1"))
            {
                _isPlacingTower = false;
                _gridManager.HideOrShowPreview(false);

                if (canBuild)
                {
                    _gridManager.SetTileHasBeenBuilt();
                    _newTower.Build();
                    RessourceManager._instance.Pay(_newTower.GetPrice());
                    for (int i = 0; i < _allTowerMesh.Count; i++)
                    {
                        _allTowerMesh[i].material = _baseTowerMaterial[i];
                    }
                }
                else
                {
                    Destroy(_newTower.gameObject);
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        _onEnterExit?.Invoke(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _onEnterExit?.Invoke(true);
    }
}
