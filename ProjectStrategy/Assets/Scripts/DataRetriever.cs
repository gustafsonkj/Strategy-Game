using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class DataRetriever : MonoBehaviour {
    public string currentLevel;//get current level the player is on
    List<Building> buildings = new List<Building>();//get teams and hitpoints
    protected Game Game;//get current day, current team
    //get the unit lists from the team script
    List<Unit> unitsA;//team 1 units
    List<Unit> unitsB;//team 2 units
    List<Building> buildingsA;//team 1 buildings
    List<Building> buildingsB;//team 2 buildings
    public Team team;//get resources for each team
    public int teamARes;
    public int teamBRes;
    // Use this for initialization
    void Start ()
    {
        
	}

    public void Init()
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
    }

    // Update is called once per frame
    
	
	// Update is called once per frame
	void Update ()
    {

	}
}
