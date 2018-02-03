using UnityEngine;

public class POI : MonoBehaviour {
    public int ID { get; set; }

    [SerializeField]
    private string Name;
    [SerializeField]
    private Vector2 GPSPosition;

    [SerializeField]
    private TextMesh NameText = null;

    private Camera MainCamera;

    public bool ShouldUpdate { get; set; }

    private void Start()
    {
        MainCamera = Camera.main;
        this.transform.localScale = this.transform.lossyScale;
        this.transform.localRotation = Quaternion.identity;
        ShouldUpdate = true;
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
        if (ShouldUpdate)
        {
            Vector3 nameTextForward = NameText.transform.up;
            Vector3 mainCamForward = (Vector3.Dot(MainCamera.transform.up, this.transform.up) * this.transform.up) ;
            mainCamForward = mainCamForward.normalized;
            float angleA = Mathf.Atan2(nameTextForward.x, nameTextForward.y) * Mathf.Rad2Deg;
            float angleB = Mathf.Atan2(mainCamForward.x, mainCamForward.y) * Mathf.Rad2Deg;

            // get the signed difference in these angles
            float angleDiff = Mathf.DeltaAngle(angleA, angleB);

            NameText.transform.RotateAround(this.transform.position, -transform.forward, angleDiff);
            this.transform.localPosition = MapInfo.instance.GetGPSAsUnityPosition(GPSPosition);
        }      
    }
}
