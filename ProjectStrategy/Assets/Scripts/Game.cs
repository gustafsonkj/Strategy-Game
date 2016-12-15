﻿using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{

    public UnitSelector Selector;
    public Level Level;
    public HeadsUpDisplay HUD;
    public GameObject DataRetriever;
    public Camera Camera;
    public Transform Units;
    public Transform Buildings;
    public int CurrentTeam = 1;
    public List<Team> Teams = new List<Team> { new Team(), new Team() };

    public int Day = 1;

    public Transform Unit_Tank;
	public Transform Unit_TankA;
	public Transform Unit_TankB;

    public Transform Unit_Basic;
    public Transform Unit_Ranged;
    public Transform Unit_TheQuacker;
    public Transform Unit_HarvesterA;
    public Transform Unit_HarvesterB;

    public Transform Effect_Explosion;
    public Transform Arrow_Line;
    public Transform Arrow_Corner;
    public Transform Arrow_Corner2;
    public Transform Arrow_End;

    // Use this for initialization
    void Start()
    {
        
        Selector = GameObject.Find("Selector").GetComponent<UnitSelector>();
        Level = GameObject.Find("Level").GetComponent<Level>();
        HUD = GameObject.Find("GUI").GetComponent<HeadsUpDisplay>();
        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        Units = GameObject.Find("Units").transform;
        Buildings = GameObject.Find("Buildings").transform;

        Teams[0].TeamNo = 1;
        Teams[1].TeamNo = 2;

        if (saver.wasCalled)
        {
            Level.ForceStart(); // I hate this line. - ZL
            foreach (Unit u in Units.GetComponentsInChildren<Unit>())
                u.ForceStart();
            saver.loadGameValues();
            Day = saver.getDay();
            CurrentTeam = saver.getCurrentTeam();
            Teams[0].Resources = saver.getTeam1Res();
            Teams[1].Resources = saver.getTeam2Res();
        }

        Teams[0].ResetUnits();
        Teams[1].ResetUnits();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StopGame()
    {
        GameObject.Destroy(Selector.gameObject);

        GameObject.DestroyObject(HUD);

        for (int y = 0; y < Level.Tiles.Count; y++)
        {
            for (int x = 0; x < Level.Tiles[y].Count; x++)
                GameObject.Destroy(Level.Tiles[y][x]);
        }

        for (int i = 0; i < Units.childCount; i++)
            GameObject.Destroy(Units.GetChild(i).GetComponent<Unit>());

        for (int i = 0; i < Buildings.childCount; i++)
            GameObject.Destroy(Buildings.GetChild(i).GetComponent<Building>());
    }

    public void EndTurn()
    {
        GetCurrentTeam().ResetUnits();
        GetCurrentTeam().GainIncome();

        CurrentTeam = CurrentTeam >= Teams.Count ? 1 : CurrentTeam + 1;

        GetCurrentTeam().HealUnitsInCities();
        GetCurrentTeam().HealUncontestedBuildings();
        GetCurrentTeam().ResetUnits();

        HUD.SetTeam(CurrentTeam, GetCurrentTeam().Resources);

        // Next Day?
        if (CurrentTeam == 1)
            Day++;

        HUD.ShowDayNo(Day);
    }

    public void CheckWinLoseConditions(int BuildingType, int newTeam)
    {
        // Energy center is captured: game automatically stops
        if (BuildingType==2)
        {
            HUD.ShowTeamWomMessage(newTeam);
            StopGame();
        }

        // Energy center is not captured: see if any units remain
        // And if they have at least one building
        if (Teams[0].Units.Count == 0 && Teams[0].Buildings.Count <= 1)
        {
            HUD.ShowTeamWomMessage(2);
            StopGame();
        }
        if (Teams[1].Units.Count == 0 && Teams[1].Buildings.Count <= 1)
        {
            HUD.ShowTeamWomMessage(1);
            StopGame();
        }
    }

    public Team GetCurrentTeam() { return Teams[CurrentTeam - 1]; }

    public List<Unit> GetEnemyUnits()
    {
        List<Unit> units = new List<Unit>();
        for (int t = 0; t < Teams.Count; t++)
        {
            if (t == CurrentTeam - 1)
                continue;

            units.AddRange(Teams[t].Units);
        }

        return units;
    }
}
