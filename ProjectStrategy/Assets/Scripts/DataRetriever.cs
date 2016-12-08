using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class DataRetriever : MonoBehaviour {
    public string currentLevel;//get current level the player is on
    List<Building> buildings = new List<Building>();//get teams and hitpoints
    public Game game;//get current day, current team
    List<Unit> units = new List<Unit>();//get locations, teams, colors, types, and health/strength
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
