using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickleSwipeLeft : MonoBehaviour {

	private float minSwipeDistanceX2 = 500.0f;

	private float startPos2;
	private float startTime2;
	private float endPos2;
	private float endTime2;
	private float swipedDistanceX2;
	private float speedOfSwipe2 ;
	private float minSpeedX2 = 400.0f;
	private float maxSpeedX2 = 800.0f;



	// Update is called once per frame
	void Update ()
	{
		TickledSwipeLeft ();

	}

	public bool TickledSwipeLeft()
	{
		TickleManager.done = false;

		Touch[] myTouches = Input.touches;

		if (Input.touchCount == 2) {


			for (int i = 0; i < Input.touchCount; i++) { 
				endPos2 = 0;
				endTime2 = 0;
				swipedDistanceX2 = 0;


				if (myTouches [i].phase == TouchPhase.Began) {
					startPos2 = myTouches [i].position.x;
					startTime2 = Time.time;

				}

				if (myTouches [i].phase == TouchPhase.Ended) {
					endPos2 = myTouches [i].position.x;
					endTime2 = Time.time - startTime2;

				}
				if (endPos2 != 0) {
					swipedDistanceX2 = Mathf.Abs (endPos2 - startPos2);
				}
				speedOfSwipe2 = swipedDistanceX2 / endTime2;






				if (swipedDistanceX2 > minSwipeDistanceX2 && speedOfSwipe2 > minSpeedX2 && speedOfSwipe2 < maxSpeedX2 && Mathf.Sign (endPos2 - startPos2) == -1) {

					Debug.Log ("The speed in left direction is " + speedOfSwipe2);
					TickleManager.done = true;
					return TickleManager.done;
				} else if ( speedOfSwipe2 > maxSpeedX2 && swipedDistanceX2 > minSwipeDistanceX2)
				{

					Debug.Log ("Be Gentle");
				} else if(speedOfSwipe2 < minSpeedX2 && swipedDistanceX2 > minSwipeDistanceX2)
				{
					Debug.Log("Didnt even feel that , Come on !!");
				}



			}

		}
		return TickleManager.done;
	}

}
