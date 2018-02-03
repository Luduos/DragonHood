using UnityEngine;

public class POI : MonoBehaviour {
    public int ID { get; set; }

    [SerializeField]
    private string Name;
    [SerializeField]
    private Vector2 GPSPosition;

    [SerializeField]
    private TextMesh NameText;

    private Camera MainCamera;

    private void Start()
    {
        MainCamera = Camera.main;
    }

    public string GetName() { return Name; }

    public void SetName(string name)
    {
        this.Name = name;
        NameText.text = name;
    }

    public Vector2 GetGPSPosition()
    {
        return GPSPosition;
    }

    public void SetGPSPosition(Vector2 gpsPosition)
    {
        GPSPosition = gpsPosition;
        this.transform.localPosition = MapInfo.instance.GetGPSAsUnityPosition(gpsPosition);
    }

    private void Update()
    {
        Vector3 nameTextForward = NameText.transform.up;
        Vector3 mainCamForward = MainCamera.transform.up;
        float angleA = Mathf.Atan2(nameTextForward.x, nameTextForward.y) * Mathf.Rad2Deg;
        float angleB = Mathf.Atan2(mainCamForward.x, mainCamForward.y) * Mathf.Rad2Deg;

        // get the signed difference in these angles
        float angleDiff = Mathf.DeltaAngle(angleA, angleB);

        NameText.transform.RotateAround(this.transform.localPosition, -Vector3.forward, angleDiff);
        this.transform.localPosition = MapInfo.instance.GetGPSAsUnityPosition(GPSPosition);
    }
}
