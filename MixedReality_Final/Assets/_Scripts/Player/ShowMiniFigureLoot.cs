using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author david
/// </summary>
public class ShowMiniFigureLoot : MonoBehaviour {

    [SerializeField]
    private GameObject BellObject = null;
    [SerializeField]
    private GameObject FeatherObject = null;
    [SerializeField]
    private PlayerLogic Player = null;

	// Use this for initialization
	void Start () {
        BellObject.SetActive(false);
        FeatherObject.SetActive(false);
        Player.OnClassSelected += OnClassSelected;
    }
	
	private void OnClassSelected(PlayerClassType type)
    {
        switch (type)
        {
            case PlayerClassType.PuzzleMaster:
                BellObject.SetActive(true);
                break;
            case PlayerClassType.Fighter:
                FeatherObject.SetActive(true);
                break;
            default:
                break;
        }
    }
}
