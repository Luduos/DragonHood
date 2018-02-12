using UnityEngine;
/// <summary>
/// @author: David Liebemann
/// </summary>
public class Android : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        if (Application.isMobilePlatform && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
