using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
/// <summary>
/// @author: David Liebemann
/// </summary>
public class SaveAdventure : MonoBehaviour
{
    [SerializeField]
    private Text DebugText = null;

    [SerializeField]
    private CreatorLogic Creator = null;

    public void OnSaveAdventure(string saveName) {

        if(saveName.Length < 1)
        {
            StartCoroutine(TimedMessage(2.0f, "Please use a non-empty file name."));
            return;
        }

        List<POI> pois = Creator.GetPointsOfInterest();
        List<POISaveInfo> adventureInfo = new List<POISaveInfo>();
        foreach (POI currPOI in pois)
        {
            POISaveInfo saveInfo = new POISaveInfo();
            saveInfo.ID = currPOI.ID;
            saveInfo.Name = currPOI.GetName();
            Vector2 gpsPos = currPOI.GetGPSPosition();
            saveInfo.XGPSPos = gpsPos.x;
            saveInfo.YGPSPos = gpsPos.y;
            adventureInfo.Add(saveInfo);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + saveName + ".adv");
        bf.Serialize(file, adventureInfo);
        file.Close();


        StartCoroutine(TimedMessage(2.0f, "Saved adventure: " + saveName));
        Debug.Log("Saved file under: " + Application.persistentDataPath + "/" + saveName + ".adv");
    }

    public List<string> GetAdventureList()
    {
        List<string> result = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/");
        FileInfo[] info = dir.GetFiles("*.adv");
        foreach (FileInfo f in info)
        {
            Debug.Log("Found file: " + f.Name);
            string fileName = f.Name;
            fileName = fileName.Substring(0, fileName.Length - 4);
            result.Add(fileName);
        }
        return result;
    }

    public List<POISaveInfo> LoadAdventure(string fileName)
    {
        List<POISaveInfo> result = new List<POISaveInfo>();

        string filePath = Application.persistentDataPath + "/" + fileName + ".adv";
        if (File.Exists(filePath))
        {

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            result = (List<POISaveInfo>)bf.Deserialize(file);
            file.Close();

        }
        return result;
    }

    private IEnumerator TimedMessage(float duration, string message)
    {
        float deltaTime = 0.0f;
        DebugText.text = message;
        while(deltaTime < duration)
        {
            deltaTime += Time.deltaTime;
            yield return null;
        }
        DebugText.text = "";
    }

    
}
[System.Serializable]
public class POISaveInfo
{
    public int ID;
    public string Name;
    public float XGPSPos;
    public float YGPSPos;
}