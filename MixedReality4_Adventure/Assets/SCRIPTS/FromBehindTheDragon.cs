using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromBehindTheDragon : MonoBehaviour {

	public GameObject ARcam;
	public GameObject Dragon;
	//Dont forget to change these to private
	public Vector3 ARCamDirection;
	public Vector3 DragonDirection;
	public static bool youAreBehind;

	void Update()
	{   ARcam = GameObject.Find ("Camera1(Clone)");
		youAreBehind = false;
		ARCamDirection = ARcam.transform.forward;
		DragonDirection = Dragon.transform.forward;

		if (Vector3.Dot (ARCamDirection, DragonDirection) > 0) {
			youAreBehind = true;
			//Debug.Log ("you are behind !!");
		} else {
			youAreBehind = false;
			//Debug.Log("you are in front !!");
		}
	}
}
