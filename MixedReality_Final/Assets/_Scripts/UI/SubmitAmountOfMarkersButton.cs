using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// @author: David Liebemann
/// </summary>
[RequireComponent(typeof(Button))]
public class SubmitAmountOfMarkersButton : MonoBehaviour {

    [SerializeField]
    private Slider PuzzleBoxAmountSlider = null;

    [SerializeField]
    private Slider DragonAmountSlider = null;

    [SerializeField]
    private CreatorLogic Creator = null;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnSubmit);
    }

    private void OnSubmit()
    {
        List<uint> amounts = new List<uint>();
        amounts.Add((uint)PuzzleBoxAmountSlider.value);
        amounts.Add((uint)DragonAmountSlider.value);
        Creator.SetAmountOfPieces(amounts);
    }
}
