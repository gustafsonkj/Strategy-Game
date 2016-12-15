using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LevelMaker : MonoBehaviour {

    public Transform Tiles;
    public Transform Buildings;
	public GameObject Tile_Empty;
    public GameObject Tile_RGB;
    public GameObject Tile_R;
    public GameObject Tile_G;
    public GameObject Tile_B;
    public GameObject Tile_RG;
    public GameObject Tile_RB;
    public GameObject Tile_GB;
    public GameObject Tile_Boost;
    public GameObject Generator;
    public GameObject Resource;
    public GameObject EnergyCenter;

	// Use this for initialization
	void Start () {
        //Get Level Data
        StreamReader reader = new StreamReader(File.OpenRead(Application.dataPath+"/Zac/Level3Data.csv"));
        List<List<int>> map = new List<List<int>>();
        for (int i = 0; !reader.EndOfStream; i++)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');
            map.Add(new List<int>());
            foreach (string value in values)
            {
                int output;
                if (System.Int32.TryParse(value, out output))
                    map[i].Add(output);
            }
        }
        //Generate Map
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                switch (map[i][j])
                {
				case 19: Instantiate(Tile_Empty, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles); break;
                    case 0: Instantiate(Tile_RGB, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles); break;
                    case 1: Instantiate(Tile_RG, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles); break;
                    case 2: Instantiate(Tile_RB, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles); break;
                    case 3: Instantiate(Tile_GB, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles); break;
                    case 4: Instantiate(Tile_R, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles); break;
                    case 5: Instantiate(Tile_G, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles); break;
                    case 6: Instantiate(Tile_B, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles); break;
                    case 7: Instantiate(Tile_Boost, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles); break;
                    case 8:
                        Instantiate(Tile_RGB, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles);
                        Instantiate(Generator, new Vector3((float)i, 1.0f, (float)j), new Quaternion(), Buildings);
                        break;
                    case 9:
                        Instantiate(Tile_RGB, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles);
                        Instantiate(Resource, new Vector3((float)i, 1.0f, (float)j), new Quaternion(), Buildings);
                        break;
                    case 10:
                        Instantiate(Tile_RGB, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles);
                        GameObject g = Instantiate(EnergyCenter, new Vector3((float)i, 1.0f, (float)j), new Quaternion(), Buildings) as GameObject;
                        g.GetComponent<Building>().Team = 1;
                        break;
                    case 11:
                        Instantiate(Tile_RGB, new Vector3((float)i, 0, (float)j), new Quaternion(), Tiles);
                        GameObject g2 = Instantiate(EnergyCenter, new Vector3((float)i, 1.0f, (float)j), new Quaternion(), Buildings) as GameObject;
                        g2.GetComponent<Building>().Team = 2;
                        break;
                    default: break;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
