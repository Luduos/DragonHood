using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class DiscoveryScript : NetworkDiscovery {

	public NetworkClient Client;
	public int count = 0;


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


}
