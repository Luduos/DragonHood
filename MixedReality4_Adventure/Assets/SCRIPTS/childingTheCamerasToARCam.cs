using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class childingTheCamerasToARCam : MonoBehaviour {
	public GameObject ARCam;
	public GameObject Dragon;
	// Use this for initialization
	void Start () {
		ARCam = GameObject.Find ("ARCamera");
		//Dragon = GameObject.Find("ImageTarget");
		this.transform.parent = ARCam.transform;
		/*//this.transform.position = ARCam.transform.position - Dragon.transform.position;
		this.transform.position = new Vector3 (0, 0, 20f);
		StartCoroutine(FaceDragon());
*/
		this.transform.position= ARCam.transform.position;
		this.transform.rotation= ARCam.transform.rotation;

	}

	/*public IEnumerator FaceDragon()
	{
		this.transform.LookAt (Dragon.transform);
		yield return new WaitForSeconds (4);
	}*/

}
