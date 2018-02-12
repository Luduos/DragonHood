using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// @author: David Liebemann
/// </summary>
public class SwitchScene : MonoBehaviour {
    public void OnSwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
