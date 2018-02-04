using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// @author: David Liebemann
/// </summary>
[RequireComponent(typeof(Button))]
public class SaveButton : MonoBehaviour {

    [SerializeField]
    private InputField InputField;

    [SerializeField]
    private SaveAdventure SaveLogic;

    private void Start()
    {
        Button button = this.GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        SaveLogic.OnSaveAdventure(InputField.text);
    }
}