using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceTowerBtn : MonoBehaviour
{
    public static Action<Tower> _onPlaceTower;
    [SerializeField] private Button _button;
    [SerializeField] private Tower _towerToPlace;

    void Start()
    {
        _button.onClick.AddListener(PlaceTower);
    }

    private void PlaceTower()
    {
        _onPlaceTower?.Invoke(_towerToPlace);
    }
}
