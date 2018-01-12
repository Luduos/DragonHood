using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class MyClientBehaviour : MonoBehaviour {

    [SerializeField]
    private Text DebugText;


    private bool isHost;
    public bool IsHost { get { return isHost; } set { isHost = value; } }

    public NetworkClient Client;


    public class MyMsgType
    {
        public static short ClassTaken = MsgType.Highest + 1;
    };

    public class ClassTakenMessage : MessageBase
    {
        public uint ClassID;
    }

    public void SetClient(NetworkClient client, bool isHost)
    {
        this.IsHost = isHost;
        Client = client;
        Client.RegisterHandler(MsgType.Connect, OnConnected);
        Client.RegisterHandler(MsgType.Error, OnError);
        Client.RegisterHandler(MsgType.Disconnect, OnDisconnect);


        if (IsHost)
        {
            //NetworkServer.RegisterHandler(MyMsgType.POIVote, OnPOIVote);
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
    public void HostStartsVote()
    {
        if (IsHost)
        {
            //NetworkServer.SendToAll(MyMsgType.StartVote, new EmptyMessage());
        }
    }

    private void OnStartVoting(NetworkMessage netMsg)
    {
        Debug.Log("It's voting time!");
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
