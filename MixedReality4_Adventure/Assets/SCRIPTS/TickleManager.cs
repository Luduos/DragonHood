using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickleManager : MonoBehaviour {

	public static bool done;
	private bool resultStage1;
	private bool resultStage2;
	private bool resultStage3;




	//for TickleSwipeRight

	private float minSwipeDistanceX1 = 500.0f;

	private float startPos1;
	private float startTime1;
	private float endPos1;
	private float endTime1;
	private float swipedDistanceX1;
	private float speedOfSwipe1 ;
	private float minSpeedX1 = 400.0f;
	private float maxSpeedX1 = 800.0f;


	//for TickleSwipeLeft

	private float minSwipeDistanceX2 = 500.0f;

	private float startPos2;
	private float startTime2;
	private float endPos2;
	private float endTime2;
	private float swipedDistanceX2;
	private float speedOfSwipe2 ;
	private float minSpeedX2 = 400.0f;
	private float maxSpeedX2 = 800.0f;


	//for TickleCrazy

	private float minSwipeDistanceY3 = 20000.0f;

	private float startPos3;
	private float startTime3;
	private float endPos3;
	private float endTime3;
	private float swipedDistanceY3;
	private float speedOfSwipe3 ;
	private float minSpeedY3= 20000.0f;


	void Start () {
		Debug.Log(this.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () 
	{

		StartTickling ();

	}


	public void StartTickling()
	{
		if (FromBehindTheDragon.youAreBehind == true) {

			if (resultStage1 == false) 
			{
				TickleSwipedRight ();
				resultStage1 = TickleSwipedRight ();

			}

			if (resultStage1 == true && resultStage2 == false) { //TickleSwipeRight
				this.transform.rotation= new Quaternion(0,1,0,0);
				Debug.Log (transform.forward);
				TickledSwipeLeft ();
				resultStage2 = TickledSwipeLeft ();
			}
			if (resultStage2 == true && resultStage3 == false) { //TickleSwipeLeft
				this.transform.rotation= new Quaternion(0,0,0,1);
				TickledCrazy ();
				resultStage3 = TickledCrazy ();
			}
			if (resultStage3 == true) { //TickleCrazy
				Debug.Log ("Dragon Disappears !!");
			}

		} 
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
					//Debug.Log ("Start pos for stage 1 is " + startPos1);

				}

				if (myTouches [i].phase == TouchPhase.Ended) {
					endPos1 = myTouches [i].position.x;
					endTime1 = Time.time - startTime1;
					//Debug.Log ("End pos for stage 1 is " + endPos1);

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

					//Debug.Log ("Be Gentle");
				} else if(speedOfSwipe1 < minSpeedX1 && swipedDistanceX1 > minSwipeDistanceX1)
				{
					//Debug.Log("Didnt even feel that , Come on !!");
				}



			}

		}
		return TickleManager.done;
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
					//Debug.Log ("Start pos for stage 2 is " + startPos2);

				}

				if (myTouches [i].phase == TouchPhase.Ended) {
					endPos2 = myTouches [i].position.x;
					endTime2 = Time.time - startTime2;
					//Debug.Log ("End pos for stage 2 is " + endPos2);

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

					//Debug.Log ("Be Gentle");
				} else if(speedOfSwipe2 < minSpeedX2 && swipedDistanceX2 > minSwipeDistanceX2)
				{
					//Debug.Log("Didnt even feel that , Come on !!");
				}



			}

		}
		return TickleManager.done;
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
