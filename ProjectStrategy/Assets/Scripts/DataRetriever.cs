using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


public class DataRetriever : MonoBehaviour {
    public static Scene currentLevel1;//get current level the player is on
    protected static Game Game;//get current day, current team
    //get the unit lists from the team script
    public static List<Unit> unitsA;//team 1 units
    public static List<Unit> unitsB;//team 2 units
    public static List<Building> buildingsA;//team 1 buildings
    public static List<Building> buildingsB;//team 2 buildings
    public static int teamARes;
    public static int teamBRes;
    public static int Day;
    public static int currentTeam;
    public static int currentLevel;
    public static List<Vector3> unitPositionsTeam1;
    public static List<Vector3> unitPositionsTeam2;
    public static List<int> unitTypesTeam1;
    public static List<int> unitTypesTeam2;

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
        currentTeam = Game.CurrentTeam;
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        foreach(Unit u in unitsA)
        {
            unitPositionsTeam1.Add(u.transform.position);
        }
        foreach (Unit u in unitsB)
        {
            unitPositionsTeam2.Add(u.transform.position);
        }
        foreach (Unit u in unitsA)
        {
            unitTypesTeam1.Add(u.Type);
        }
        foreach (Unit u in unitsB)
        {
            unitTypesTeam2.Add(u.Type);
        }
    }
}

[System.Serializable]
public class AllMyData
{
    public int currentLevel = DataRetriever.currentLevel;//loadscene
    public int currentTeam = DataRetriever.currentTeam;//game.currentteam?
    public int currentDay = DataRetriever.Day;//game.day
    public int team1Resources = DataRetriever.teamARes;//game.teams[0].resources
    public int team2Resources = DataRetriever.teamBRes;//game.teams[1].resources
    public List<Unit> unitsTeam1 = DataRetriever.unitsA;//game.teams[0].units
    public List<Unit> unitsTeam2 = DataRetriever.unitsB;//game.teams[1].units
    public List<Building> buildingsTeam1 = DataRetriever.buildingsA;//game.teams[0].buildings
    public List<Building> buildingsTeam2 = DataRetriever.buildingsB;//game.teams[1].buildings
}

public class saver : MonoBehaviour
{
    public static void saveGame()
    {
        if (File.Exists(Application.persistentDataPath + "/strategygame.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            AllMyData amd = new AllMyData();
            File.Delete(Application.persistentDataPath + "/strategygame.save");
            FileStream fs = File.Create(Application.persistentDataPath + "/strategygame.save");
            bf.Serialize(fs, amd);
            fs.Close();
        }
        else
        {
            BinaryFormatter bf = new BinaryFormatter();
            AllMyData amd = new AllMyData();
            FileStream fs = File.Create(Application.persistentDataPath + "/strategygame.save");
            bf.Serialize(fs, amd);
            fs.Close();
        }
    }

    public static void loadGame()
    {
        if (File.Exists(Application.persistentDataPath+"/strategygame.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/strategygame.save", FileMode.Open);
            AllMyData amd = (AllMyData)bf.Deserialize(fs);
            SceneManager.LoadScene(amd.currentLevel);

        }
    }
}
