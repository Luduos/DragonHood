using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour {
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
    }

    public void OnChooseClass()
    {
        ClassDecisionUI.gameObject.SetActive(true);
    }

    // This is dangerous. Only use classtypes <=2
    public void SetClass(int classType)
    {
        ClassType = (PlayerClassType) classType;
        ClassDecisionUI.gameObject.SetActive(false);
        Debug.Log("Class set to " + classType);

        ClassIconDisplay.sprite = ClassIcons[classType - 1];
    }
}

[System.Serializable]
public enum PlayerClassType
{
    NotChosen,
    PuzzleMaster,
    Figher
}