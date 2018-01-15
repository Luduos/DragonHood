using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickleCrazy : MonoBehaviour {

	private float minSwipeDistanceY3 = 20000.0f;

	private float startPos3;
	private float startTime3;
	private float endPos3;
	private float endTime3;
	private float swipedDistanceY3;
	private float speedOfSwipe3 ;
	private float minSpeedY3= 20000.0f;


	// Update is called once per frame
	void Update ()
	{
		TickledCrazy ();


	}


	public bool TickledCrazy()
	{
		TickleManager.done = false;
		Touch[] myTouches = Input.touches;

		if (Input.touchCount == 2) {


			for (int i = 0; i < Input.touchCount; i++) { 
				endPos3 = 0;
				endTime3 = 0;



				if (myTouches [i].phase == TouchPhase.Began) {
					startPos3 = myTouches [i].position.x;
					startTime3 = Time.time;

				}

				if (myTouches [i].phase == TouchPhase.Moved) 
				{
					swipedDistanceY3 += Mathf.Abs(myTouches [i].deltaPosition.y);
					//Debug.Log ("Swiping... " + swipedDistanceY);
				}


				if (myTouches [i].phase == TouchPhase.Ended) {
					endPos3= myTouches [i].position.x;
					endTime3 = Time.time - startTime3;

				}


				speedOfSwipe3 = swipedDistanceY3 / endTime3;






				if (swipedDistanceY3 > minSwipeDistanceY3 && speedOfSwipe3 > minSpeedY3) {

					Debug.Log ("Muhuhuuhuhahahahhaha !!!! " );
					TickleManager.done = true;
					return TickleManager.done;
				} 



			}

		}
		return TickleManager.done;
	}
}
