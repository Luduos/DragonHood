using UnityEngine;

public class Android : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        if (Application.isMobilePlatform && Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
