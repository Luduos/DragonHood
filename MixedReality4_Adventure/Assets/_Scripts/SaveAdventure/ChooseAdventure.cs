using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChooseAdventure : MonoBehaviour {

    [SerializeField]
    private Button AdventureButtonPrefab;

    [SerializeField]
    private RectTransform ScrollViewContent;

    [SerializeField]
    private SaveAdventure SaveAdventureObj;

    [SerializeField]
    private CreatorLogic Creator;

    public void OnShowAdventures()
    {
        List<string> FileNames = SaveAdventureObj.GetAdventureList();
        int contentChildCount = ScrollViewContent.childCount;
        for (int i = 0; i < contentChildCount; ++i)
        {
            Destroy(ScrollViewContent.GetChild(i).gameObject);
        }
        foreach(string fileName in FileNames)
        {
            Button button = Instantiate(AdventureButtonPrefab, ScrollViewContent, false);
            button.GetComponentInChildren<Text>().text = fileName;
            button.onClick.AddListener(()=>OnButtonClick(fileName));
        }
    }

    private void OnButtonClick(string fileName)
    {
        Creator.CreatePOIs(SaveAdventureObj.LoadAdventure(fileName));
        this.gameObject.SetActive(false);
    }
}
