using UnityEngine;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// @author: David Liebemann
/// </summary>
[RequireComponent(typeof(Renderer))]
public class PuzzleBoxFace : MonoBehaviour {

    public UnityAction<PuzzleBoxFace> OnCorrectTouchDetected;

    [SerializeField]
    private int id = 0;

    [SerializeField]
    private int touchCount = 0;

    //public int TouchCount { get { return touchCount; } }

    public int ID { get { return id; } }

    [SerializeField]
    private Texture2D puzzleMasterIcon = null;
    [SerializeField]
    private Texture2D puzzleMasterNormal = null;
    [SerializeField]
    private Texture2D puzzleMasterHeight = null;

    [SerializeField]
    private Texture2D image = null;
    [SerializeField]
    private Texture2D normalMap = null;
    [SerializeField]
    private Texture2D heightMap = null;

    [SerializeField]
    private Light Highlight = null;

    private Material faceMaterial;
    public bool WasCorrectlyTouched { get; private set; }
    public bool WasFinalized { get; private set; }

    private Color CorrectColor = Color.green;
    private Color WrongColor = Color.red;
    private Color WaitingColor = Color.yellow;
    private Color NeutralColor = Color.white;


	// Use this for initialization
	void Start () {
        faceMaterial = GetComponent<Renderer>().material;
        WasCorrectlyTouched = false;
        WasFinalized = false;
        Highlight.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (Application.isEditor && Input.anyKey)
        {
            OnCorrectTouchCount();
        }else if (!WasCorrectlyTouched)
        {
            StartCoroutine(TollerantTouchCheck());
        } 
    }

    private IEnumerator TollerantTouchCheck()
    {
        float elapsedTime = 0.0f;
        float tollerance = 0.5f;
        while(elapsedTime < tollerance)
        {
            if (Input.touchCount == touchCount && !WasCorrectlyTouched)
            {
                OnCorrectTouchCount();
                yield break; 
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (Input.touchCount == touchCount && !WasCorrectlyTouched)
        {
            OnCorrectTouchCount();
        }
        else if (Input.touchCount != touchCount && !WasCorrectlyTouched)
        {
            StartCoroutine(OnWrongTouchCount());
        }
    }

    private void OnCorrectTouchCount()
    {
        Debug.Log("Correct: " + touchCount);
        if (null != OnCorrectTouchDetected)
        {
            OnCorrectTouchDetected.Invoke(this);
        }  
    }

    public IEnumerator OnWrongTouchCount()
    {
        bool highlightWasActive = Highlight.gameObject.activeSelf;
        Debug.Log("Wrong: " + touchCount);
        WasCorrectlyTouched = false;
        WasFinalized = false;
        faceMaterial.color = WrongColor;
        Handheld.Vibrate();

        float elapsedTime = 0.0f;
        float allertedTime = 0.5f;
        while (elapsedTime < allertedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!WasCorrectlyTouched)
            ResetTouchable();
        Highlight.gameObject.SetActive(highlightWasActive);
        yield return null;
    }

    public IEnumerator OnNetworkRegisteredWrongFace()
    {
        Handheld.Vibrate();
        faceMaterial.color = WrongColor;
        WasCorrectlyTouched = false;
        WasFinalized = false;

        float elapsedTime = 0.0f;
        float allertedTime = 0.5f;
        while (elapsedTime < allertedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!WasCorrectlyTouched)
            ResetTouchable();
        yield return null;
    }

    public void ResetTouchable()
    {
        WasCorrectlyTouched = false;
        WasFinalized = false;
        faceMaterial.color = NeutralColor;
        Highlight.gameObject.SetActive(false);
    }

    public void OnRegisterNetworkCorrectTouch()
    {
        Debug.Log("Network: " + touchCount);
        if (!WasCorrectlyTouched)
        {
            WasCorrectlyTouched = true;
            faceMaterial.color = WaitingColor;
        }
        Highlight.gameObject.SetActive(false);
    }

    public void OnFinalizeCorrectTouch()
    {
        Debug.Log("Finalize: " + touchCount);
        WasCorrectlyTouched = true;
        WasFinalized = true;
        faceMaterial.color = CorrectColor;
        Highlight.gameObject.SetActive(false);
    }

    public void SetNumberVisibility(bool visible)
    {
        if (null == faceMaterial)
            faceMaterial = GetComponent<Renderer>().material;

        if (visible)
        {
            faceMaterial.mainTexture = image;
            faceMaterial.SetTexture("_BumpMap", normalMap);
            faceMaterial.SetTexture("_ParallaxMap", heightMap);
        }
        else
        {
            faceMaterial.mainTexture = puzzleMasterIcon;
            faceMaterial.SetTexture("_BumpMap", puzzleMasterNormal);
            faceMaterial.SetTexture("_ParallaxMap", puzzleMasterHeight);
        }
    }

    public void OnGiveHint()
    {
        Highlight.gameObject.SetActive(true);
    }
}
