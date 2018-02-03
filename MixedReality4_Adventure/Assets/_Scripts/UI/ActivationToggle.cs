using UnityEngine;

public class ActivationToggle : MonoBehaviour {

	public void OnToggleActivate(GameObject ToToggle)
    {
        ToToggle.SetActive(!ToToggle.activeSelf);
    }
}
