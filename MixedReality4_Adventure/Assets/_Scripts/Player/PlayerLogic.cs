using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
/// <summary>
/// @author: David Liebemann
/// </summary>
public class PlayerLogic : MonoBehaviour
{
    [SerializeField]
    private PuzzleBox PuzzleBoxObj;

    [SerializeField]
    private RectTransform ClassDecisionUI;

    [SerializeField]
    private Image ClassIconDisplay;

    [SerializeField]
    private Sprite[] ClassIcons;

    public PlayerClassType ClassType { get; set; }

    public bool HasFeather { get; set; }
    public bool HasBell { get; set; }

    public UnityAction<PlayerClassType> OnClassSelected;

    // Use this for initialization
    void Start()
    {
        HasFeather = false;
        HasBell = false;
        ClassType = PlayerClassType.NotChosen;
        ClassDecisionUI.gameObject.SetActive(false);
        UpdateClassAbilities();
    }

    public void OnStartClassDecision()
    {
        ClassDecisionUI.gameObject.SetActive(true);
    }

    public void OnRegisteredPuzzleClick(int ID)
    {
        PuzzleBoxObj.OnRegisteredNetworkPuzzleTouch(ID);
    }

    public void OnWrongPuzzleTouch()
    {
        PuzzleBoxObj.OnFalselyTouchedSides();
    }

    // This is dangerous. Only use classtypes <=2
    public void SetClass(int classType)
    {
        ClassType = (PlayerClassType)classType;
        ClassDecisionUI.gameObject.SetActive(false);
        Debug.Log("Class set to " + classType);

        ClassIconDisplay.sprite = ClassIcons[classType - 1];

        UpdateClassAbilities();
        if (null != OnClassSelected)
            OnClassSelected.Invoke(ClassType);
    }

    private void UpdateClassAbilities()
    {
        // make faces of puzzle visible only for puzzle master
        PuzzleBoxObj.SetFaceVisibility(PlayerClassType.PuzzleMaster == ClassType);
    }
}

[System.Serializable]
public enum PlayerClassType
{
    NotChosen,
    PuzzleMaster,
    Fighter
}