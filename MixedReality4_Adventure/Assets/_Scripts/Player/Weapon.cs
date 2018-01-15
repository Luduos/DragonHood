using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Weapon : MonoBehaviour {

    [SerializeField]
    private Text InfoDisplay;

    [SerializeField]
    private Image WeaponIconDisplay;

    [SerializeField]
    private Sprite WeaponIcon;

    [SerializeField]
    private PlayerLogic Player;

    [SerializeField]
    private GameObject Mesh;

    private void OnMouseDown()
    {
        if(PlayerClassType.Fighter == Player.ClassType)
        {
            Player.HasFeather = true;
            WeaponIconDisplay.color = Color.white;
            WeaponIconDisplay.sprite = WeaponIcon;
            this.gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(WrongClassInfo());
            Mesh.gameObject.SetActive(false);
        }
    }

    public IEnumerator WrongClassInfo()
    {
        float elapsedTime = 0.0f;
        float allertedTime = 2.0f;

        InfoDisplay.text = "For Fighters only!";

        while (elapsedTime < allertedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        InfoDisplay.text = "";
        Mesh.gameObject.SetActive(true);
        //this.gameObject.SetActive(false);
        yield return null;
    }
}
