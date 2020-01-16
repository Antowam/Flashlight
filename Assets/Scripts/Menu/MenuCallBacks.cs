using Bolt.Matchmaking;
using System;
using UdpKit;
using UnityEngine.UI;
using TMPro;

public class MenuCallBacks : Bolt.GlobalEventListener
{
    public Button serverJoinGameButton;

    public string serverName = "";
    public TextMeshProUGUI serverNameText;

    //What happens when you click the host server button
    public void StartServer()
    {
        if(serverName != null)
        {
            serverName = serverNameText.text;
        }
        BoltLauncher.StartServer();
    }

    //What happens when you click join random server button
    public void StartClient()
    {
        BoltLauncher.StartClient();
    }

    //What happens when start server is done
    public override void BoltStartDone()
    {
        //If it was called from the host of the game
        if (BoltNetwork.IsServer)
        {
            string matchName = serverName;

            BoltMatchmaking.CreateSession( 
                sessionID: matchName, 
                sceneToLoad: "Room"
            );
        }
    }

    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        foreach(var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;

            /* this joins the first session
            if(photonSession.Source == UdpSessionSource.Photon)
            {
                BoltNetwork.Connect(photonSession);
            }
            */
        }
    }

    public void QuickJoin(Map<Guid, UdpSession> sessionList)
    {
        foreach (var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;

            //This connects to the first one found
            if(photonSession.Source == UdpSessionSource.Photon)
            {
                BoltNetwork.Connect(photonSession);
            }
        }
    }
}