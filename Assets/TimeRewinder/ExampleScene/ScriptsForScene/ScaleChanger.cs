using UnityEngine;
using UnityEngine.UI;

public class ScaleChanger : MonoBehaviour
{
    [SerializeField] private Text textToUpdate;
    [SerializeField] private Slider slider;

    public void OnSliderChange(float value)
    {
        transform.localScale = new Vector3(value, value, value);
        textToUpdate.text = value.ToString("0.00");
    }
}