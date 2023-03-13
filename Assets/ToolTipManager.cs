using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTipManager : MonoBehaviour
{
    [SerializeField] private GameObject _towerWindow;
    [SerializeField] private GameObject _enemyWindow;

    [SerializeField] private Sprite _normalDamage;
    [SerializeField] private Sprite _fireDamage;
    [SerializeField] private Sprite _iceDamage;

    [SerializeField] private Image _attackTypeImage;
    [SerializeField] TextMeshProUGUI _damageTxt;
    [SerializeField] TextMeshProUGUI _attackRateTxt;
    [SerializeField] TextMeshProUGUI _attackRangeTxt;
    [SerializeField] TextMeshProUGUI _lvlTxt;
    [SerializeField] TextMeshProUGUI _descriptionTxt;

    private void Start()
    {
        ToolTiper._OnDisplayEnemytTooltip += ShowEnemyTooltip;
        ToolTiper._OnDisplayTowertTooltip += ShowTowerWindow;
        ToolTiper._OnHideTooltip += HideAllToolTip;
    }
    private void OnDestroy()
    {
        ToolTiper._OnDisplayEnemytTooltip -= ShowEnemyTooltip;
        ToolTiper._OnDisplayTowertTooltip -= ShowTowerWindow;
        ToolTiper._OnHideTooltip -= HideAllToolTip;
    }

    private void ShowTowerWindow(Tower tower, int offset)
    {
        _towerWindow.SetActive(true);
        _enemyWindow.SetActive(false);
        UpdateTowerWindow(tower,offset);
    }

    private void ShowEnemyTooltip(Enemy enemy)
    {
        _towerWindow.SetActive(false);
        _enemyWindow.SetActive(true);
    }

    private void HideAllToolTip()
    {
        _towerWindow.SetActive(false);
        _enemyWindow.SetActive(false);
    }

    private void UpdateTowerWindow(Tower tower,int offset)
    {
        if (tower.GetProjectile(offset)._damageType == Projectile.DamageType.Fire)
        {
            _attackTypeImage.sprite = _fireDamage;
        }
        else if (tower.GetProjectile(offset)._damageType == Projectile.DamageType.Ice)
        {
            _attackTypeImage.sprite = _iceDamage;
        }
        else if (tower.GetProjectile(offset)._damageType == Projectile.DamageType.Normal)
        {
            _attackTypeImage.sprite = _normalDamage;
        }
        TowerStats stats = tower.GetTowerStats(offset);

        _damageTxt.text = stats._damage.ToString();
        _attackRateTxt.text = stats._attackDelay.ToString();
        _attackRangeTxt.text = stats._attackRange.ToString();
        _lvlTxt.text = (GameManager._instance.GetTowerLvl(tower.GetTowerType()) +offset+1).ToString();
        _descriptionTxt.text = tower.GetToolTip(offset);
    }
}
