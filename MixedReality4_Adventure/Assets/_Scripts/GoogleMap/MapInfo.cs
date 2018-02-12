using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// @author: David Liebemann
/// </summary>
public class MapInfo : MonoBehaviour {
    [SerializeField]
    private GoogleMap MapScript = null;

    [SerializeField]
    public Text DebugText = null;

    public static MapInfo instance = null;

    [SerializeField]
    private CreatorLogic CreatorObject = null;

    MapInfo()
    {
        instance = this;
    }

    private void Start()
    {

        if (null == CreatorObject)
            CreatorObject = FindObjectOfType<CreatorLogic>();
        if(null != GPS.Instance)
            GPS.Instance.OnInitialized += RefreshMapCenter;

        RefreshMapCenter();
    }

    private void OnEnable()
    {
        UpdatePositions();
        RefreshMapCenter();
    }

    private void Update()
    {
        UpdatePositions();
    }

    public Vector2 GetGPSMapCenter()
    {
        return new Vector2(MapScript.centerLocation.longitude, MapScript.centerLocation.latitude);
    }

    public void RefreshMapCenter()
    {
        MapScript.centerLocation.longitude = CreatorObject.GPSPosition.x;
        MapScript.centerLocation.latitude = CreatorObject.GPSPosition.y;
        MapScript.Refresh();  
        UpdatePositions();
    }

    public Vector2 GetUnityMapCenter()
    {
        Vector2 gpsMapCenter = GetGPSMapCenter();
        double x = MercatorProjection.lonToX(gpsMapCenter.x);
        double y = MercatorProjection.latToY(gpsMapCenter.y);
        return new Vector2((float)x, (float)y);
    }

    public void IncreaseZoom()
    {
        MapScript.zoom = MapScript.zoom + 1;
        UpdatePositions();
    }

    public void UpdatePositions()
    {
        if (LocationServiceStatus.Running == Input.location.status)
        {
            CreatorObject.SetGPSPosition(new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude));
            
            DebugText.text = "lon: " + GPS.Instance.lon + "lat: " + GPS.Instance.lat;
        }
        else
        {
            string status = "Unknown";
            if (LocationServiceStatus.Stopped == Input.location.status)
                status = "Stopped";
            if (LocationServiceStatus.Failed == Input.location.status)
                status = "Failed";
            if (LocationServiceStatus.Initializing == Input.location.status)
                status = "Initializing";
            DebugText.text = "Current status: " + status;

            CreatorObject.SetGPSPosition(CreatorObject.GPSPosition);
        }
    }

    public float GetZoom()
    {
        return MapScript.zoom;
    }

    public Vector2 GetGPSAsUnityPosition(Vector2 gpsLocation)
    {
        /*
        double posX = MercatorProjection.lonToX(gpsLocation.x);
        double posY = MercatorProjection.latToY(gpsLocation.y);

        Vector2 mapCenter = GetUnityMapCenter();

        
        // 100 units is 1 Miles = 1609.34 meters at zoom level 14
        posX -= mapCenter.x;
        posX *= 1.60934f  / 20.0f;
        posY -= mapCenter.y;
        posY *= 1.60934f / 20.0f;
        */

        // 
        double posX = (gpsLocation.x - GetGPSMapCenter().x) * 9027.977761f * 5.0f; 
        double posY = (gpsLocation.y - GetGPSMapCenter().y) * 9027.977761f * 3.25f;
        return new Vector2((float)posY, (float)posX);
    }
}