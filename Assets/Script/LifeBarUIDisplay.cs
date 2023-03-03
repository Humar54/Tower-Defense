using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarUIDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _lifeDisplay;
    [SerializeField] private RectTransform _lifeBarImage;

    private Color _startingColor;

    private void Start()
    {
        _startingColor = _lifeBarImage.GetComponent<Image>().color;
    }
    public  void UpdateLifeBar( int currentLife, int startingLife)
    {
        _lifeDisplay.text = $"{currentLife}/{startingLife}";
        _lifeBarImage.localScale = new Vector3((float)currentLife / (float)startingLife, 1f, 1f);

        _lifeBarImage.GetComponent<Image>().color =Color.Lerp(_startingColor,Color.red, 1f-(float)currentLife / (float)startingLife);
    }
}
