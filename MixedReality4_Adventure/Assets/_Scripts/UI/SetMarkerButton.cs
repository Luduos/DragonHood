using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @author: David Liebemann
/// 
/// Used on the Set Marker Buttons to display the correct amount of currently set
/// markers and to deactivate, if the max amount has been reached.
/// 
/// I don't even know. This seems really dumb. Should do something with enums, but
/// that would take so much more time...
/// </summary>
[RequireComponent(typeof(Button))]
public class SetMarkerButton : MonoBehaviour {
    [SerializeField]
    private int TypeIDToReactTo = 0;

    [SerializeField]
    private Text ButtonHeader = null;

    [SerializeField]
    private CreatorLogic Creator = null;

    [SerializeField]
    private string[] BaseMessages = { "PuzzleBox: ", "Dragon: " };

    private void Start()
    {
        Creator.OnMarkerPieceWasCreated += OnMarkerPieceWasCreated;
        POITypeInfo info = Creator.GetTypeInfos()[TypeIDToReactTo];
        OnMarkerPieceWasCreated(TypeIDToReactTo, 0, info.NumberOfPieces);
    }

    public void OnMarkerPieceWasCreated(int ID, uint currAmount, uint MaxAmount)
    {
        if(TypeIDToReactTo == ID)
        {
            ButtonHeader.text = BaseMessages[ID] + currAmount + "/" + MaxAmount;
            if (currAmount == MaxAmount)
                GetComponent<Button>().interactable = false;
        }
            
    }
}
