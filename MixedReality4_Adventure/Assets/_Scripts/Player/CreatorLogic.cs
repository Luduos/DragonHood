using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatorLogic : MonoBehaviour {

    [SerializeField]
    private float RotationSpeed = 5.0f;
    [SerializeField]
    private float RotationThreshold = 0.5f;
    [SerializeField]
    private Text DebugText;
    [SerializeField]
    private Vector2 GPSPosition;

    [SerializeField]
    private float ZoomSpeed = 1.0f;

    public Vector2 GetGPSPosition { get { return GPSPosition; }  set{ GPSPosition = value; } }

    [SerializeField]
    private GameObject CreatorModel;

    [SerializeField]
    private SpriteRenderer CreatorSprite;

    [SerializeField]
    private POIPointer PointerPrefab;
    private List<POIPointer> pointers = new List<POIPointer>();

    public bool IsVotingTime { get; set; }
    public bool IsZooming { get; set; }

    private Camera MainCamera;

    private void Start()
    {
        MainCamera = Camera.main;
        this.transform.position = MapInfo.instance.GetGPSAsUnityPosition(GPSPosition) ;
        this.transform.position += new Vector3(0.0f, 0.0f, -0.1f);

        Input.gyro.updateInterval = 0.03f;
        Input.gyro.enabled = true; 
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if(horizontalInput != 0)
        {
            CreatorModel.transform.rotation = CreatorModel.transform.rotation * Quaternion.AngleAxis(Time.deltaTime * RotationSpeed * horizontalInput, -Vector3.forward);
            SetLookAt(CreatorModel.transform.up);
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            CreatorModel.transform.rotation = new Quaternion(0.0f, 0.0f, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
            SetLookAt(CreatorModel.transform.up);

        }

        

        float verticalInput = Input.GetAxis("Vertical");
        if(verticalInput != 0)
        {
            GPSPosition += new Vector2(Time.deltaTime * 0.001f * verticalInput, 0.0f);
            SetGPSPosition(GPSPosition);
        }
 
        if (IsZooming)
        {
            MainCamera.orthographicSize = MainCamera.orthographicSize - Input.acceleration.y * Time.deltaTime * ZoomSpeed;
        }

        if(IsZooming && Input.GetKey(KeyCode.Q))
        {
            MainCamera.orthographicSize = MainCamera.orthographicSize - 1.0f * Time.deltaTime * ZoomSpeed;
        }
        if (IsZooming && Input.GetKey(KeyCode.E))
        {
            MainCamera.orthographicSize = MainCamera.orthographicSize + 1.0f * Time.deltaTime * ZoomSpeed;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            MapInfo.instance.RefreshMapCenter();
        }
    }



    public void SetGPSPosition(Vector2 gpsPosition)
    {
        GPSPosition = gpsPosition;
        Vector2 UnityPosition = MapInfo.instance.GetGPSAsUnityPosition(gpsPosition);
        this.transform.position = UnityPosition;
        this.transform.position += new Vector3(0.0f, 0.0f, -0.1f);
        foreach(POIPointer poi in pointers)
        {
            poi.OnPlayerPositionChanged(UnityPosition);
        }
    }

    public void SetLookAt(Vector2 lookat)
    {
        foreach (POIPointer poi in pointers)
        {
            poi.OnPlayerRotationChanged(lookat);
        }
    }

    public void OnMapInfoLoaded()
    {
        Vector3 CurrentPos = MapInfo.instance.GetGPSAsUnityPosition(GPSPosition);
        foreach (POIInfo poi in MapInfo.instance.GetPointsOfInterest())
        {
            POIPointer pointer = Instantiate(PointerPrefab);
            pointer.transform.SetParent(this.transform, false);

            pointer.POIName = poi.Name;
            pointer.UnityTarget = poi.UnityPosition;
            pointer.ID = poi.ID;
            pointer.poiObject = poi.poiObject;
            pointer.OnPlayerPositionChanged(CurrentPos);
            
            pointers.Add(pointer);

        }
        MapInfo.instance.RefreshMapCenter();
    }

}
