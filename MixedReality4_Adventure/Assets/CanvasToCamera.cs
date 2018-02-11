using System;
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
		try{
		cameraToLookAt = GameObject.Find ("Camera1(Clone)");
		}
		catch(Exception e) {
			Debug.LogException (e, this);
		}
		try{
			this.transform.LookAt (cameraToLookAt.transform);}
		catch(Exception e) {
			Debug.LogException (e, this);
		}
	}
}
