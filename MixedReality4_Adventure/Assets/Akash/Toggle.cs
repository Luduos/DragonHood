using UnityEngine;
using UnityEngine.EventSystems;

public class Toggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField]
	private CreatorLogic CreatorObject;

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
