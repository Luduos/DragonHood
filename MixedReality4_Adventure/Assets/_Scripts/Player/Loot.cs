using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// @author: David Liebemann
/// </summary>
public class Loot : MonoBehaviour {

    [SerializeField]
    private PlayerLogic Player;

    [Header("Feather Info")]
    [SerializeField]
    private Sprite FeatherIcon;
    [SerializeField]
    private GameObject FeatherMesh;

    [Header("Bell Info")]
    [SerializeField]
    private Sprite BellIcon;
    [SerializeField]
    private GameObject BellMesh;


    [Header("Display")]
    [SerializeField]
    private Text InfoDisplay;
    [SerializeField]
    private Image LootIconDisplay;

    private void OnEnable()
    {
        if (PlayerClassType.Fighter == Player.ClassType)
        {
            FeatherMesh.gameObject.SetActive(true);
            BellMesh.gameObject.SetActive(false);
        }
        else
        {
            BellMesh.gameObject.SetActive(true);
            FeatherMesh.gameObject.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if(PlayerClassType.Fighter == Player.ClassType)
        {
            Player.HasFeather = true;
            Player.HasBell = false;
            LootIconDisplay.color = Color.white;
            LootIconDisplay.sprite = FeatherIcon;
            this.gameObject.SetActive(false);
        }
        else
        {
            Player.HasBell = true;
            Player.HasFeather = false;
            LootIconDisplay.color = Color.white;
            LootIconDisplay.sprite = BellIcon;
            this.gameObject.SetActive(false);
        }
    }
}
