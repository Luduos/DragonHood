using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// @author: David Liebemann
/// </summary>
public class Toggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
	private CreatorLogic CreatorObject = null;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Button Down");
        CreatorObject.IsZooming = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up");
        CreatorObject.IsZooming = false;
    }
}
