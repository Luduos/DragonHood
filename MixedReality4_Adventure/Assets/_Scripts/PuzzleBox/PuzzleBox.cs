﻿using UnityEngine;
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

    private bool IsWaiting;

    [SerializeField]
    private MyClientBehaviour clientBehaviour;

    public bool PuzzleBoxIsSolved { get; private set; }

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
            if (!IsWaiting && !touchedFace.WasCorrectlyTouched)
            {
                IsWaiting = true;
                touchedFace.OnRegisterNetworkCorrectTouch();
                clientBehaviour.ClientTouchedBoxFace(touchedFace.TouchCount);
            }
        }
    }

    public void OnRegisteredNetworkPuzzleTouch(int ID)
    {
        if (ID == 5)
        {
            OnCorrectlyTouchedOppositeSides(ID);
        }else if (IsWaiting)
        {
            if(!BoxFaces[ID - 1].WasCorrectlyTouched)
            {
                BoxFaces[ID - 1].OnRegisterNetworkCorrectTouch();
                CheckOppositeSide(ID);
            }
        }
        else
        {
            BoxFaces[ID - 1].OnRegisterNetworkCorrectTouch();
        }
    }

    // Using the number of touches needed as IDs is a really bad idea. I just don't wanna implement something
    // more suffisticated right now. Sorry future me :/ :*
    private void CheckOppositeSide(int touchedID)
    {
        // we check if we have a 2 or a 4 - if yes, give back a (2-1)=1/(4-1)=3, else the other way round
        int oppositeID = touchedID % 2 == 0 ? touchedID - 1 : touchedID + 1;
        if (BoxFaces[oppositeID - 1].WasCorrectlyTouched)
        {
            OnCorrectlyTouchedOppositeSides(touchedID);
        }
        else
        {
            OnFalselyTouchedSides(touchedID);
        }
    }

    private void OnCorrectlyTouchedOppositeSides(int recentlyTouched)
    {
        IsWaiting = false;
        BoxFaces[recentlyTouched - 1].OnFinalizeCorrectTouch();
        if (recentlyTouched < 5)
        {
            int oppositeID = recentlyTouched % 2 == 0 ? recentlyTouched - 1 : recentlyTouched + 1;
            BoxFaces[oppositeID-1].OnFinalizeCorrectTouch();
        }
        CheckForPuzzleCompletion();
    }

    private void OnFalselyTouchedSides(int recentlyTouched)
    {
        IsWaiting = false;

        foreach (PuzzleBoxFace face in BoxFaces)
        {
            StartCoroutine(face.OnWrongTouchCount());
        }

        /*
        StartCoroutine(BoxFaces[recentlyTouched - 1].OnWrongTouchCount());
        if (recentlyTouched < 5)
        {
            int oppositeID = recentlyTouched % 2 == 0 ? recentlyTouched - 1 : recentlyTouched + 1;
            BoxFaces[oppositeID - 1].OnWrongTouchCount();
        }
        */
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