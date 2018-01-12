using UnityEngine;
using UnityEngine.Events;
using System.Collections;


[RequireComponent(typeof(Renderer))]
public class PuzzleBoxFace : MonoBehaviour {

    [SerializeField]
    private int touchCount;

    public int TouchCount { get { return touchCount; } }

    [SerializeField]
    private Texture2D image;

    private Material faceMaterial;
    public bool WasCorrectlyTouched { get; private set; }

    private Color CorrectColor = Color.green;
    private Color WrongColor = Color.red;
    private Color WaitingColor = Color.yellow;
    private Color NeutralColor = Color.white;

    public UnityAction OnCorrectTouchDetected;

	// Use this for initialization
	void Start () {
        faceMaterial = GetComponent<Renderer>().material;
        WasCorrectlyTouched = false;
        if (null != image)
            faceMaterial.mainTexture = image;
	}

    private void OnMouseDown()
    {
        Debug.Log("Test " + touchCount);
        Debug.Log("Number of touches: " + Input.touchCount);
        if(Input.touchCount == touchCount && !WasCorrectlyTouched)
        {
            OnCorrectTouchCount();
        }else if(Input.touchCount != touchCount && !WasCorrectlyTouched)
        {
            StartCoroutine(OnWrongTouchCount());
        }

        if (Application.isEditor && Input.anyKey)
        {
            OnCorrectTouchCount();
        }
    }

    private void OnCorrectTouchCount()
    {
        WasCorrectlyTouched = true;
        faceMaterial.color = CorrectColor;

        if (null != OnCorrectTouchDetected)
        {
            OnCorrectTouchDetected.Invoke();
        }  
    }

    public IEnumerator OnWrongTouchCount()
    {
        float elapsedTime = 0.0f;
        float allertedTime = 0.5f;
        faceMaterial.color = WrongColor;
        while (elapsedTime < allertedTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (!WasCorrectlyTouched)
            faceMaterial.color = NeutralColor;

        yield return null;
    }

    public void ResetTouchable()
    {
        WasCorrectlyTouched = false;
        faceMaterial.color = Color.white;
    }
}
