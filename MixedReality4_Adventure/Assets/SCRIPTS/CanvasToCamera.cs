using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasToCamera : MonoBehaviour {
	public GameObject cameraToLookAt;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		cameraToLookAt = GameObject.Find ("Camera1(Clone)");


		this.transform.LookAt (cameraToLookAt.transform);
	}
}
