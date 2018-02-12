using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasToCamera : MonoBehaviour {
    [SerializeField]
    private Camera cameraToLookAt = null;

	// Use this for initialization
	void Start () {
        if (!cameraToLookAt)
        {
            GameObject arCam = GameObject.Find("ARCamera");
            if (arCam)
                cameraToLookAt = arCam.GetComponent<Camera>();
        }
    }
	


	// Update is called once per frame
	void Update () {
        if(cameraToLookAt)
		    this.transform.LookAt (cameraToLookAt.transform);
	}
}
