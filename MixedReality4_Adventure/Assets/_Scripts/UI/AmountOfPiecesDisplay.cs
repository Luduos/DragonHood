using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @author: David Liebemann
/// </summary>
[RequireComponent(typeof(Text))]
public class AmountOfPiecesDisplay : MonoBehaviour {

    [SerializeField]
    private string BaseMessage;

    private Text AmountDisplay;

    private void Start()
    {
        AmountDisplay = GetComponent<Text>();
    }

    public void OnSliderValueChanged(Slider slider)
    {
        AmountDisplay.text = BaseMessage + (int) slider.value;
    }
}
