using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaceTowerBtn : MonoBehaviour
{
    public static Action<Tower> _onPlaceTower;
    [SerializeField] private Button _button;
    [SerializeField] private Button _upgradeBtn;
    [SerializeField] private Tower _towerToPlace;

    [SerializeField] private TextMeshProUGUI _towerPriceTxt;
    [SerializeField] private TextMeshProUGUI _towerUpgradePriceTxt;


    void Start()
    {
        _button.onClick.AddListener(PlaceTower);
        _upgradeBtn.onClick.AddListener(UpgradeTower);

        RessourceManager._onUpdateMoney += DisableButton;
        GameManager._onUpdateTower += UpdateTowerPrice;
        GameManager._onUpdateTower += UpdateTowerUpgradePrice;

        UpdateTowerPrice(_towerToPlace.GetTowerType());
        UpdateTowerUpgradePrice(_towerToPlace.GetTowerType());
    }

    private void OnDestroy()
    {
        RessourceManager._onUpdateMoney -= DisableButton;
        GameManager._onUpdateTower -= UpdateTowerPrice;
        GameManager._onUpdateTower -= UpdateTowerUpgradePrice;
    }

    private void UpgradeTower()
    {
        _upgradeBtn.interactable = false;
        RessourceManager._instance.Pay(_towerToPlace.GetTowerStats(0)._towerUpgradePrice);
        _upgradeBtn.GetComponent<Shine>().enabled = false;
        GameManager._instance.IncreaseTowerLvl(_towerToPlace.GetTowerType());
    }
    private void DisableButton(int money)
    {
        if (_towerToPlace.GetTowerStats(0)._price <= money)
        {
            _button.interactable = true;
            GetComponent<Shine>().enabled = true;
        }
        else
        {
            _button.interactable = false;
            GetComponent<Shine>().enabled = false;
        }

        int upgradePrice = _towerToPlace.GetTowerStats(0)._towerUpgradePrice;

        if (upgradePrice <= money && upgradePrice != 0)
        {
            _upgradeBtn.interactable = true;
            _upgradeBtn.GetComponent<Shine>().enabled = true;
        }
        else
        {
            _upgradeBtn.interactable = false;
            _upgradeBtn.GetComponent<Shine>().enabled = false;
        }
    }

    private void UpdateTowerPrice(Tower.TowerType type)
    {
        if (_towerToPlace.GetTowerType() == type)
        {
            _towerPriceTxt.text = $"-{_towerToPlace.GetTowerStats(0)._price.ToString()}$-";
        }
    }

    private void UpdateTowerUpgradePrice(Tower.TowerType type)
    {
        if (_towerToPlace.GetTowerType() == type)
        {
            float price = _towerToPlace.GetTowerStats(0)._towerUpgradePrice;
            if (price == 0)
            {
                _upgradeBtn.interactable = false;
                _upgradeBtn.GetComponent<Shine>().enabled = false;
            }
            _towerUpgradePriceTxt.text = price.ToString() + "$";
        }
    }

    private void PlaceTower()
    {
        _onPlaceTower?.Invoke(_towerToPlace);
    }
}
