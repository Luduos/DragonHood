using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class DiscoveryScript : NetworkDiscovery {

	public NetworkClient Client;

	public void InitialiseHost()
	{
		this.Initialize ();
		this.StartAsServer();
	}

	public void InitialiseClient()
	{
		
		this.Initialize ();
		this.StartAsClient();
	}

	public override void OnReceivedBroadcast(string fromAddress, string data)
	{
		if (Client == null) {
			NetworkManager.singleton.networkAddress = fromAddress;
			Client = NetworkManager.singleton.StartClient ();

		}
	}
}
