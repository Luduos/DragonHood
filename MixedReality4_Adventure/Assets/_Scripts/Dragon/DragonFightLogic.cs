using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @author: David Liebemann
/// </summary>
public class DragonFightLogic : MonoBehaviour {
    [SerializeField]
    private Text InfoDisplay;
    [SerializeField]
    private Camera ARCamera;
    [SerializeField]
    private PlayerLogic Player;

    [SerializeField]
    private Image MotionDisplay;

    [SerializeField]
    private Sprite FailureSprite;
    [SerializeField]
    private Sprite CorrectSprite;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float MotionPrecision = 0.5f;
    [SerializeField]
    private StrokeMotion[] Motions;

    private bool IsDisplayingSymbol;
    private bool DragonIsDead;
    private int CurrentMotion = 0;
    private Vector2 TouchStart;

    private void Start()
    {
        IsDisplayingSymbol = false;
        this.enabled = false;
        DragonIsDead = false;
    }


    private void Update()
    {
        if (!Application.isEditor && Input.touchCount < 1)
            return;

        if (IsDisplayingSymbol)
            return;

        MotionDisplay.sprite = Motions[CurrentMotion].Icon;
        MotionDisplay.rectTransform.rotation = Quaternion.LookRotation(Vector3.forward, Motions[CurrentMotion].StrokeDirection.normalized);
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("TouchStart");
                TouchStart = Input.mousePosition;

            }
            if (Input.GetMouseButtonUp(0))
            {
                CheckTouchEnd(Input.mousePosition);
                Debug.Log("Touch End");

            }
        }
        else
        {
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    TouchStart = Input.GetTouch(0).position;
                    Debug.Log("TouchStart");
                    break;
                case TouchPhase.Moved:
                    break;
                case TouchPhase.Ended:
                    Debug.Log("Touch End");
                    CheckTouchEnd(Input.GetTouch(0).position);
                    break;
                default:
                    break;
            }
        }
    }

    private void CheckTouchEnd(Vector2 touchEnd)
    {
        Vector2 startToEnd = touchEnd - TouchStart;
        float correctness = Vector2.Dot(startToEnd.normalized, Motions[CurrentMotion].StrokeDirection.normalized);
        if(correctness > MotionPrecision)
        {
            StartCoroutine(DisplaySymbol(CorrectSprite, 1.0f));
            CurrentMotion = CurrentMotion + 1;
            if(CurrentMotion >= Motions.Length)
            {
                OnEndFight();
            }
        }
        else
        {
            StartCoroutine(DisplaySymbol(FailureSprite, 1.0f));
        }
    }

    private void OnMouseDown()
    {
        if(!DragonIsDead && !this.enabled && PlayerClassType.Fighter == Player.ClassType)
        {
            OnStartFight();
        }
    }

    private void OnStartFight()
    {
        StartCoroutine(DisplayMessage("Started Fight!"));
        StartCoroutine(DelayedStart(1f));
    }

    private void OnEndFight()
    {
        this.enabled = false;
        DragonIsDead = true;
        StartCoroutine(DisplayMessage("Won Fight!"));
        MotionDisplay.gameObject.SetActive(false);
    }

    private IEnumerator DelayedStart(float delayTime)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < delayTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.enabled = true;
        MotionDisplay.gameObject.SetActive(true);
    }

    private IEnumerator DisplaySymbol(Sprite icon, float displayTime)
    {
        IsDisplayingSymbol = true;
        float elapsedTime = 0.0f;
        MotionDisplay.sprite = icon;
        MotionDisplay.rectTransform.rotation = Quaternion.identity;
        while (elapsedTime < displayTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        IsDisplayingSymbol = false;
    }

    private IEnumerator DisplayMessage(string message)
    {
        float elapsedTime = 0.0f;
        float allertedTime = 2.0f;

        InfoDisplay.text = message;
        while (elapsedTime < allertedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        InfoDisplay.text = "";
    }

}

[System.Serializable]
public struct StrokeMotion
{
    public uint TouchCount;
    public Vector2 StrokeDirection;
    public Sprite Icon;
}
