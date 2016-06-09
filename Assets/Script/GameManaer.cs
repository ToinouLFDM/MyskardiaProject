using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManaer: MonoBehaviour

{
    
    public static GameManaer instance;
    public static float timer;
    public int respawnTime = 3;

    //static bool reboot = false;




    //static double time;
    void Awake()
    {




        if (instance != null)
        {
            Debug.LogError("Too many GMs");
        }
        else
        {
            instance = this;
        }
    }
    void Update()
    {

    }

    private const string ID_PREFIX = "Player ";
    static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void ReisterPlayer(string _netID, Player _player)
    {



        string playerID = ID_PREFIX + _netID;
        players.Add(playerID, _player);

        _player.transform.name = playerID;

        players[playerID].Timer = timer;

    }

    public static void UnregisterPlayer(string name)
    {
        players.Remove(name);
    }














    internal static Player[] GetAllPlayers()
    {
        Player[] allPlayers = new Player[players.Count];
        int i = 0;
        foreach (var item in players.Values)
        {
            allPlayers[i] = item;
            i++;
        }

        return allPlayers;
    }


}

