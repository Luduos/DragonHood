using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Events;

public class PuzzleBox : MonoBehaviour {

    public UnityAction OnPuzzleWasSolved;

    [SerializeField]
    private Text DebugText;

    [SerializeField]
    private GameObject Loot;

    [SerializeField]
    private PuzzleBoxFace[] BoxFaces;

    private PuzzleBoxFace IsWaitingForOppositeTouch = null;

    [SerializeField]
    private MyClientBehaviour clientBehaviour;

    public bool PuzzleBoxIsSolved { get; private set; }

    public bool IsWaiting { get; private set; }

	// Use this for initialization
	void Start () {
        Loot.SetActive(false);
        PuzzleBoxIsSolved = false;
        IsWaiting = false;
        foreach (PuzzleBoxFace face in BoxFaces)
        {
            face.OnCorrectTouchDetected += OnPuzzleBoxFaceWasCorrectlyTouched;
        }
	}

    void OnPuzzleBoxFaceWasCorrectlyTouched(PuzzleBoxFace touchedFace)
    {
        if (!PuzzleBoxIsSolved)
        {
            if (!IsWaiting)
            {
                CheckOppositeSide(touchedFace.TouchCount);
                IsWaiting = true;
                clientBehaviour.ClientTouchedBoxFace(touchedFace.TouchCount);
            }
        }
    }

    public void OnRegisteredNetworkPuzzleTouch(int ID)
    {
        BoxFaces[ID - 1].OnRegisterNetworkCorrectTouch();
        if (!IsWaiting)
        {
            IsWaitingForOppositeTouch = BoxFaces[ID - 1];
        }
        else
        {
            CheckOppositeSide(ID);
            IsWaiting = false;
        }
    }

    // Using the number of touches needed as IDs is a really bad idea. I just don't wanna implement something
    // more suffisticated right now. Sorry future me :/ :*
    private void CheckOppositeSide(int touchedID)
    {
        if(null != IsWaitingForOppositeTouch)
        {
            // the top side (which we define to have a touchcount of 5) doesn't need a partner
            if(IsWaitingForOppositeTouch.TouchCount < 5)
            {
                // we check if we have a 2 or a 4 - if yes, give back a (2-1)=1/(4-1)=3, else the other way round
                int oppositeID = IsWaitingForOppositeTouch.TouchCount % 2 == 0 ? IsWaitingForOppositeTouch.TouchCount - 1 : IsWaitingForOppositeTouch.TouchCount + 1;
                if(touchedID == oppositeID)
                {
                    OnCorrectlyTouchedOppositeSides(touchedID);
                }
                else
                {
                    OnFalselyTouchedSides(touchedID);
                }
               
            }        
        }
    }

    private void OnCorrectlyTouchedOppositeSides(int recentlyTouched)
    {
        BoxFaces[recentlyTouched - 1].OnFinalizeCorrectTouch();
        if (null != IsWaitingForOppositeTouch)
        {
            IsWaitingForOppositeTouch.OnFinalizeCorrectTouch();
            IsWaitingForOppositeTouch = null;
        }
        CheckForPuzzleCompletion();
    }

    private void OnFalselyTouchedSides(int recentlyTouched)
    {
        StartCoroutine(BoxFaces[recentlyTouched - 1].OnWrongTouchCount());
        if (null != IsWaitingForOppositeTouch)
        {
            StartCoroutine(IsWaitingForOppositeTouch.OnWrongTouchCount());
            IsWaitingForOppositeTouch = null;
        }
    }

    private void CheckForPuzzleCompletion()
    {
        bool puzzleWasJustSolved = true;

        foreach (PuzzleBoxFace face in BoxFaces)
        {
            puzzleWasJustSolved = puzzleWasJustSolved && face.WasCorrectlyTouched;
        }

        if (puzzleWasJustSolved)
        {
            PuzzleBoxIsSolved = true;
            Loot.SetActive(true);
            DebugText.text = "Solved!!!";
            if (null != OnPuzzleWasSolved)
                OnPuzzleWasSolved.Invoke();
        }
    }

    public void Reset()
    {
        PuzzleBoxIsSolved = false;
        IsWaiting = false;
        Loot.SetActive(false);
        foreach (PuzzleBoxFace face in BoxFaces)
        {
            face.ResetTouchable();
        }
        DebugText.text = "NotSolved";
    }

    public void SetFaceVisibility(bool visible)
    {
        foreach (PuzzleBoxFace face in BoxFaces)
        {
            face.SetNumberVisibility(visible);
        }
    }
}
