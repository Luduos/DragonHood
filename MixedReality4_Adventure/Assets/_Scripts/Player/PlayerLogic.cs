using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour {

    [SerializeField]
    private PuzzleBox PuzzleBoxObj;

    [SerializeField]
    private RectTransform ClassDecisionUI;

    [SerializeField]
    private Image ClassIconDisplay;

    [SerializeField]
    private Sprite[] ClassIcons;

    public PlayerClassType ClassType { get; set; }

	// Use this for initialization
	void Start () {
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

    // This is dangerous. Only use classtypes <=2
    public void SetClass(int classType)
    {
        ClassType = (PlayerClassType) classType;
        ClassDecisionUI.gameObject.SetActive(false);
        Debug.Log("Class set to " + classType);

        ClassIconDisplay.sprite = ClassIcons[classType - 1];

        UpdateClassAbilities();
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
    Figher
}