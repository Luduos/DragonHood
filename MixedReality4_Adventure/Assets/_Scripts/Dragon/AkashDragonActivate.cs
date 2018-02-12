using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
/// @author: David Liebemann
/// </summary>
public class AkashDragonActivate : MonoBehaviour {

    [SerializeField]
    private PlayerLogic Player;

    [SerializeField]
    private Text InfoDisplay;

    [SerializeField]
    private TickleManager TickleManager;

    [SerializeField]
    private Text DragonFightText;


    private void Start()
    {
        TickleManager.enabled = false;
        DragonFightText.enabled = false;
    }

    private void OnMouseDown()
    {
        if (!TickleManager.enabled)
        {
            CheckPlayerClass();
        }
    }

    private void CheckPlayerClass()
    {
        if (PlayerClassType.Fighter == Player.ClassType)
        {
            if (Player.HasFeather)
            {
                TickleManager.enabled = true;
                DragonFightText.enabled = true;
            }
            else
            {
                StartCoroutine(DisplayMessage("No weapon yet - how are you going to fight?", 2.0f));
            }
        }
        else
        {
            if (Player.HasBell)
            {
                TickleManager.enabled = true;
                DragonFightText.enabled = true;
            }
            else
            {
                StartCoroutine(DisplayMessage("You need something to distract the Dragon!", 2.0f));
            }
        }
    }

    private IEnumerator DisplayMessage(string message, float messageDuration)
    {
        float elapsedTime = 0.0f;
        DragonFightText.enabled = true;
        DragonFightText.text = message;
        while (elapsedTime < messageDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        DragonFightText.text = "";
        DragonFightText.enabled = false;
    }
}
