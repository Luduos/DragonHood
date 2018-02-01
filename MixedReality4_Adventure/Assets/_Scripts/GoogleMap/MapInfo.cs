using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class MapInfo : MonoBehaviour {
    [SerializeField]
    private GoogleMap MapScript;

    [SerializeField]
    public Text DebugText;

    public static MapInfo instance = null;

    private CreatorLogic CreatorObject;

    MapInfo()
    {
        if (instance != null)
        {
            Debug.Log("There can't be multiple MapInfo objects, destroying this object");
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        
    }

    private void Start()
    {
        if(null == CreatorObject)
            CreatorObject = FindObjectOfType<CreatorLogic>();
        if(null != GPS.Instance)
            GPS.Instance.OnInitialized += RefreshMapCenter;

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
        MapScript.centerLocation.longitude = CreatorObject.GetGPSPosition.x;
        MapScript.centerLocation.latitude = CreatorObject.GetGPSPosition.y;
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
        //UpdatePositions();
    }

    public void UpdatePositions()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer && LocationServiceStatus.Running == Input.location.status)
        {
            CreatorObject.SetGPSPosition(new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude));
            
            DebugText.text = "lon: " + GPS.Instance.lon + "lat: " + GPS.Instance.lat;
        }
        else
        {
            CreatorObject.SetGPSPosition(CreatorObject.GetGPSPosition);
        }
    }

    public float GetZoom()
    {
        return MapScript.zoom;
    }

    public Vector2 GetGPSAsUnityPosition(Vector2 gpsLocation)
    {
        double posX = MercatorProjection.lonToX(gpsLocation.x);
        double posY = MercatorProjection.latToY(gpsLocation.y);

        Vector2 mapCenter = GetUnityMapCenter();

        posX -= mapCenter.x;
        posX /= GetZoom();
        posY -= mapCenter.y;
        posY /= GetZoom();

        return new Vector2((float)posY, (float)posX);
    }

}