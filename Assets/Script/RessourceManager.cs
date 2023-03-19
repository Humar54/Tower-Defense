using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class RessourceManager : MonoBehaviour
{

    public static RessourceManager _instance;
    public static Action<int> _onUpdateMoney;

    [SerializeField] private GameObject _coinIncrease;
    [SerializeField] private int _startingMoney = 100;
    [SerializeField] private TextMeshProUGUI _moneyTxt;

    private int _money;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _money = _startingMoney;
        _onUpdateMoney?.Invoke(_money);
        _moneyTxt.text = _money.ToString();
        Enemy._onDeath += ReceiveMoney;
    }

    private void OnDestroy()
    {
        Enemy._onDeath -= ReceiveMoney;
    }

    public void ReceiveMoney(int value, Enemy enemy)
    {
        _money += value;
        _onUpdateMoney?.Invoke(_money);
        _moneyTxt.text = _money.ToString();
        SpawnMoneyUI(value);

    }

    public void ReceiveMoney(int value)
    {
        _money += value;
        _onUpdateMoney?.Invoke(_money);
        _moneyTxt.text = _money.ToString();
        SpawnMoneyUI(value);
    }

    private void SpawnMoneyUI(int value)
    {
        GameObject coinIncrease = Instantiate(_coinIncrease, _moneyTxt.transform.position, Quaternion.identity);
        coinIncrease.transform.SetParent(_moneyTxt.transform);
        coinIncrease.GetComponentInChildren<TextMeshProUGUI>().text = $"+{value}";
        Destroy(coinIncrease, 2f);
    }

    public void Pay(int value)
    {
        _money -= value;
        _onUpdateMoney?.Invoke(_money);
        _moneyTxt.text = _money.ToString();
    }

    public int GetMoney()
    {
        return _money;
    }
}
