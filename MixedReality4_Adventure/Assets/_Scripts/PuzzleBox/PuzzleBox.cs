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

    public bool PuzzleBoxIsSolved { get; private set; }

	// Use this for initialization
	void Start () {
        Loot.SetActive(false);
        PuzzleBoxIsSolved = false;
        foreach (PuzzleBoxFace face in BoxFaces)
        {
            face.OnCorrectTouchDetected += OnPuzzleBoxFaceWasCorrectlyTouched;
        }
	}

    void OnPuzzleBoxFaceWasCorrectlyTouched()
    {
        bool puzzleSolved = true;
        foreach(PuzzleBoxFace face in BoxFaces)
        {
            puzzleSolved = puzzleSolved && face.WasCorrectlyTouched;
        }
        if (!PuzzleBoxIsSolved && puzzleSolved)
        {
            if(null != OnPuzzleWasSolved)
                OnPuzzleWasSolved.Invoke();

            Loot.SetActive(true);
            DebugText.text = "Solved!!!";
        }
    }


    public void Reset()
    {
        PuzzleBoxIsSolved = false;
        Loot.SetActive(false);
        foreach (PuzzleBoxFace face in BoxFaces)
        {
            face.ResetTouchable();
        }
        DebugText.text = "NotSolved";
    }
}
