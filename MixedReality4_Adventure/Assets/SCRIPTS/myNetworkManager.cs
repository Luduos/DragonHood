using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MyNetworkManager : NetworkManager {

	int avatarIndex = 0;

    [SerializeField]
    private PlayerLogic playerLogic = null;

    private bool ClassWasSelected = false;


	// Use this for initialization
	void Start () {
        Time.timeScale = 1.0f;
        if(null == playerLogic)
        {
            playerLogic = FindObjectOfType<PlayerLogic>();
        }
        playerLogic.OnClassSelected += AvatarPicker;
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

        ClassWasSelected = true;  
    }

    
	public override void OnClientConnect(NetworkConnection conn)
	{
        StartCoroutine(WaitingForClassSelection(conn));
    }

    private IEnumerator WaitingForClassSelection(NetworkConnection conn)
    {
        while (!ClassWasSelected)
        {
            yield return new WaitForSeconds(0.5f);
        }

        IntegerMessage msg = new IntegerMessage(avatarIndex);


        if (!clientLoadedScene)
        {
            // Ready/AddPlayer is usually triggered by a scene load completing. if no scene was loaded, then Ready/AddPlayer it here instead.
            ClientScene.Ready(conn);
            if (autoCreatePlayer)
            {
                ClientScene.AddPlayer(conn, 0, msg);
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
			player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		}
		else
		{
			player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		}

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

}
