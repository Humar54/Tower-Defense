using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Image _healthBar;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _displayDelay =2f;

    private float _healthTimer =0f;


    private void Start()
    {
        transform.LookAt(transform.position + new Vector3(0, 4f, -2f));
    }
    void Update()
    {

        _healthTimer += Time.deltaTime;

        if(_healthTimer >= _displayDelay)
        {
            _canvasGroup.alpha = 0f;
        }
    }

    public void UpdateHealthBar(int maxHealth, int health)
    {
        _healthBar.fillAmount = ((float)health / (float)maxHealth);
        _healthTimer = 0f;
        _canvasGroup.alpha = 1f;
    }
}
