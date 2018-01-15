using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDragonGamePlay : MonoBehaviour {
	private TickleManager startScript;
	// Use this for initialization
	void Start () {
		startScript = this.GetComponent<TickleManager> ();

	}


	
	public void StartDragonGamePlayFunction()
	{
		startScript.enabled = true;
	}
}
