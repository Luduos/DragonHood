using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildingTheCamerasToARCam : MonoBehaviour {
	public GameObject ARCam = null;
    public int avatarIndex = 0;
	// Use this for initialization
	void Start () {
		ARCam = GameObject.Find ("ARCamera");
        //Dragon = GameObject.Find("ImageTarget");
        MyNetworkManager myNetworkManager = (MyNetworkManager)MyNetworkManager.singleton;
        if (myNetworkManager.avatarIndex == avatarIndex)
        {
            this.transform.parent = ARCam.transform;
        }
        /*//this.transform.position = ARCam.transform.position - Dragon.transform.position;
		this.transform.position = new Vector3 (0, 0, 20f);
		StartCoroutine(FaceDragon());
*/
        this.transform.localPosition= Vector3.zero;
		this.transform.localRotation= Quaternion.identity;

	}

    /*public IEnumerator FaceDragon()
	{
		this.transform.LookAt (Dragon.transform);
		yield return new WaitForSeconds (4);
	}*/

}
