using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasToCamera : MonoBehaviour {
	public Camera cameraToLookAt;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		


		this.transform.LookAt (cameraToLookAt.transform);
	}
}
