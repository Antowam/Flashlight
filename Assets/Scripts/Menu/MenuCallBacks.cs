using Bolt.Matchmaking;
using System;
using UdpKit;

public class MenuCallBacks : Bolt.GlobalEventListener
{
    //What happens when you click the host server button
    public void StartServer()
    {
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
            string matchName = "Its_a_match";

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

            if(photonSession.Source == UdpSessionSource.Photon)
            {
                BoltNetwork.Connect(photonSession);
            }
        }
    }
}