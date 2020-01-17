using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bolt.Matchmaking;
using UdpKit;
using TMPro;

public class MenuCallBacks : Bolt.GlobalEventListener
{
    public Button serverJoinGameButton;
    public GameObject serverListPanel;
    private List<Button> joinServerButtonList = new List<Button>();

    string serverName;
    public TextMeshProUGUI serverNameText;

    public List<MapObject> mapID = new List<MapObject>();
    int mapSelValue = 0;
    public TextMeshProUGUI mapName;
    public Image mapImage;

    //This is just for easy testing so that i can move game windows
    private void Awake()
    {
        Screen.fullScreen = false;
    }

    private void Start()
    {
        mapSelValue = 0;
        SetMapSelection();
    }

    //What happens when you click the host server button
    public void StartServer()
    {
        if (serverNameText.text != null)
        {
            serverName = serverNameText.text;
        }
        else
        {
            serverName = "No Name " + UnityEngine.Random.Range(0, 25555);
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
            string levelName = mapName.text;

            BoltMatchmaking.CreateSession( 
                sessionID: matchName, 
                sceneToLoad: levelName
            );
        }
    }

    //Updates server list
    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        ClearServerList();
        foreach(var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;
            UdpSession photonSessionOld = photonSession;

            //Might switch this out for a pool of servers if garbage handling gets to bad;
            Button joinGameButtonClone = Instantiate(serverJoinGameButton);
            joinGameButtonClone.transform.SetParent(serverListPanel.transform);
            joinGameButtonClone.transform.localScale = new Vector3(1, 1, 1);
            //joinGameButtonClone.transform.localPosition = new Vector3(0, buttonSpacing * joinServerButtonList.Count, 0); //this is now handled by the vertical group handler
            joinGameButtonClone.GetComponentInChildren<TextMeshProUGUI>().text = photonSession.HostName;
            
            //Adds join game function to onclick on the instantiated buttons
            joinGameButtonClone.onClick.AddListener(() => JoinGame(photonSession));
            
            joinServerButtonList.Add(joinGameButtonClone);
        }
    }

    //Join game function but on buttons
    public void JoinGame(UdpSession photonSession)
    {
        BoltNetwork.Connect(photonSession);
    }

    //Join a random session
    public void QuickJoin()
    {
        BoltMatchmaking.JoinRandomSession();
    }

    //Clears server list
    public void ClearServerList()
    {
        foreach(Button button in joinServerButtonList)
        {
            Destroy(button.gameObject);
        }

        joinServerButtonList.Clear();
    }

    void SetMapSelection()
    {
        mapName.text = mapID[mapSelValue].name;
        mapImage.sprite = mapID[mapSelValue].levelThumbnail;
    }

    //What happens when you press the right map selection button
    public void ScrollRightMapList()
    {
        if(mapSelValue < mapID.Count - 1)
        {
            mapSelValue++;
            SetMapSelection();
        }
        else
        {
            mapSelValue = 0;
            SetMapSelection();
        }
    }
    //What happens when you press the left map selection button
    public void ScrollLeftMapList()
    {
        if (mapSelValue <= mapID.Count && mapSelValue > 0)
        {
            mapSelValue--;
            SetMapSelection();
        }
        else
        {
            mapSelValue = mapID.Count - 1;
            SetMapSelection();
        }
    }

    //fixes problem with entering server list and going back and back to list
    public void Back()
    {
        BoltLauncher.Shutdown();
    }
}