using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

public class ToolTipManager : MonoBehaviour
{

    
    [BoxGroup("Tower")][SerializeField] private Image _attackTypeImage;
    [BoxGroup("Tower")][SerializeField] TextMeshProUGUI _damageTxt;
    [BoxGroup("Tower")][SerializeField] TextMeshProUGUI _attackRateTxt;
    [BoxGroup("Tower")][SerializeField] TextMeshProUGUI _attackRangeTxt;
    [BoxGroup("Tower")][SerializeField] TextMeshProUGUI _lvlTxt;
    [BoxGroup("Tower")][SerializeField] TextMeshProUGUI _descriptionTxt;
    [BoxGroup("Enemy")][SerializeField] TextMeshProUGUI _name,_speed,_hitPoint;
    [BoxGroup("Enemy")][SerializeField] TextMeshProUGUI _frostArmor,_fireArmor,_normalArmor;
    [BoxGroup("Enemy")][SerializeField] TextMeshProUGUI _description;
    [BoxGroup("Tower")][SerializeField] private GameObject _towerWindow;
    [BoxGroup("Enemy")][SerializeField] private GameObject _enemyWindow;

    [SerializeField] private Sprite _normalDamage;
    [SerializeField] private Sprite _fireDamage;
    [SerializeField] private Sprite _iceDamage;

    private Enemy _enemy;
    private bool _isShowingEnemy;

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
        _isShowingEnemy = false;
    }

    private void ShowEnemyTooltip(Enemy enemy)
    {
        _enemy = enemy;
        _towerWindow.SetActive(false);
        _enemyWindow.SetActive(true);
        _isShowingEnemy = true;
    }

    private void HideAllToolTip()
    {
        _towerWindow.SetActive(false);
        _enemyWindow.SetActive(false);
        _isShowingEnemy = false;
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

    private void UpdateEnemyWindow(Enemy enemy)
    {
        if (enemy == null) { return; }
        _name.text = enemy._name;
        _speed.text = enemy.GetAgent().velocity.magnitude.ToString("F1");
        _hitPoint.text = enemy.GetCurrentHealth().ToString();
        _frostArmor.text = enemy.GetEnemyArmor(Projectile.DamageType.Ice).ToString();
        _fireArmor.text = enemy.GetEnemyArmor(Projectile.DamageType.Fire).ToString();
        _normalArmor.text = enemy.GetEnemyArmor(Projectile.DamageType.Normal).ToString();
        _description.text = enemy.GetDescription();
    }

    private void Update()
    {
        if (!_isShowingEnemy) return; 
        UpdateEnemyWindow(_enemy);
    }


}
