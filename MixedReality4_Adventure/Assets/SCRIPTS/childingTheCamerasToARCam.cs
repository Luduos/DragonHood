using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class childingTheCamerasToARCam : MonoBehaviour {
	private GameObject ARCam;
	// Use this for initialization
	void Start () {
		ARCam = GameObject.Find ("ARCamera");
		this.transform.parent = ARCam.transform;
	}
	

}
