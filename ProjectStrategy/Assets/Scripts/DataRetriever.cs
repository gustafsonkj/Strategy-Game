using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class DataRetriever : MonoBehaviour {
    public string currentLevel;//get current level the player is on
    List<Building> buildings = new List<Building>();//get teams and hitpoints
    public static Game game;//get current day, current team
    //get the unit lists from the team script
    List<Unit> unitsA = game.Teams[0].Units;//team 1 units
    List<Unit> unitsB = game.Teams[1].Units;//team 2 units
    List<Building> buildingsA = game.Teams[0].Buildings;//team 1 buildings
    List<Building> buildingB = game.Teams[1].Buildings;//team 2 buildings
    public Team team;//get resources for each team
    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {

	}
}
