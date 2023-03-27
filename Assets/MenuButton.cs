using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MenuButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup _blackScreen;

    [SerializeField] private Button _btn;
    private bool _isMenuOn;

    private void Start()
    {
        _btn.onClick.AddListener(ToggleMenu);
    }

    public void ToggleMenu()
    {
        _isMenuOn = !_isMenuOn;

        if(_isMenuOn)
        {
            Time.timeScale = 0;


            LeanTween.value(_blackScreen.gameObject, 0, 0.3f, 0.5f).setOnUpdate(UpdateAlpha).setIgnoreTimeScale(true);
        }
        else
        {
            Time.timeScale = 1;
            LeanTween.value(_blackScreen.gameObject, 0, 0.0f, 0.5f).setOnUpdate(UpdateAlpha).setIgnoreTimeScale(true);

        }
    }

    private void UpdateAlpha(float value)
    {
        _blackScreen.alpha = value;
    }
}
