using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// @author: David Liebemann
/// </summary>
public class CreatorLogic : MonoBehaviour {

    /**  Inspector variables   **/
    [Header("Desktop Movement variables")]
    [SerializeField]
    private POITypeInfo[] TypeInfos = null;

    public POITypeInfo[] GetTypeInfos() { return TypeInfos; }

    [Header("Debug")]
    [SerializeField]
    private Text DebugText = null;
    [SerializeField]
    private Vector2 GPSPos;


    [Header("Desktop Movement variables")]
    [SerializeField]
    private float RotationSpeed = 5.0f;
    [SerializeField]
    private float ZoomSpeed = 1.0f;

    [Header("Prefabs and access")]
    [SerializeField]
    private GameObject CreatorModel = null;
    [SerializeField]
    private SpriteRenderer CreatorSprite = null;
    [SerializeField]
    private POI POIPrefab = null;
    [SerializeField]
    private POIPointer PointerPrefab = null;
    [SerializeField]
    private Button SaveButton = null;
    [SerializeField]
    private MapInfo MapPlane = null;

    /** Other class members **/
    private List<POIPointer> pointers = new List<POIPointer>();
    private List<POI> pointsOfInterest = new List<POI>();
    public List<POI> GetPointsOfInterest() { return pointsOfInterest; }

    // invokes with the ID of the POI type, current amount of pieces and max amount of pieces
    public UnityAction<int, uint, uint> OnMarkerPieceWasCreated; 

    private Camera MainCamera;

    public Vector2 GPSPosition { get { return GPSPos; } set { GPSPos = value; } }
    public bool IsVotingTime { get; set; }
    public bool IsZooming { get; set; }

    private void Start()
    {
        MainCamera = Camera.main;
        this.transform.localPosition = MapInfo.instance.GetGPSAsUnityPosition(GPSPos) ;
        this.transform.localPosition += new Vector3(0.0f, 0.0f, -0.1f);

        Input.gyro.updateInterval = 0.03f;
        Input.gyro.enabled = true;
        if(null != SaveButton)
            SaveButton.interactable = false;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if(horizontalInput != 0)
        {
            CreatorModel.transform.localRotation = CreatorModel.transform.localRotation * Quaternion.AngleAxis(Time.deltaTime * RotationSpeed * horizontalInput, -Vector3.forward);
            SetLookAt(CreatorModel.transform.up);
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            CreatorModel.transform.localRotation = new Quaternion(0.0f, 0.0f, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
            SetLookAt(CreatorModel.transform.up);

        }
        float verticalInput = Input.GetAxis("Vertical");
        if(verticalInput != 0)
        {
            GPSPos += new Vector2(Time.deltaTime * 0.001f * verticalInput, 0.0f);
            SetGPSPosition(GPSPos);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            GPSPos += new Vector2(0.0f, -Time.deltaTime * 0.001f);
            SetGPSPosition(GPSPos);
        }
        if (Input.GetKey(KeyCode.E))
        {
            GPSPos += new Vector2(0.0f, Time.deltaTime * 0.001f);
            SetGPSPosition(GPSPos);
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
        GPSPos = gpsPosition;
        Vector2 UnityPosition = MapInfo.instance.GetGPSAsUnityPosition(gpsPosition);
        this.transform.localPosition = UnityPosition;
        this.transform.localPosition += new Vector3(0.0f, 0.0f, -0.1f);
        foreach(POIPointer poi in pointers)
        {
            poi.OnPlayerPositionChanged(UnityPosition);
        }
    }

    public void SetLookAt(Vector3 lookat)
    {
        foreach (POIPointer poi in pointers)
        {
            poi.OnPlayerRotationChanged(lookat);
        }
    }

    public void OnMapInfoLoaded()
    {
        MapInfo.instance.RefreshMapCenter();
    }

    /// <summary>
    /// Sets the amount of pieces for each
    /// </summary>
    /// <param name="amounts"></param>
    public void SetAmountOfPieces(List<uint> amounts)
    {
        for(int i = 0; i < TypeInfos.Length; ++i)
        {
            TypeInfos[i].NumberOfPieces = amounts[i];
            if(null != OnMarkerPieceWasCreated)
                OnMarkerPieceWasCreated.Invoke(i, 0, TypeInfos[i].NumberOfPieces);
        }
    }

    /// <summary>
    /// Called during creation mode to set a new POI marker.
    /// </summary>
    /// <param name="PointerTypeID"></param>
    public void OnCreateNewPOI(int PointerTypeID)
    {
        // get info from the info types
        POITypeInfo info = TypeInfos[PointerTypeID];

        uint numPOIOfSameType = 0;
        foreach (POI currPOI in pointsOfInterest)
        {
            if (currPOI.ID == PointerTypeID)
                numPOIOfSameType++;
        }
        if(!(numPOIOfSameType < info.NumberOfPieces))
        {
            Debug.Log("Tried to create too many pois of the same type");
            return;
        }
        numPOIOfSameType++;


        // instantiate the actual poi object in unity space
        POI poiObject = Instantiate(POIPrefab);
        poiObject.transform.SetParent(MapPlane.transform, true);
        poiObject.SetID(info.ID);
        poiObject.SetGPSPosition(GPSPos);
        poiObject.SetName(info.Name + "(" + numPOIOfSameType + ")");
        pointsOfInterest.Add(poiObject);
  
        if(null != OnMarkerPieceWasCreated)
        {
            OnMarkerPieceWasCreated.Invoke(PointerTypeID, numPOIOfSameType, info.NumberOfPieces);
        }

        // create a pointer pointing toward the poi object
        POIPointer pointer = Instantiate(PointerPrefab);
        pointer.transform.SetParent(this.transform, false);

        pointer.POIName = poiObject.GetName();
        pointer.UnityTarget = poiObject.transform.localPosition;
        pointer.ID = poiObject.ID;
        pointer.poiObject = poiObject;
        Vector2 CurrentPos = MapInfo.instance.GetGPSAsUnityPosition(GPSPos);
        pointer.OnPlayerPositionChanged(CurrentPos);
        pointers.Add(pointer);

        CheckSaveButtonActivation();
    }

    private void CheckSaveButtonActivation()
    {
        // Check if we have created all POIs and activate Save Button, if that is the case
        bool allMarkersSet = true;
        uint[] currentPOIAmounts = new uint[TypeInfos.Length];
        foreach (POI currPOI in pointsOfInterest)
        {
            currentPOIAmounts[currPOI.ID] = currentPOIAmounts[currPOI.ID] + 1;
        }
        for(int i = 0; i < TypeInfos.Length; ++i)
        {
            allMarkersSet = allMarkersSet && (currentPOIAmounts[i] == TypeInfos[i].NumberOfPieces);
        }

        if (null != SaveButton && allMarkersSet)
        {
            SaveButton.interactable = true;
        }
    }

    public void LoadPOIsFromSaveFile(List<POISaveInfo> poiInfos)
    {
        foreach(POI poi in pointsOfInterest)
        {
            Destroy(poi.gameObject);
        }
        pointsOfInterest.Clear();
        foreach (POIPointer pointer in pointers)
        {
            Destroy(pointer.gameObject);
        }
        pointers.Clear();

        foreach (POISaveInfo info in poiInfos)
        {
            // instantiate the actual poi object in unity space
            POI poiObject = Instantiate(POIPrefab);
            poiObject.transform.SetParent(MapPlane.transform, true);
            poiObject.SetID(info.ID);
            poiObject.SetGPSPosition(new Vector2(info.XGPSPos, info.YGPSPos));
            poiObject.SetName(info.Name);
            pointsOfInterest.Add(poiObject);


            // create a pointer pointing toward the poi object
            POIPointer pointer = Instantiate(PointerPrefab);
            pointer.transform.SetParent(this.transform, false);

            pointer.POIName = poiObject.GetName();
            pointer.UnityTarget = poiObject.transform.localPosition;
            pointer.ID = poiObject.ID;
            pointer.poiObject = poiObject;
            Vector2 CurrentPos = MapInfo.instance.GetGPSAsUnityPosition(GPSPos);
            pointer.OnPlayerPositionChanged(CurrentPos);
            pointers.Add(pointer);
        }
    }

}

[System.Serializable]
public struct POITypeInfo
{
    /// ID of PuzzleBox: 0, ID of Dragon: 1. I know that this is bad. Need more than two weeks of dev time to do it better. ///
    public int ID;
    public string Name;
    [HideInInspector]
    public uint NumberOfPieces;
}
