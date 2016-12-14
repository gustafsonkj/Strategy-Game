﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;




public class DataRetriever : MonoBehaviour
{
    public static Scene currentLevel1;//get current level the player is on
    protected static Game Game;//get current day, current team
    //get the unit lists from the team script
    public static int teamARes = 0;
    public static int teamBRes = 0;
    public static int Day = 0;
    public static int currentTeam = 0;
    public static int currentLevel = 0;
    public static List<Vector3> unitPositionsTeam1 = new List<Vector3>();
    public static List<Vector3> unitPositionsTeam2 = new List<Vector3>();
    public static List<Vector3> buildPosT1 = new List<Vector3>();
    public static List<Vector3> buildPosT2 = new List<Vector3>();
    public static List<float> unitPosX1 = new List<float>();//unit positions
    public static List<float> unitPosX2 = new List<float>();
    public static List<float> unitPosY1 = new List<float>();
    public static List<float> unitPosY2 = new List<float>();
    public static List<float> unitPosZ1 = new List<float>();
    public static List<float> unitPosZ2 = new List<float>();
    public static List<float> buildPosX1 = new List<float>();//build positions
    public static List<float> buildPosX2 = new List<float>();
    public static List<float> buildPosY1 = new List<float>();
    public static List<float> buildPosY2 = new List<float>();
    public static List<float> buildPosZ1 = new List<float>();
    public static List<float> buildPosZ2 = new List<float>();
    public static List<int> unitTypesTeam1 = new List<int>();
    public static List<int> unitTypesTeam2 = new List<int>();
    public static List<int> unitColorsTeam1 = new List<int>();
    public static List<int> unitColorsTeam2 = new List<int>();
    public static List<float> unitStrengthsTeam1 = new List<float>();
    public static List<float> unitStrengthsTeam2 = new List<float>();
    public static List<int> buildHPTeam1 = new List<int>();
    public static List<int> buildHPTeam2 = new List<int>();
    public static List<int> buildTypesTeam1 = new List<int>();
    public static List<int> buildTypesTeam2 = new List<int>();

    public static void saveAllData()
    {
        Game = GameObject.Find("Game").GetComponent<Game>();
        Day = Game.Day;


        currentTeam = Game.CurrentTeam;
        currentLevel = SceneManager.GetActiveScene().buildIndex;


        Unit tempUnit;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Unit"))
        {
            tempUnit = go.GetComponent<Unit>();
            if (tempUnit.Team == 1)
            {
                unitPositionsTeam1.Add(new Vector3(go.transform.position.x,
                                                    go.transform.position.y,
                                                    go.transform.position.z
                ));
                unitTypesTeam1.Add(tempUnit.Type);
                unitColorsTeam1.Add(tempUnit.UnitColor);
                unitStrengthsTeam1.Add(tempUnit.GetHitPoints());
            }
            else if (tempUnit.Team == 2)
            {
                unitPositionsTeam2.Add(new Vector3(go.transform.position.x,
                                                    go.transform.position.y,
                                                    go.transform.position.z
                ));
                unitTypesTeam2.Add(tempUnit.Type);
                unitColorsTeam2.Add(tempUnit.UnitColor);
                unitStrengthsTeam2.Add(tempUnit.GetHitPoints());
            }
        }


        Building tempBuild;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Building"))
        {
            tempBuild = go.GetComponent<Building>();
            if (tempBuild.Team == 1)
            {
                buildHPTeam1.Add(tempBuild.GetHitPoints());
                buildTypesTeam1.Add(tempBuild.Type);
                buildPosT1.Add(new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z));
            }
            else if (tempBuild.Team == 2)
            {
                buildHPTeam2.Add(tempBuild.GetHitPoints());
                buildTypesTeam2.Add(tempBuild.Type);
                buildPosT2.Add(new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z));
            }
        }
        foreach (Vector3 v in unitPositionsTeam1)
        {
            unitPosX1.Add(v.x);
            unitPosY1.Add(v.y);
            unitPosZ1.Add(v.z);
        }
        foreach (Vector3 v in unitPositionsTeam2)
        {
            unitPosX2.Add(v.x);
            unitPosY2.Add(v.y);
            unitPosZ2.Add(v.z);
        }
        foreach (Vector3 v in buildPosT1)
        {
            buildPosX1.Add(v.x);
            buildPosY1.Add(v.y);
            buildPosZ1.Add(v.z);
        }
        foreach (Vector3 v in buildPosT2)
        {
            buildPosX2.Add(v.x);
            buildPosY2.Add(v.y);
            buildPosZ2.Add(v.z);
        }
    }
}


[System.Serializable]
public class AllMyData
{
    public int currentLevel = DataRetriever.currentLevel;//loadscene
    public int currentTeam = DataRetriever.currentTeam;//game.currentteam
    public int currentDay = DataRetriever.Day;//game.day
    public int team1Resources = DataRetriever.teamARes;//game.teams[0].resources
    public int team2Resources = DataRetriever.teamBRes;//game.teams[1].resources
    public List<float> uPX1 = DataRetriever.unitPosX1;//unit position x team 1
    public List<float> uPY1 = DataRetriever.unitPosY1;//y team 1
    public List<float> uPZ1 = DataRetriever.unitPosZ1;//z team 1
    public List<float> uPX2 = DataRetriever.unitPosX2;//x team 2
    public List<float> uPY2 = DataRetriever.unitPosY2;//y team 2
    public List<float> uPZ2 = DataRetriever.unitPosZ2;//z team 2
    public List<float> bPX1 = DataRetriever.buildPosX1;//building position x team 1
    public List<float> bPY1 = DataRetriever.buildPosY1;//building position y team 1
    public List<float> bPZ1 = DataRetriever.buildPosZ1;//building position z team 1
    public List<float> bPX2 = DataRetriever.buildPosX2;//building position x team 2
    public List<float> bPY2 = DataRetriever.buildPosY2;//building position y team 2
    public List<float> bPZ2 = DataRetriever.buildPosZ2;//building position z team 2
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
        Debug.Log(Application.persistentDataPath);
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
        Debug.Log("You got to loadGame()!");
        Transform t;
        Unit u;
        if (File.Exists(Application.persistentDataPath + "/strategygame.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/strategygame.save", FileMode.Open);
            AllMyData amd = (AllMyData)bf.Deserialize(fs);
            Game game = GameObject.Find("Game").GetComponent<Game>();
            SceneManager.LoadScene(amd.currentLevel);
            game.Day = amd.currentDay;
            game.CurrentTeam = amd.currentTeam;
            game.Teams[0].Resources = amd.team1Resources;
            game.Teams[1].Resources = amd.team2Resources;
            //destroy ALL existing units in scene
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Unit"))
            {
                g.GetComponent<Unit>().Remove();
            }
            //team 1 units instantiation
            for (int i = 0; i < amd.unitTypesTeam1.Count; i++)
            {
                switch (amd.unitTypesTeam1[i])
                {
                    case 0:
                        t = Instantiate(game.Unit_Basic, new Vector3(amd.uPX1[i], amd.uPY1[i], amd.uPZ1[i]), Quaternion.identity) as Transform;
                        t.parent = GameObject.Find("Units").transform;
                        u = t.GetComponent<Unit>();
                        u.Init();
                        u.SetTeam(1);
                        u.AcceptMove();
                        u.UnitColor = amd.unitColorsTeam1[i];
                        u.Type = amd.unitTypesTeam1[i];
                        u.HitPoints = amd.unitStrengthsTeam1[i];
                        break;
                    case 1:
                        t = Instantiate(game.Unit_Ranged, new Vector3(amd.uPX1[i], amd.uPY1[i], amd.uPZ1[i]), Quaternion.identity) as Transform;
                        t.parent = GameObject.Find("Units").transform;
                        u = t.GetComponent<Unit>();
                        u.Init();
                        u.SetTeam(1);
                        u.AcceptMove();
                        u.UnitColor = amd.unitColorsTeam1[i];
                        u.Type = amd.unitTypesTeam1[i];
                        u.HitPoints = amd.unitStrengthsTeam1[i];
                        break;
                    case 2:
                        t = Instantiate(game.Unit_HarvesterA, new Vector3(amd.uPX1[i], amd.uPY1[i], amd.uPZ1[i]), Quaternion.identity) as Transform;
                        t.parent = GameObject.Find("Units").transform;
                        u = t.GetComponent<Unit>();
                        u.Init();
                        u.SetTeam(1);
                        u.AcceptMove();
                        u.UnitColor = amd.unitColorsTeam1[i];
                        u.Type = amd.unitTypesTeam1[i];
                        u.HitPoints = amd.unitStrengthsTeam1[i];
                        break;
                    case 3:
                        t = Instantiate(game.Unit_TheQuacker, new Vector3(amd.uPX1[i], amd.uPY1[i], amd.uPZ1[i]), Quaternion.identity) as Transform;
                        t.parent = GameObject.Find("Units").transform;
                        u = t.GetComponent<Unit>();
                        u.Init();
                        u.SetTeam(1);
                        u.AcceptMove();
                        u.UnitColor = amd.unitColorsTeam1[i];
                        u.Type = amd.unitTypesTeam1[i];
                        u.HitPoints = amd.unitStrengthsTeam1[i];
                        break;
                    default: break;
                }
            }
            //team 2 unit instantiation
            for (int i = 0; i < amd.unitTypesTeam2.Count; i++)
            {
                switch (amd.unitTypesTeam2[i])
                {
                    case 0:
                        t = Instantiate(game.Unit_Basic, new Vector3(amd.uPX2[i], amd.uPY2[i], amd.uPZ2[i]), Quaternion.identity) as Transform;
                        t.parent = GameObject.Find("Units").transform;
                        u = t.GetComponent<Unit>();
                        u.Init();
                        u.SetTeam(2);
                        u.AcceptMove();
                        u.UnitColor = amd.unitColorsTeam1[i];
                        u.Type = amd.unitTypesTeam1[i];
                        u.HitPoints = amd.unitStrengthsTeam1[i];
                        break;
                    case 1:
                        t = Instantiate(game.Unit_Ranged, new Vector3(amd.uPX2[i], amd.uPY2[i], amd.uPZ2[i]), Quaternion.identity) as Transform;
                        t.parent = GameObject.Find("Units").transform;
                        u = t.GetComponent<Unit>();
                        u.Init();
                        u.SetTeam(2);
                        u.AcceptMove();
                        u.UnitColor = amd.unitColorsTeam1[i];
                        u.Type = amd.unitTypesTeam1[i];
                        u.HitPoints = amd.unitStrengthsTeam1[i];
                        break;
                    case 2:
                        t = Instantiate(game.Unit_HarvesterA, new Vector3(amd.uPX2[i], amd.uPY2[i], amd.uPZ2[i]), Quaternion.identity) as Transform;
                        t.parent = GameObject.Find("Units").transform;
                        u = t.GetComponent<Unit>();
                        u.Init();
                        u.SetTeam(2);
                        u.AcceptMove();
                        u.UnitColor = amd.unitColorsTeam1[i];
                        u.Type = amd.unitTypesTeam1[i];
                        u.HitPoints = amd.unitStrengthsTeam1[i];
                        break;
                    case 3:
                        t = Instantiate(game.Unit_TheQuacker, new Vector3(amd.uPX2[i], amd.uPY2[i], amd.uPZ2[i]), Quaternion.identity) as Transform;
                        t.parent = GameObject.Find("Units").transform;
                        u = t.GetComponent<Unit>();
                        u.Init();
                        u.SetTeam(2);
                        u.AcceptMove();
                        u.UnitColor = amd.unitColorsTeam1[i];
                        u.Type = amd.unitTypesTeam1[i];
                        u.HitPoints = amd.unitStrengthsTeam1[i];
                        break;
                    default: break;
                }
            }
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Building"))
            {
                for (int i = 0; i < GameObject.FindGameObjectsWithTag("Building").Length; i++)
                {


                }
                for (int i = 0; i < GameObject.FindGameObjectsWithTag("Building").Length; i++)
                {


                }
            }
        }
    }
}