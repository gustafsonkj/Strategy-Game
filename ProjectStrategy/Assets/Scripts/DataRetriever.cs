using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


public class DataRetriever : MonoBehaviour {
    public static Scene currentLevel;//get current level the player is on
    protected static Game Game;//get current day, current team
    //get the unit lists from the team script
    public static List<Unit> unitsA;//team 1 units
    public static List<Unit> unitsB;//team 2 units
    public static List<Building> buildingsA;//team 1 buildings
    public static List<Building> buildingsB;//team 2 buildings
    public static int teamARes;
    public static int teamBRes;
    public static int Day;
    public static Team currentTeam;

    // Use this for initialization
    void Start ()
    {
        //saveAllData();
	}

    public static void saveAllData()
    {
        if (Game != null)
            return;
        Game = GameObject.Find("Game").GetComponent<Game>();
        unitsA = Game.Teams[0].Units;
        unitsB = Game.Teams[1].Units;
        buildingsA = Game.Teams[0].Buildings;
        buildingsB = Game.Teams[1].Buildings;
        teamARes = Game.Teams[0].Resources;
        teamBRes = Game.Teams[1].Resources;
        Day = Game.Day;
        currentTeam = Game.GetCurrentTeam();
        currentLevel = SceneManager.GetActiveScene();
    }
}

[System.Serializable]
public class AllMyData
{
    private Scene currentLevel;
    private List<Unit> unitsTeam1;
    private List<Unit> unitsTeam2;
    private List<Building> buildingsTeam1;
    private List<Building> buildingsTeam2;
    private int team1Resources;
    private int team2Resources;
    private int currentDay;
    private Team currentTeam;

    public void saveDatStuff()
    {
        currentLevel = DataRetriever.currentLevel;
        currentTeam = DataRetriever.currentTeam;
        currentDay = DataRetriever.Day;
        team1Resources = DataRetriever.teamARes;
        team2Resources = DataRetriever.teamBRes;
        unitsTeam1 = DataRetriever.unitsA;
        unitsTeam2 = DataRetriever.unitsB;
        buildingsTeam1 = DataRetriever.buildingsA;
        buildingsTeam2 = DataRetriever.buildingsB;
    }
}

public class saver : MonoBehaviour
{
    public static void saveGame()
    {
        if (File.Exists(Application.persistentDataPath + "strategygamesave"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            AllMyData amd = new AllMyData();
            amd.saveDatStuff();
            FileStream fs = File.Open(Application.persistentDataPath + "strategygamesave", FileMode.Open);
            bf.Serialize(fs, amd);
            fs.Close();
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            AllMyData amd = new AllMyData();
            amd.saveDatStuff();
            FileStream fs = File.Create(Application.persistentDataPath + "strategygamesave");
            bf.Serialize(fs, amd);
            fs.Close();
        }
    }

    public static void loadGame()
    {

    }
}
