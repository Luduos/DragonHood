using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickleSwipeRight : MonoBehaviour 
{
	private float minSwipeDistanceX1 = 500.0f;

	private float startPos1;
	private float startTime1;
	private float endPos1;
	private float endTime1;
	private float swipedDistanceX1;
	private float speedOfSwipe1 ;
	private float minSpeedX1 = 400.0f;
	private float maxSpeedX1 = 800.0f;

	// Update is called once per frame
	void Update ()
	{
		TickleSwipedRight ();
		


		}



public bool TickleSwipedRight()
	{
		TickleManager.done = false;
		
		Touch[] myTouches = Input.touches;

		if (Input.touchCount == 2) {


			for (int i = 0; i < Input.touchCount; i++) { 
				endPos1 = 0;
				endTime1 = 0;
				swipedDistanceX1 = 0;


				if (myTouches [i].phase == TouchPhase.Began) {
					startPos1 = myTouches [i].position.x;
					startTime1 = Time.time;

				}

				if (myTouches [i].phase == TouchPhase.Ended) {
					endPos1 = myTouches [i].position.x;
					endTime1 = Time.time - startTime1;

				}
				if (endPos1!=0) 
				{
					swipedDistanceX1 = endPos1 - startPos1;
				}
				speedOfSwipe1 = swipedDistanceX1 / endTime1;






				if (swipedDistanceX1 > minSwipeDistanceX1 && speedOfSwipe1 > minSpeedX1 && speedOfSwipe1 < maxSpeedX1 && Mathf.Sign (endPos1 - startPos1) == 1) {

					Debug.Log ("The speed in the right direction is " + speedOfSwipe1);
					TickleManager.done = true;
					return TickleManager.done;
				} else if ( speedOfSwipe1 > maxSpeedX1 && swipedDistanceX1 > minSwipeDistanceX1)
				{

					Debug.Log ("Be Gentle");
				} else if(speedOfSwipe1 < minSpeedX1 && swipedDistanceX1 > minSwipeDistanceX1)
				{
					Debug.Log("Didnt even feel that , Come on !!");
				}



			}

}
		return TickleManager.done;
	}
}

	
