using UnityEngine;
using UnityEngine.SceneManagement;


public class SwitchScene : MonoBehaviour {
    public void OnSwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
