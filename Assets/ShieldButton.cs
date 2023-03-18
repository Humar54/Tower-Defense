using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShieldButton : MonoBehaviour
{
    [SerializeField] private int _shieldCost = 50;
    [SerializeField] private GameObject _shield;
    [SerializeField] private float _shieldDelay = 10f;

    [SerializeField] private TextMeshProUGUI _textCash;
    private void Start()
    {
        RessourceManager._onUpdateMoney += DisableButton;
        _textCash.text = $"-{_shieldCost.ToString()}-$";
    }

    public void ActivateShield()
    {
        RessourceManager._instance.Pay(_shieldCost);
        _shieldCost += 50;
        _textCash.text = $"-{_shieldCost.ToString()}-$";
        DisableButton(RessourceManager._instance.GetMoney());
        _shield.SetActive(true);
        StartCoroutine(StopShield());
    }

    private void DisableButton(int money)
    {
        if(_shieldCost <= money)
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }
    private void OnDestroy()
    {
        RessourceManager._onUpdateMoney -= DisableButton;
    }

    private IEnumerator StopShield()
    {
        yield return new WaitForSeconds(_shieldDelay);
        _shield.GetComponent<Animator>().SetBool("_FadeBarrier", true);
        yield return new WaitForSeconds(2f);
        _shield.SetActive(false);
        _shield.GetComponent<Animator>().SetBool("_FadeBarrier", false);
    }
}
