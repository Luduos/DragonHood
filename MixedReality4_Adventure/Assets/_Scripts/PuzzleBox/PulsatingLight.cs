using UnityEngine;

[RequireComponent(typeof(Light))]
public class PulsatingLight : MonoBehaviour {
    [SerializeField]
    private float baseIntensity = 20.0f;

    [SerializeField]
    private float pulsatingFrequency = 4.0f;

    [SerializeField]
    private float amplitude = 0.75f;

    [SerializeField]
    private Color lightColor = new Color(1.0f,0.8f, 0.3f);

    private Light pLight = null;

	// Use this for initialization
	void Start () {
        pLight = GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        pLight.intensity = baseIntensity + baseIntensity* (Mathf.Sin(Time.time * pulsatingFrequency) * amplitude);
    }
}
