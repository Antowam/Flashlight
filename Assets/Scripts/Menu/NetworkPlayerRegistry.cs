using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NetworkPlayerRegistry
{
    public static List<NetPlayerHandling> players = new List<NetPlayerHandling>();

    static NetPlayerHandling CreatePlayer(BoltConnection connection)
    {
        NetPlayerHandling player;

        player = new NetPlayerHandling();
        player.connection = connection;

        if (player.connection != null)
        {
            player.connection.UserData = player;
        }

        players.Add(player);

        return player;
    }

    public static IEnumerable<NetPlayerHandling> GetAllPlayers
    {
        get { return players; }
    }

    public static NetPlayerHandling CreateServerPlayer()
    {
        return CreatePlayer(null);
    }

    public static NetPlayerHandling ServerPlayer
    {
        get { return players.Find(player => player.IsServer); }
    }

    public static NetPlayerHandling CreateClientPlayer(BoltConnection connection)
    {
        return CreatePlayer(connection);
    }

    public static NetPlayerHandling GetClientPlayer(BoltConnection connection)
    {
        if (connection == null)
        {
            return ServerPlayer;
        }

        return (NetPlayerHandling)connection.UserData;
    }
}
