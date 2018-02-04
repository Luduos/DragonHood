using UnityEngine;
/// <summary>
/// @author: David Liebemann
/// </summary>
public class ActivationToggle : MonoBehaviour {

	public void OnToggleActivate(GameObject ToToggle)
    {
        ToToggle.SetActive(!ToToggle.activeSelf);
    }
}
