using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Shine : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private List<Image> _imageList;
    [SerializeField] private float _delay = 2f;
    private bool _isDisabled;
    private Color _startingColor;

    private void Awake()
    {
        _startingColor = _imageList[0].color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.value(_imageList[0].color.a, 1, _delay).setOnUpdate(UpdateAlpha).setOnComplete(CancelLeanTween);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.value(_imageList[0].color.a, 0, _delay).setOnUpdate(UpdateAlpha).setOnComplete(CancelLeanTween);
    }

    private void UpdateAlpha(float newAlpha)
    {
        foreach (Image image in _imageList)
        {
            image.color = new Color(_startingColor.r, _startingColor.g, _startingColor.b, newAlpha);
        }
    }

    private void OnDisable()
    {
        
        foreach (Image image in _imageList)
        {
            image.color = new Color(0, 0, 0, 1);
        }
    }

    private void CancelLeanTween()
    {
        //LeanTween.cancelAll();
    }
    private void OnDestroy()
    {
        LeanTween.cancelAll();
    }

    private void OnEnable()
    {
        foreach (Image image in _imageList)
        {
            image.color = _startingColor;
        }
    }
}
