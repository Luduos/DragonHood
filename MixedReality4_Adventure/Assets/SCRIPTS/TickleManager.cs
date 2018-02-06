using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TickleManager : MonoBehaviour {

	public static bool done;
	private bool resultStage1;
	private bool resultStage2;
	private bool resultStage3;
	private Animator animator;
	private bool valu;
	private bool mistaken=false;
	Text text;
	private RawImage imageInstruction;
	public Texture right;
	public Texture left;
	public Texture upAndDown;
	public bool hasBell;
	public bool hasFeather;
	public GameObject ARCam1;
	public GameObject ARCam2;
	public bool rangBell;
	public bool rangBellOnce, rangBellTwice, rangeBellThrice;
	public AudioSource audioSource;




	//for TickleSwipeRight

	private float minSwipeDistanceX1 = 50.0f;

	private float startPos1;
	private float startTime1;
	private float endPos1;
	private float endTime1;
	private float swipedDistanceX1;
	private float speedOfSwipe1 ;
	private float minSpeedX1 = 250.0f;
	private float maxSpeedX1 = 1000.0f;


	//for TickleSwipeLeft

	private float minSwipeDistanceX2 = 100.0f;

	private float startPos2;
	private float startTime2;
	private float endPos2;
	private float endTime2;
	private float swipedDistanceX2;
	private float speedOfSwipe2 ;
	private float minSpeedX2 = 250.0f;
	private float maxSpeedX2 = 1000.0f;


	//for TickleCrazy

	private float minSwipeDistanceY3 = 10000.0f;

	private float startPos3;
	private float startTime3;
	private float endPos3;
	private float endTime3;
	private float swipedDistanceY3;
	private float speedOfSwipe3 ;
	private float minSpeedY3= 10000.0f;


	void Start () {
		//Debug.Log(this.transform.rotation);
		rangBell= false;
		rangBellOnce= false;
		rangBellTwice= false;
		rangeBellThrice = false;

		text=this.GetComponentInChildren<Text>();
		imageInstruction = this.GetComponentInChildren<RawImage> ();
		mistaken = false;

		//this.transform.rotation = new Quaternion (0, 1, 0, 0);
		animator = this.GetComponent<Animator> ();
		animator.SetBool ("mistake", false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		ARCam1 = GameObject.Find ("Camera1(Clone)");
		ARCam2 = GameObject.Find ("Camera2(Clone)");

		StartTickling ();

	}


	public void StartTickling()
	{    //resultStage3 != true && resultStage2!=true && resultStage1!=true && 
		if ((Input.acceleration.y *1000) > 3000) {
			Debug.Log (Input.acceleration.y * 3000);
			audioSource.Play ();

		}
		if (ARCam2 && (Input.acceleration.y *1000) > 3000)
		{
			rangBell = true;
			audioSource.Play ();
		}
		if (rangBell == true) {
			this.transform.rotation = Quaternion.LookRotation (ARCam2.transform.position);
		}

		if (rangBell == false) {
			this.transform.rotation = Quaternion.LookRotation (ARCam1.transform.position);
		}
		if ( FromBehindTheDragon.youAreBehind == false ) {
			imageInstruction.texture = null;
			text.text = "He can see you ! \n Get Behind!";
			animator.SetBool ("mistake", true);

		} 
	

		if (FromBehindTheDragon.youAreBehind == true) {

			animator.SetBool ("mistake", false);

			if (resultStage1 == false) 
			{Debug.Log (this.transform.rotation);
				
					
				TickleSwipedRight ();
				resultStage1 = TickleSwipedRight ();


			}

			if (resultStage1 == true && resultStage2 == false && rangBellOnce==false ) { //TickleSwipeRight
				
				rangBell = false;

				rangBellOnce = true;
					

			}
			if (resultStage1 == true && resultStage2 == false && rangBell==true && rangBellOnce==true) { //TickleSwipeRight



				TickledSwipeLeft ();

				resultStage2 = TickledSwipeLeft ();

			}

			if (resultStage2 == true && resultStage3 == false && rangBellTwice==false) { 
				//TickleSwipeLeft

				resultStage1=false;
				rangBell = false;
				rangBellTwice = true;

				TickledCrazy ();
				resultStage3 = TickledCrazy ();
			}
			if (resultStage2 == true && resultStage3 == false && rangBellTwice==true && rangBell==true) { 
				

				TickledCrazy ();
				resultStage3 = TickledCrazy ();

			}
			if (resultStage3 == true && rangeBellThrice==false) { //TickleCrazy
				Debug.Log ("Dragon Disappears !!");
				text.text="";
				Destroy (imageInstruction);
			}


		} 
	}

	public bool TickleSwipedRight()

	{ 
		if (mistaken == false) {
			text.text = "Lets tickle!Do the gesture slow.";
			imageInstruction.texture = right;
		}
		TickleManager.done = false;

		Touch[] myTouches = Input.touches;
		if (Input.touchCount > 2 || (Input.touchCount < 2 && Input.touchCount > 0)) {
			animator.SetBool ("mistake", true);
			text.text = "You see those 2 arrows, what could they mean ?";
		}

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



				valu =animator.GetBool ("mistake");
				Debug.Log (valu);


				if (swipedDistanceX1 > minSwipeDistanceX1 && speedOfSwipe1 > minSpeedX1 && speedOfSwipe1 < maxSpeedX1 && Mathf.Sign (endPos1 - startPos1) == 1) {

					//Debug.Log ("The speed in the right direction is " + speedOfSwipe1);
					mistaken = false;
					TickleManager.done = true;
					return TickleManager.done;
				}  else if (speedOfSwipe1 > maxSpeedX1 && swipedDistanceX1 > minSwipeDistanceX1) {
					animator.SetBool ("mistake", true);
					mistaken = true;
					text.text = "Be Gentle! Too quick!";

					//Debug.Log ("Be Gentle");
				} else if (speedOfSwipe1 > maxSpeedX1 && swipedDistanceX1 > minSwipeDistanceX1 && mistaken == true) {
					animator.SetBool ("mistake", true);
					valu = animator.GetBool ("mistake");
					Debug.Log (valu);
					text.text = "Be Gentle! Too quick!";
					break;
					//Debug.Log ("Be Gentle");
				} else if (speedOfSwipe1 < minSpeedX1 && swipedDistanceX1 > minSwipeDistanceX1) {
					animator.SetBool ("mistake", true);
					mistaken = true;
					text.text = "Too slow!";
					//Debug.Log("Didnt even feel that , Come on !!");
				} else if (speedOfSwipe1 < minSpeedX1 && swipedDistanceX1 > minSwipeDistanceX1 && mistaken == true) {
					animator.SetBool ("mistake", true);
					text.text = "Too slow!";
					//Debug.Log("Didnt even feel that , Come on !!");
				} 




			}

		}
		return TickleManager.done;
	}


	public bool TickledSwipeLeft()
	{
		Debug.Log (this.transform.rotation);

		if (mistaken == false) {
			text.text = "It works YAY !\n Do the gesture slow";
			imageInstruction.texture = left;
		}



		TickleManager.done = false;
		animator.SetBool ("mistake", false);
		Touch[] myTouches = Input.touches;
		if (Input.touchCount > 3 || (Input.touchCount < 3 && Input.touchCount > 0)) {
			animator.SetBool ("mistake", true);
			text.text = "You see those 3 arrows , what could they mean?";
		}
		if (Input.touchCount == 3) {


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
					swipedDistanceX2 = Mathf.Abs(endPos2 - startPos2);
				}
				speedOfSwipe2 = swipedDistanceX2 / endTime2;






				if (swipedDistanceX2 > minSwipeDistanceX2 && speedOfSwipe2 > minSpeedX2 && speedOfSwipe2 < maxSpeedX2 && Mathf.Sign (endPos2 - startPos2) == -1) {

					Debug.Log ("The speed in left direction is " + speedOfSwipe2);
					mistaken = false;
					TickleManager.done = true;
					return TickleManager.done;
				} else if (speedOfSwipe2 > maxSpeedX2 && swipedDistanceX2 > minSwipeDistanceX2) {
					text.text = "Be Gentle! Too quick!";
					mistaken = true;

					//Debug.Log ("Be Gentle");
					animator.SetBool ("mistake", true);
					break;
				} else if (speedOfSwipe2 > maxSpeedX2 && swipedDistanceX2 > minSwipeDistanceX2 && mistaken == true) {
					text.text = "Be Gentle! Too quick!";
					//Debug.Log ("Be Gentle");
					animator.SetBool ("mistake", true);
					break;

				} else if (speedOfSwipe2 < minSpeedX2 && swipedDistanceX2 > minSwipeDistanceX2) {
					
					//Debug.Log("Didnt even feel that , Come on !!");
					animator.SetBool ("mistake", true);
					mistaken = true;

					//text.text = "Too slow didnt even feel that , Come on !! Long And Slow to the left";
				} else if (speedOfSwipe2 < minSpeedX2 && swipedDistanceX2 > minSwipeDistanceX2 && mistaken == true) {
					
					//Debug.Log("Didnt even feel that , Come on !!");
					animator.SetBool ("mistake", true);
					//text.text = "Too slow didnt even feel that , Come on !! Long And Slow to the left!";
				} 




			}

		}
		return TickleManager.done;
	}


	public bool TickledCrazy()
	{
		imageInstruction.texture = upAndDown;
		if (mistaken == false) {
			text.text ="Continuously do the gesture\n QUICKLY till he flies";
		}

		TickleManager.done = false;
		animator.SetBool ("mistake", false);
		Touch[] myTouches = Input.touches;


		if (Input.touchCount > 2 || (Input.touchCount < 2 && Input.touchCount > 0)) {
			animator.SetBool ("mistake", true);
			text.text = "You see those 2 arrows,what could they mean?";
		}


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
					animator.SetBool ("disappear", true);
					//Debug.Log ("Muhuhuuhuhahahahhaha !!!! ");
					mistaken = false;
					TickleManager.done = true;
					return TickleManager.done;
				} else if (swipedDistanceY3 > minSwipeDistanceY3 && speedOfSwipe3 < minSpeedY3) {
					//Debug.Log ("Too slow Come on !!");
					mistaken = true;
					text.text="Not Fast enough :/";
					animator.SetBool("mistake",true);
				}else if (swipedDistanceY3 > minSwipeDistanceY3 && speedOfSwipe3 < minSpeedY3 && mistaken==true) {
					//Debug.Log ("Too slow Come on !!");

					text.text="Not Fast enough :/";
					animator.SetBool("mistake",true);
				}




			}

		}
		return TickleManager.done;
	}



}
