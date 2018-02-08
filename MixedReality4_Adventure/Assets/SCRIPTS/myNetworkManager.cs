using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MyNetworkManager : NetworkManager {

	int avatarIndex = 0;

    [SerializeField]
    private PlayerLogic player = null;

	// Use this for initialization
	void Start () {
        if(null == player)
        {
            player = FindObjectOfType<PlayerLogic>();
        }
        player.OnClassSelected += AvatarPicker;
	}

	private void AvatarPicker(PlayerClassType classType)
	{
		switch (classType) 
		{
		case PlayerClassType.PuzzleMaster:
            avatarIndex = 0;
			break;
		case PlayerClassType.Fighter:
			avatarIndex = 1;
			break;
		}

		playerPrefab = spawnPrefabs [avatarIndex];
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
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
}
