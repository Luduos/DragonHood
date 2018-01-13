using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MyClientBehaviour : MonoBehaviour {

    [SerializeField]
    private Text DebugText;

    [SerializeField]
    private PlayerLogic Player;

    private bool isHost;
    public bool IsHost { get { return isHost; } set { isHost = value; } }

    public NetworkClient Client;

    public class ClickedPuzzleMessage : MessageBase
    {

    }

    public class MyMsgType
    {
        public static short ChooseClass = MsgType.Highest + 1;
    };

    public void SetClient(NetworkClient client, bool isHost)
    {
        this.IsHost = isHost;
        Client = client;
        Client.RegisterHandler(MsgType.Connect, OnConnected);
        Client.RegisterHandler(MsgType.Error, OnError);
        Client.RegisterHandler(MsgType.Disconnect, OnDisconnect);
        Client.RegisterHandler(MyMsgType.ChooseClass, OnChooseClass);

        if (IsHost)
        {
            //NetworkServer.RegisterHandler(MyMsgType.ChooseClass, OnPOIVote);
        }
        else
        {
        }

        Debug.Log(Client.isConnected);
        DebugText.text = "Was Set";
    }

    /// <summary>
    /// Called by Button
    /// </summary>
    public void HostStartsClassDecision()
    {
        if (IsHost)
        {
            NetworkServer.SendToAll(MyMsgType.ChooseClass, new EmptyMessage());
        }
    }

    private void OnChooseClass(NetworkMessage netMsg)
    {
        Player.OnChooseClass();
    }

    private void OnError(NetworkMessage msg)
    {
        Debug.Log("Error");
        DebugText.text = "Error";
        this.Client.Shutdown();
        this.Client = null;
    }

    private void OnConnected(NetworkMessage msg)
    {
        Debug.Log("Connected to Server");
        DebugText.text = "Connected";
    }

    private void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player connected to Server");
        
    }

    private void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.Log("Network Connection Error");
        DebugText.text = "Connection Error";
        this.Client.Shutdown();
        this.Client = null;
    }

    void OnDisconnect(NetworkMessage msg)
    {
        Debug.Log("Disconnect");
        DebugText.text = "Disconnect";
        this.Client.Shutdown();
        this.Client = null;
    }
}
