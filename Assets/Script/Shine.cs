using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Shine : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private List<Image> _imageList;
    [SerializeField] private float _delay = 2f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.value(_imageList[0].color.a, 1, _delay).setOnUpdate(UpdateAlpha);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.value(_imageList[0].color.a, 0, _delay).setOnUpdate(UpdateAlpha);
    }

    private void UpdateAlpha(float newAlpha)
    {
        foreach (Image image in _imageList)
        {
            Color color = image.color;
            image.color = new Color(color.r, color.g, color.b, newAlpha);
        }
    }

}
