
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    [SerializeField] private Image _speedImage;
    [SerializeField] private Image _slowImage;
    [SerializeField] private Image _armorImage;

    private void Update()
    {
        transform.LookAt(transform.position + new Vector3(0, 4f, -2f));
    }

    public void ActivateSpeed()
    {
        _speedImage.gameObject.SetActive(true);
    }

    public void DisactivateSpeed()
    {
        _speedImage.gameObject.SetActive(false);
    }

    public void ActivateSlow()
    {
        _slowImage.gameObject.SetActive(true);
    }

    public void DisactivateSlow()
    {
        _slowImage.gameObject.SetActive(false);
    }

    public void ActivateArmor()
    {
        _armorImage.gameObject.SetActive(true);
    }

    public void DisactivateArmor()
    {
        _armorImage.gameObject.SetActive(false);
    }
}
