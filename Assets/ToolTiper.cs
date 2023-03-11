using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTiper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static Action<Tower, int> _OnDisplayTowertTooltip;
    public static Action<Enemy> _OnDisplayEnemytTooltip;
    public static Action _OnHideTooltip;

    [SerializeField] private Tower _tower;
    [Range(0, 1)][SerializeField] private int _offset;
    private Enemy _enemy;

    void Start()
    {
        _enemy = GetComponent<Enemy>();

        if (_tower == null)
        {
            _tower = GetComponent<Tower>();
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_tower)
        {
            int level = GameManager._instance.GetTowerLvl(_tower.GetTowerType()) + _offset + 1;

            if (level < 4)
            {
                _OnDisplayTowertTooltip?.Invoke(_tower, _offset);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _OnHideTooltip?.Invoke();
    }

    public void OnMouseEnter()
    {
        if (_tower)
        {
            _OnDisplayTowertTooltip?.Invoke(_tower, _offset);
        }
        else if (_enemy)
        {
            _OnDisplayEnemytTooltip?.Invoke(_enemy);
        }
    }

    public void OnMouseExit()
    {
        _OnHideTooltip?.Invoke();
    }



    // Update is called once per frame
    void Update()
    {

    }
}
