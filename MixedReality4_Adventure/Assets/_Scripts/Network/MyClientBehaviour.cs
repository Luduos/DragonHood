using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

/// <summary>
/// @author: David Liebemann
/// </summary>
public class MyClientBehaviour : MonoBehaviour {

    [SerializeField]
    private Text DebugText = null;

    [SerializeField]
    private PlayerLogic Player = null;

    [SerializeField]
    private CreatorLogic Creator = null;

    private bool isHost;
    public bool IsHost { get { return isHost; } set { isHost = value; } }

    public NetworkClient Client = null;

    public class ClickedPuzzleMessage : MessageBase
    {
        public int FaceID;
    }

    public class ChooseAdventureMessage : MessageBase
    {
        public POISaveInfo[] poiSaveInfos;
    }

    public class MyMsgType
    {
        public static short ChooseClass = MsgType.Highest + 1;
        public static short ClickedPuzzle = MsgType.Highest + 2;
        public static short WrongPuzzleTouch = MsgType.Highest + 3;
        public static short ChooseAdventure = MsgType.Highest + 4;
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
        Client.RegisterHandler(MyMsgType.ChooseAdventure, OnReceivedChooseAdventure);

        if (IsHost)
        {
            NetworkServer.RegisterHandler(MyMsgType.ClickedPuzzle, OnHostClickedPuzzleMessage);
            NetworkServer.RegisterHandler(MyMsgType.WrongPuzzleTouch, OnHostWrongTouch);
            NetworkServer.RegisterHandler(MyMsgType.ChooseAdventure, OnHostReceivedChooseAdventure);
        }
        MyNetworkManager currentManager = (MyNetworkManager)MyNetworkManager.singleton;
        currentManager.IsHost = isHost;
        currentManager.OnInitHandlers();

        Debug.Log(Client.isConnected);
        DebugText.text = "Was Set";
    }

    public void OnLoadAdventure(List<POISaveInfo> poiSaveInfos)
    {
        ChooseAdventureMessage adventureMessage = new ChooseAdventureMessage();
        adventureMessage.poiSaveInfos = poiSaveInfos.ToArray();

        if (null != Client)
            Client.Send(MyMsgType.ChooseAdventure, adventureMessage);
    }

    /// <summary>
    /// Called, when client received a "choose adventure" message. Loads the chosen adventure.
    /// </summary>
    /// <param name="netMsg"></param>
    private void OnReceivedChooseAdventure(NetworkMessage netMsg)
    {
        ChooseAdventureMessage advMessage =  netMsg.ReadMessage<ChooseAdventureMessage>();

        Creator.LoadPOIsFromSaveFile(new List<POISaveInfo>(advMessage.poiSaveInfos));
    }

    /// <summary>
    /// Called, when Host received a "choose adventure" message. Delivers the message to all clients
    /// </summary>
    /// <param name="netMsg"></param>
    private void OnHostReceivedChooseAdventure(NetworkMessage netMsg)
    {
        NetworkServer.SendToAll(MyMsgType.ChooseAdventure, netMsg.ReadMessage<ChooseAdventureMessage>());
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
        NetworkManager.singleton.OnClientConnect(Client.connection);
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
