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
    public static List<int> unitColorsTeam1;
    public static List<int> unitColorsTeam2;
    public static List<float> unitStrengthsTeam1;
    public static List<float> unitStrengthsTeam2;
    public static List<int> buildHPTeam1;
    public static List<int> buildHPTeam2;
    public static List<int> buildTypesTeam1;
    public static List<int> buildTypesTeam2;

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
            unitTypesTeam1.Add(u.Type);
            unitColorsTeam1.Add(u.UnitColor);
            unitStrengthsTeam1.Add(u.GetHitPoints());
        }
        foreach (Unit u in unitsB)
        {
            unitPositionsTeam2.Add(u.transform.position);
            unitTypesTeam2.Add(u.Type);
            unitColorsTeam2.Add(u.UnitColor);
            unitStrengthsTeam2.Add(u.GetHitPoints());
        }
        foreach(Building b in buildingsA)
        {
            buildHPTeam1.Add(b.GetHitPoints());
            buildTypesTeam1.Add(b.Type);
        }
        foreach(Building b in buildingsB)
        {
            buildHPTeam2.Add(b.GetHitPoints());
            buildTypesTeam2.Add(b.Type);
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
    public List<Vector3> unitPositionsTeam1 = DataRetriever.unitPositionsTeam1;
    public List<Vector3> unitPositionsTeam2 = DataRetriever.unitPositionsTeam2;
    public List<int> unitTypesTeam1 = DataRetriever.unitTypesTeam1;
    public List<int> unitTypesTeam2 = DataRetriever.unitTypesTeam2;
    public List<int> unitColorsTeam1 = DataRetriever.unitColorsTeam1;
    public List<int> unitColorsTeam2 = DataRetriever.unitColorsTeam2;
    public List<float> unitStrengthsTeam1 = DataRetriever.unitStrengthsTeam1;
    public List<float> unitStrengthsTeam2 = DataRetriever.unitStrengthsTeam2;
    public List<int> buildHPTeam1 = DataRetriever.buildHPTeam1;
    public List<int> buildHPTeam2 = DataRetriever.buildHPTeam2;
    public List<int> buildTypesTeam1 = DataRetriever.buildTypesTeam1;
    public List<int> buildTypesTeam2 = DataRetriever.buildTypesTeam2;
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
