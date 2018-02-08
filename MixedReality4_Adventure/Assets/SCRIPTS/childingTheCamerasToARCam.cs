using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildingTheCamerasToARCam : MonoBehaviour {
	private GameObject ARCam;
	// Use this for initialization
	void Start () {
        ARCam = GameObject.Find("ARCamera");
		this.transform.parent = ARCam.transform;
        this.transform.localPosition = Vector3.zero;
        this.transform.rotation = Quaternion.identity;

    }
	

}
