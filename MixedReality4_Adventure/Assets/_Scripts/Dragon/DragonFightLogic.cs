using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Texture2D FailureSprite;
    [SerializeField]
    private Texture2D CorrectSprite;
    [SerializeField]
    private StrokeMotion[] Motions;

    private int CurrentMotion = 0;

    private void Start()
    {
        this.enabled = false;
    }

    private void OnMouseDown()
    {
        if(!this.enabled && PlayerClassType.Fighter == Player.ClassType)
        {
            StartCoroutine(DisplayMessage("Started Fight!"));
            this.enabled = true;
        }
    }


    public IEnumerator DisplayMessage(string message)
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
        yield return null;
    }

}

[System.Serializable]
public struct StrokeMotion
{
    public uint TouchCount;
    public Vector2 StrokeDirection;
    public Texture2D Icon;
}
