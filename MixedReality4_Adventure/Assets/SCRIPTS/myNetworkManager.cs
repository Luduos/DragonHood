using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Networking.NetworkSystem;

public class myNetworkManager : NetworkManager {

	public Button playerFeatherButton;
	public Button playerBellButton;
	int avatarIndex = 0;
	public Canvas characterSelectionCanvas;

	public TickleManager tickleManager; 


    private bool isHost = false;


	// Use this for initialization
	void Start () {

		playerFeatherButton.onClick.AddListener (delegate {
																AvatarPicker (playerFeatherButton.name);
															});

		playerBellButton.onClick.AddListener (delegate {
																AvatarPicker (playerBellButton.name);
															});
		tickleManager= FindObjectOfType<TickleManager>(); // TEMPORARY - CHANGE THIS @AKASH!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	}



	void AvatarPicker(string buttonName)
	{
		switch (buttonName)
		{
		case "PlayerFeather":
			avatarIndex = 0;
			break;
		case "PlayerBell":
			avatarIndex = 1;
			break;
		}

		playerPrefab = spawnPrefabs [avatarIndex];
	}


	public override void OnClientConnect(NetworkConnection conn)
	{

		characterSelectionCanvas.enabled = false;

		IntegerMessage msg = new IntegerMessage (avatarIndex);
		if (!clientLoadedScene)
		{
			// Ready/AddPlayer is usually triggered by a scene load completing. if no scene was loaded, then Ready/AddPlayer it here instead.
			ClientScene.Ready(conn);
			if (autoCreatePlayer)
			{
				ClientScene.AddPlayer(conn,0,msg);
			}
		}

		client.RegisterHandler(AkashMessageType.BellMessageID, OnClientReceivedBellMessage);
        if (isHost)
        {
            NetworkServer.RegisterHandler(AkashMessageType.BellMessageID, OnHostReceivedBellMessage);
        }
	}
	//@david all the bools used
    public class BellMessage : MessageBase
    {
		public bool Rang;
        public bool RangOnce;
        public bool RangTwice;
        public bool RangThrice;
		public bool ResultStage1;
		public bool ResultStage2;
		public bool ResultStage3;
		public bool Agitate;
		public bool Disappear;
    }



    public class AkashMessageType
    {
        public static short BellMessageID = MsgType.Highest + 4;

    }

    /// <summary>
    /// Updates TickleMaster, if we receive a bell message
    /// </summary>
    /// <param name="netMsg"></param>
    private void OnClientReceivedBellMessage(NetworkMessage netMsg)
    {
        BellMessage msg = netMsg.ReadMessage<BellMessage>();
        
		tickleManager.rangBell = msg.Rang;
		tickleManager.rangBellOnce = msg.RangOnce;
        tickleManager.rangBellTwice = msg.RangTwice;
        tickleManager.rangeBellThrice = msg.RangThrice;
		tickleManager.resultStage1 = msg.ResultStage1;
		tickleManager.resultStage2 = msg.ResultStage2;
		tickleManager.resultStage3 = msg.ResultStage3;

    }

    /// <summary>
    /// Sends Bell Message to all clients
    /// </summary>
    /// <param name="netMsg"></param>
    private void OnHostReceivedBellMessage(NetworkMessage netMsg)
    {
        NetworkServer.SendToAll(AkashMessageType.BellMessageID, netMsg.ReadMessage<BellMessage>());
    }

    /// <summary>
    /// Call this, if you want to change the BellStatus of the TickleManager on the network.
    /// </summary>
    /// <param name="bellHasBeenRung"></param>
	public void CommunicateStatus(bool rang, bool rangOnce, bool rangTwice, bool rangThrice, bool resultStage1, bool resultStage2, bool resultStage3,bool agitate,bool disappear)
    {
        BellMessage message = new BellMessage();
		message.Rang = rang;
        message.RangOnce = rangOnce;
        message.RangTwice = rangTwice;
        message.RangThrice = rangThrice;
		message.ResultStage1 = resultStage1;
		message.ResultStage2 = resultStage2;
		message.ResultStage3 = resultStage3;
		message.Agitate = agitate;
		message.Disappear = disappear;
        client.Send(AkashMessageType.BellMessageID, message);
    }




    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
	{

		int id = 0;

		if (extraMessageReader != null)
		{
			IntegerMessage i = extraMessageReader.ReadMessage<IntegerMessage> ();
			id = i.value;
		}
		GameObject playerPrefab = spawnPrefabs [id];


		GameObject player;
		Transform startPos = GetStartPosition();
		if (startPos != null)
		{
			player = (GameObject)Instantiate(playerPrefab, startPos.position, startPos.rotation);
		}
		else
		{
			player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		}

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}



	public void CreateHost()
	{
		myNetworkManager.singleton.StartHost ();
        isHost = true;

    }

	public void CreateClient()
	{
		 myNetworkManager.singleton.StartClient ();
	}
}
