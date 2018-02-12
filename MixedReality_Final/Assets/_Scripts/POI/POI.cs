using UnityEngine;
/// <summary>
/// @author: David Liebemann
/// 
/// Point of Interest
/// </summary>
public class POI : MonoBehaviour {
    public int ID { get; private set; }

    [SerializeField]
    private string Name;
    [SerializeField]
    private Vector2 GPSPosition;

    [SerializeField]
    private GameObject TextSubObject = null;
    [SerializeField]
    private TextMesh NameText = null;

    [SerializeField]
    private GameObject MiniBoxPrefab = null;

    [SerializeField]
    private GameObject MiniDragonPrefab = null;

    private Camera MainCamera;

    public bool ShouldUpdate { get; set; }

    private GameObject SubMesh = null;

    private void Start()
    {
        MainCamera = Camera.main;
        this.transform.localScale = this.transform.lossyScale;
        this.transform.localRotation = Quaternion.identity;
        ShouldUpdate = true;
    }

    public void SetID(int newID)
    {
        ID = newID;
        if (null != SubMesh)
            return;

        if(0 ==newID)
        {
            SubMesh = Instantiate(MiniBoxPrefab, this.transform, false);
            NameText.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);
        }
        else
        {
            SubMesh = Instantiate(MiniDragonPrefab, this.transform, false);
            NameText.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);
            //SubMesh.transform.Rotate(SubMesh.transform.right, Random.Range(0.0f, 90.0f));
        }
        SubMesh.transform.Rotate(SubMesh.transform.forward, Random.Range(0.0f, 360.0f));
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
            /*
            Vector3 nameTextForward = NameText.transform.up;
            Vector3 mainCamForward = (Vector3.Dot(MainCamera.transform.up, this.transform.up) * this.transform.up) ;
            mainCamForward = mainCamForward.normalized;
            float angleA = Mathf.Atan2(nameTextForward.x, nameTextForward.y) * Mathf.Rad2Deg;
            float angleB = Mathf.Atan2(mainCamForward.x, mainCamForward.y) * Mathf.Rad2Deg;

            // get the signed difference in these angles
            float angleDiff = Mathf.DeltaAngle(angleA, angleB);

            NameText.transform.RotateAround(this.transform.position, -transform.forward, angleDiff);
            */

            TextSubObject.transform.localRotation = new Quaternion(0.0f, 0.0f, -MainCamera.transform.rotation.z, -MainCamera.transform.rotation.w);

            this.transform.localPosition = MapInfo.instance.GetGPSAsUnityPosition(GPSPosition);
        }      
    }
}
