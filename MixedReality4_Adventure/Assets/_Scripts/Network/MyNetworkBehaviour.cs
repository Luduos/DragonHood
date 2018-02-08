using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
/// <summary>
/// @author: David Liebemann
/// </summary>
[RequireComponent(typeof(MyClientBehaviour))]
public class MyNetworkBehaviour : NetworkDiscovery {

    private MyClientBehaviour clientBehaviour;

    private void Start()
    {
        clientBehaviour = this.gameObject.GetComponent<MyClientBehaviour>();
        StartCoroutine(DelayedClientStart(0.2f));
    }

    private IEnumerator DelayedClientStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.Initialize();
        this.StartAsClient();
    }

    public void OnServeAsHost()
    {
        if (null == clientBehaviour.Client)
        {
            clientBehaviour.Client = MyNetworkManager.singleton.StartHost();
            if (clientBehaviour.Client != null)
            {
                clientBehaviour.SetClient(clientBehaviour.Client, true);

            }
            this.StopBroadcast();
            this.Initialize();
            this.StartAsServer();
        }
        else
        {
            if (!clientBehaviour.Client.isConnected)
            {
                MyNetworkManager.singleton.StartServer();
                clientBehaviour.IsHost = true;
                this.StopBroadcast();
                this.Initialize();
                this.StartAsServer();
            }
            else if (clientBehaviour.IsHost)
            {
                MyNetworkManager.singleton.StopHost();
                clientBehaviour.IsHost = false;
                clientBehaviour.Client.Disconnect();
                this.StopBroadcast();
                this.Initialize();
                this.StartAsClient();
            }
        }
        
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        if(clientBehaviour.Client == null)
        {
            NetworkManager.singleton.networkAddress = fromAddress;
            clientBehaviour.Client = NetworkManager.singleton.StartClient();
            if (clientBehaviour.Client != null)
            {
                clientBehaviour.SetClient(clientBehaviour.Client, false);
            }
        }
        else
        {
            if (!clientBehaviour.Client.isConnected)
            {
                clientBehaviour.Client.Connect(fromAddress, MyNetworkManager.singleton.networkPort);
            }
        }
        
    }

    private void OnDestroy()
    {

        if (clientBehaviour.IsHost)
        {
            MyNetworkManager.singleton.StopHost();
            this.StopBroadcast();
        }
        else
        {
            MyNetworkManager.singleton.StopClient();
        }
        MyNetworkManager.singleton.networkAddress = "localHost";
        clientBehaviour.IsHost = false;
    }
}
