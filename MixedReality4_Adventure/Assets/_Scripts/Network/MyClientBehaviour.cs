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
        public int FaceID;
    }

    public class MyMsgType
    {
        public static short ChooseClass = MsgType.Highest + 1;
        public static short ClickedPuzzle = MsgType.Highest + 2;
        public static short WrongPuzzleTouch = MsgType.Highest + 3;

    };

    public void SetClient(NetworkClient client, bool isHost)
    {
        this.IsHost = isHost;
        Client = client;
        Client.RegisterHandler(MsgType.Connect, OnConnected);
        Client.RegisterHandler(MsgType.Error, OnError);
        Client.RegisterHandler(MsgType.Disconnect, OnDisconnect);
        Client.RegisterHandler(MyMsgType.ChooseClass, OnChooseClass);
        Client.RegisterHandler(MyMsgType.ClickedPuzzle, OnClickedPuzzle);
        Client.RegisterHandler(MyMsgType.WrongPuzzleTouch, OnWrongPuzzleTouch);

        if (IsHost)
        {
            NetworkServer.RegisterHandler(MyMsgType.ClickedPuzzle, OnHostClickedPuzzleMessage);
            NetworkServer.RegisterHandler(MyMsgType.WrongPuzzleTouch, OnHostWrongTouch);
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

    // Using the following IDs:
    // 1 and 2 lie opposite to each other and
    // 3 and 4 lie opposite to each other
    // 5 is on top
    public void ClientTouchedBoxFace(int ID)
    {
        ClickedPuzzleMessage msg = new ClickedPuzzleMessage();
        msg.FaceID = ID;
        if(null != Client)
            Client.Send(MyMsgType.ClickedPuzzle, msg);
    }

    public void ClientRegisteredWrongTouch()
    {
        Client.Send(MyMsgType.WrongPuzzleTouch, new EmptyMessage());
    }

    private void OnHostClickedPuzzleMessage(NetworkMessage netMsg)
    {
        NetworkServer.SendToAll(MyMsgType.ClickedPuzzle, netMsg.ReadMessage<ClickedPuzzleMessage>());
    }

    private void OnHostWrongTouch(NetworkMessage netMsg)
    {
        NetworkServer.SendToAll(MyMsgType.WrongPuzzleTouch, new EmptyMessage());
    }

    private void OnChooseClass(NetworkMessage netMsg)
    {
        Player.OnStartClassDecision();
    }

    private void OnClickedPuzzle(NetworkMessage netMsg)
    {
        ClickedPuzzleMessage msg = netMsg.ReadMessage<ClickedPuzzleMessage>();
        Player.OnRegisteredPuzzleClick(msg.FaceID);
    }

    private void OnWrongPuzzleTouch(NetworkMessage netMsg)
    {
        Player.OnWrongPuzzleTouch();
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
