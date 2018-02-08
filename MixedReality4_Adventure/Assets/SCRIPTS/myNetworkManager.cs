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
	public NetworkClient Client;


	// Use this for initialization
	void Start () {

		playerFeatherButton.onClick.AddListener (delegate {
																AvatarPicker (playerFeatherButton.name);
															});

		playerBellButton.onClick.AddListener (delegate {
																AvatarPicker (playerBellButton.name);
															});

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

		//characterSelectionCanvas.enabled = false;

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
	}

	public void CreateClient()
	{
		 myNetworkManager.singleton.StartClient ();
	}
}
