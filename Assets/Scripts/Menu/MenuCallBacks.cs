using System.Collections.Generic;
using Bolt.Matchmaking;
using System;
using UdpKit;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MenuCallBacks : Bolt.GlobalEventListener
{
    public Button serverJoinGameButton;
    public GameObject serverListPanel;
    private List<Button> joinServerButtonList = new List<Button>();
    private float buttonSpacing = -100.0f;

    public string serverName = "";
    public TextMeshProUGUI serverNameText;

    private void Awake()
    {
        Screen.fullScreen = false;
    }

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
        ClearServerList();
        foreach(var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;
            UdpSession photonSessionOld = photonSession;

            Button joinGameButtonClone = Instantiate(serverJoinGameButton);
            joinGameButtonClone.transform.SetParent(serverListPanel.transform);
            joinGameButtonClone.transform.localScale = new Vector3(1, 1, 1);
            //joinGameButtonClone.transform.localPosition = new Vector3(0, buttonSpacing * joinServerButtonList.Count, 0);
            joinGameButtonClone.GetComponentInChildren<TextMeshProUGUI>().text = photonSession.HostName;

            joinGameButtonClone.onClick.AddListener(() => JoinGame(photonSession));

            joinServerButtonList.Add(joinGameButtonClone);

            //this joins the first session
            //if (photonSession.Source == UdpSessionSource.Photon)
            //{
            //    BoltNetwork.Connect(photonSession);
            //}
        }
    }

    public void JoinGame(UdpSession photonSession)
    {
        BoltNetwork.Connect(photonSession);
    }

    public void QuickJoin()
    {
        BoltMatchmaking.JoinRandomSession();
    }

    private void ClearServerList()
    {
        foreach(Button button in joinServerButtonList)
        {
            Destroy(button.gameObject);
        }

        joinServerButtonList.Clear();
    }

    public void Back()
    {
        BoltLauncher.Shutdown();
    }
}