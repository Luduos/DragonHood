using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;


/// <summary>
/// @author Akash.
/// </summary>
public class TickleManager : MonoBehaviour {





	private bool valu;
	private bool mistaken=false;
	Text text;
	private RawImage imageInstruction;
	public Texture right;
	public Texture left;
	public Texture upAndDown;

	public GameObject ARCam1;
	public GameObject ARCam2;

	public bool rangBell,rangBellOnce, rangBellTwice, rangeBellThrice;
	public bool resultStage1,resultStage2,resultStage3;
	public bool youAreBehind;

	public bool agitate, disappear;

	private Animator animator;
	public AudioSource audioSource;
	public ParticleSystem fire;
	public MyNetworkManager myNetworkManager;
	public Vector3 ARCamDirection;
	public Vector3 DragonDirection;




	//for TickleSwipeRight

	private float minSwipeDistanceX1 = 25.0f;
	private float minSpeedX1 = 150.0f;
	private float maxSpeedX1 = 1500.0f;

	private float startPos1;
	private float startTime1;
	private float endPos1;
	private float endTime1;
	private float swipedDistanceX1;
	private float speedOfSwipe1 ;


	//for 3 finger poke

	/*private float minSwipeDistanceX2 = 25.0f;
	private float minSpeedX2 = 150.0f;
	private float maxSpeedX2 = 1500.0f;
	*/
    /*
	[SerializeField]
	private float startPos2;
	private float startTime2;
	[SerializeField]
	private float endPos2;
	private float endTime2;
	[SerializeField]
	private float swipedDistanceX2;
	[SerializeField]
	private float speedOfSwipe2 ;
    */


	//for TickleCrazy

	private float minSwipeDistanceY3 = 10000.0f;

	private float startPos3;
	private float startTime3;
	private float endPos3;
	private float endTime3;
	private float swipedDistanceY3;
	private float speedOfSwipe3 ;
	private float minSpeedY3= 10000.0f;


    private PlayerClassType playerClassType = PlayerClassType.NotChosen;
    [SerializeField]
    private PlayerLogic playerLogic = null;

	void Start () {
		
		rangBell= false;
		rangBellOnce= false;
		rangBellTwice= false;
		rangeBellThrice = false;

		text=this.GetComponentInChildren<Text>();
		imageInstruction = this.GetComponentInChildren<RawImage> ();
		mistaken = false;
        if(!fire)
		    fire = FindObjectOfType<ParticleSystem> ();
        if(!myNetworkManager)
		    myNetworkManager = FindObjectOfType<MyNetworkManager>();


		animator = this.GetComponent<Animator> ();
		animator.SetBool ("mistake", false);

        if (!playerLogic)
            playerLogic = FindObjectOfType<PlayerLogic>();
        playerLogic.OnClassSelected += OnClassSelected;

    }

    private void OnClassSelected(PlayerClassType type)
    {
        playerClassType = type;
    }


	void Update () 
	{
        imageInstruction.color = Color.white;
        //finds the cameras
        ARCam1 = GameObject.Find ("Camera1(Clone)");
		ARCam2 = GameObject.Find ("Camera2(Clone)");

        if(ARCam1)
		    ARCamDirection = ARCam1.transform.forward;
		DragonDirection = this.transform.forward;

		if (Vector3.Dot (ARCamDirection, DragonDirection) > 0) {
			youAreBehind = true;
			myNetworkManager.CommunicateStatus (rangBell, rangBellOnce, rangBellTwice, rangeBellThrice, resultStage1, resultStage2, resultStage3, agitate, disappear);
			//Debug.Log ("you are behind !!");
		} else {
			youAreBehind = false;
			//Debug.Log("you are in front !!");
		}

		//@david this is where set bool values are USED for the animations...The networkmanager.communicatestatus functions are called in the tickleswipedleft, 
		//tickledpoke3 and ticklecrazy functions where the bool values are SET and COMMUNICATED..
		//Starting From here
		if (agitate == true) {
			animator.SetBool ("mistake", true);
			fire.Play ();
			agitate = false;
		} else if (agitate == false)
		{
			animator.SetBool ("mistake", false);
		}; 
		if (disappear == true) {
			animator.SetBool ("disappear", true);
		} else if (disappear == false)
		{
			animator.SetBool ("disappear", false);
		}
		// To here
		StartTickling ();

	}


	public void StartTickling()
	{    
		if (PlayerClassType.PuzzleMaster == playerClassType && (Input.acceleration.y *1000) > 3000)
		{
			
			rangBell = true;
			audioSource.Play ();
			myNetworkManager.CommunicateStatus (rangBell,rangBellOnce,rangBellTwice,rangeBellThrice,resultStage1,resultStage2,resultStage3,agitate,disappear);


		}
		if (rangBell == true) {
            if (ARCam2)
            {
                Vector3 forward = ARCam2.transform.position - this.transform.position;
                forward = new Vector3(forward.x, 0.0f, forward.z);
                this.transform.rotation = Quaternion.LookRotation(forward);
            }
        }

		if (rangBell == false) {
            if (ARCam1)
            {
                Vector3 forward = ARCam1.transform.position - this.transform.position;
                forward = new Vector3(forward.x, 0.0f, forward.z);
                this.transform.rotation = Quaternion.LookRotation(forward);
            }
        }

        


		if ( youAreBehind == false ) {
			imageInstruction.texture = null;
			text.text = "He can see you ! \n Get Behind!";
			animator.SetBool ("mistake", true);

		} 

		if (youAreBehind == true) {

			animator.SetBool ("mistake", false);


			if (resultStage1 == false && rangBell == true) 
			{


				TickledSwipeRight ();
				resultStage1 = TickledSwipeRight ();
				myNetworkManager.CommunicateStatus (rangBell,rangBellOnce,rangBellTwice,rangeBellThrice,resultStage1,resultStage2,resultStage3,agitate,disappear);


			}

			if (resultStage1 == true && resultStage2 == false && rangBellOnce==false ) { 

				rangBell = false;

				rangBellOnce = true;
				myNetworkManager.CommunicateStatus (rangBell,rangBellOnce,rangBellTwice,rangeBellThrice,resultStage1,resultStage2,resultStage3,agitate,disappear);


			}
			if (resultStage1 == true && resultStage2 == false && rangBell==true && rangBellOnce==true) { 



				TickledPokeThree ();

				resultStage2 = TickledPokeThree ();
				myNetworkManager.CommunicateStatus (rangBell,rangBellOnce,rangBellTwice,rangeBellThrice,resultStage1,resultStage2,resultStage3,agitate,disappear);

			}

			if (resultStage2 == true && resultStage3 == false && rangBellTwice==false) { 
				

				resultStage1=false;
				rangBell = false;
				rangBellTwice = true;

				TickledCrazy ();
				resultStage3 = TickledCrazy ();
				myNetworkManager.CommunicateStatus (rangBell,rangBellOnce,rangBellTwice,rangeBellThrice,resultStage1,resultStage2,resultStage3,agitate,disappear);

			}
			if (resultStage2 == true && resultStage3 == false && rangBellTwice==true && rangBell==true) { 


				TickledCrazy ();
				resultStage3 = TickledCrazy ();
				myNetworkManager.CommunicateStatus (rangBell,rangBellOnce,rangBellTwice,rangeBellThrice,resultStage1,resultStage2,resultStage3,agitate,disappear);


			}
			if (resultStage3 == true && rangeBellThrice==false) { 
				//Debug.Log ("Dragon Disappears !!");
				text.text="";
				Destroy (imageInstruction);

                ShowFinalScene();
			}


		}

        if (!resultStage3 && PlayerClassType.PuzzleMaster == playerClassType)
        {
            ShowPuzzleMasterInfo();
        }
    }

    public void ShowPuzzleMasterInfo()
    {
        imageInstruction.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        if (youAreBehind)
        {
            text.text = "Get his Attention!";
        }
        else
        {
            text.text = "Distract him!";
        }
    }

    public void ShowFinalScene()
    {

    }

	public bool TickledSwipeRight()
	{ 

		if (mistaken == false) {
			text.text = "Lets tickle, slow and steady!";
			imageInstruction.texture = right;
		}
		bool done = false;

		Touch[] myTouches = Input.touches;
		if (Input.touchCount > 2 || (Input.touchCount < 2 && Input.touchCount > 0)) {
			agitate = true;
			myNetworkManager.CommunicateStatus (rangBell, rangBellOnce, rangBellTwice, rangeBellThrice, resultStage1, resultStage2, resultStage3, agitate, disappear);

			text.text = "Using more than two fingers only angers him!";
		}

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


				Debug.Log (Mathf.Sign (endPos1 - startPos1)+" is the sign , must be 1.");
				Debug.Log (swipedDistanceX1+" is Swiped Distance");
				Debug.Log (speedOfSwipe1+" is Speed of Swipe");



				if (swipedDistanceX1 > minSwipeDistanceX1 && speedOfSwipe1 > minSpeedX1 && speedOfSwipe1 < maxSpeedX1 && Mathf.Sign (endPos1 - startPos1) == 1 ) {


					mistaken = false;
					done = true;
					return done;
				}  else if (speedOfSwipe1 > maxSpeedX1 && swipedDistanceX1 > minSwipeDistanceX1) {
					agitate = true;
					myNetworkManager.CommunicateStatus (rangBell, rangBellOnce, rangBellTwice, rangeBellThrice, resultStage1, resultStage2, resultStage3, agitate, disappear);
					mistaken = true;
					text.text = "Be Gentle! Too quick!";


					//Debug.Log ("Be Gentle");
				} else if (speedOfSwipe1 > maxSpeedX1 && swipedDistanceX1 > minSwipeDistanceX1 && mistaken == true) {
					agitate = true;
					myNetworkManager.CommunicateStatus (rangBell, rangBellOnce, rangBellTwice, rangeBellThrice, resultStage1, resultStage2, resultStage3, agitate, disappear);
					valu = animator.GetBool ("mistake");
					Debug.Log (valu);
					text.text = "Be Gentle! Too quick!";

					break;
					//Debug.Log ("Be Gentle");
				} else if (speedOfSwipe1 < minSpeedX1 && swipedDistanceX1 > minSwipeDistanceX1) {
					agitate = true;
					myNetworkManager.CommunicateStatus (rangBell, rangBellOnce, rangBellTwice, rangeBellThrice, resultStage1, resultStage2, resultStage3, agitate, disappear);
					mistaken = true;
					text.text = "Too slow!";

				} else if (speedOfSwipe1 < minSpeedX1 && swipedDistanceX1 > minSwipeDistanceX1 && mistaken == true) {
					agitate = true;
					myNetworkManager.CommunicateStatus (rangBell, rangBellOnce, rangBellTwice, rangeBellThrice, resultStage1, resultStage2, resultStage3, agitate, disappear);
					text.text = "Too slow!";

				} 




			}

		}
		return done;
	}


	public bool TickledPokeThree()
	{
		
		if (mistaken == false) {
			text.text = "Poke him, three at a time!";

		}
		bool done = false;

		Touch[] myTouches = Input.touches;

		if (Input.touchCount == 3) {
			mistaken = false;
			done = true;
			return done;

		}
		else if (Input.touchCount > 3 || (Input.touchCount < 3 && Input.touchCount > 0)) {
			agitate = true;
			myNetworkManager.CommunicateStatus (rangBell, rangBellOnce, rangBellTwice, rangeBellThrice, resultStage1, resultStage2, resultStage3, agitate, disappear);
			text.text = "Poke with three!";
		}




		return done;
	}


	public bool TickledCrazy()
	{
		imageInstruction.texture = upAndDown;
		if (mistaken == false) {
			text.text ="Keep scratching him!\n QUICKLY, till he flies.";
		}

		bool done = false;
		animator.SetBool ("mistake", false);
		Touch[] myTouches = Input.touches;


		if (Input.touchCount > 2 || (Input.touchCount < 2 && Input.touchCount > 0)) {
			agitate = true;
			myNetworkManager.CommunicateStatus (rangBell, rangBellOnce, rangBellTwice, rangeBellThrice, resultStage1, resultStage2, resultStage3, agitate, disappear);
			text.text = "Using more than two fingers only angers him!";

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

				}


				if (myTouches [i].phase == TouchPhase.Ended) {
					endPos3= myTouches [i].position.x;
					endTime3 = Time.time - startTime3;

				}


				speedOfSwipe3 = swipedDistanceY3 / endTime3;






				if (swipedDistanceY3 > minSwipeDistanceY3 && speedOfSwipe3 > minSpeedY3) {
					disappear = true;
					myNetworkManager.CommunicateStatus (rangBell, rangBellOnce, rangBellTwice, rangeBellThrice, resultStage1, resultStage2, resultStage3, agitate, disappear);

					mistaken = false;
					done = true;
					return done;
				} else if (swipedDistanceY3 > minSwipeDistanceY3 && speedOfSwipe3 < minSpeedY3) {
					
					mistaken = true;

					agitate = true;
					myNetworkManager.CommunicateStatus (rangBell, rangBellOnce, rangBellTwice, rangeBellThrice, resultStage1, resultStage2, resultStage3, agitate, disappear);
				}else if (swipedDistanceY3 > minSwipeDistanceY3 && speedOfSwipe3 < minSpeedY3 && mistaken==true) {
					

					text.text="";
					agitate = true;
					myNetworkManager.CommunicateStatus (rangBell, rangBellOnce, rangBellTwice, rangeBellThrice, resultStage1, resultStage2, resultStage3, agitate, disappear);
				}




			}

		}
		return done;
	}



}
