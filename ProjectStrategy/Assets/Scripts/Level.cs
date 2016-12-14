using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    public int levelNumber;
    public int offset;

    public List<List<Tile>> Tiles;

    public Rect Bounds = new Rect();

    public void ForceStart()
    {
        Start();
    }
    // Use this for initialization
    void Start()
    {
		
        // Setup Tile Selection
        Transform TileObjects = this.gameObject.transform.FindChild("Tiles");

        // Get Dimensions
        for (int i = 0; i < TileObjects.childCount; i++)
        {
            Vector3 pos = TileObjects.GetChild(i).gameObject.transform.position;

            if (pos.x < Bounds.x)
                Bounds.x = pos.x;
            else if (pos.x > Bounds.width)
                Bounds.width = pos.x;

            if (pos.z < Bounds.y)
                Bounds.y = pos.z;
            else if (pos.z > Bounds.height)
                Bounds.height = pos.z;
        }
        // Setup 2D Array
        if (levelNumber == 1)
        {
            Tiles = new List<List<Tile>>();
            for (int y = 0; y < Bounds.height + 1; y++)
            {
                Tiles.Add(new List<Tile>());
                for (int x = 0; x < Bounds.width + 1; x++)
                    Tiles[y].Add(null);
            }
        }
        else if (levelNumber==2)
        {
            Tiles = new List<List<Tile>>();
            for (int y = 0; y < 75; y++) // X-axis
            {
                Tiles.Add(new List<Tile>()); // Z-Axis
                for (int x = 0; x < 75; x++)
                    Tiles[y].Add(null);
            }
        }
        else if (levelNumber == 3)
        {
            Tiles = new List<List<Tile>>();
            for (int y = 0; y < 41; y++) // X-axis
            {
                Tiles.Add(new List<Tile>()); // Z-Axis
                for (int x = 0; x < 41; x++)
                    Tiles[y].Add(null);
            }
        }
        // Add Tiles into Array
        for (int i = 0; i < TileObjects.childCount; i++)
        {
            Vector3 pos = TileObjects.GetChild(i).gameObject.transform.position;
            if (Tiles[Mathf.RoundToInt(pos.z)+offset][Mathf.RoundToInt(pos.x)+offset] != null)
                continue;
            Tiles[Mathf.RoundToInt(pos.z)+offset][Mathf.RoundToInt(pos.x)+offset] = TileObjects.GetChild(i).gameObject.GetComponent<Tile>();
           
            //The titles of the tile objects inside "Tiles" have the numbers reversed. Example: transform.position of Tile_Ground 0,1 is ACTUALLY Z1, X0, despite the format of the title of the tile being z, x
            //Debug.Log(Tiles[Mathf.RoundToInt(pos.z)][Mathf.RoundToInt(pos.x)]);
        }

        // Let Tiles know about buildings that are on top of them
        Transform Buildings = this.gameObject.transform.FindChild("Buildings");
        for (int i = 0; i < Buildings.childCount; i++)
        {
            Building building = Buildings.GetChild(i).GetComponent<Building>();
            GetTile(building.TilePosition()).BuildingOnTop = building;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public Tile GetTile(Point tilePosition) // Zac Lindsey
    {
        return Tiles[tilePosition.y + offset][tilePosition.x + offset];
    }
    /*public Tile GetTile(int x, int y) {return Tiles[y+offset][x+offset]; }*/
    public Tile GetTile(int x, int y)
    {
        
        return GetTile(new Point(x, y));
    }

    public bool ValidTile(int x, int y) // Modified -ZL
    {
        if (levelNumber == 1 || levelNumber == 3)
            return x >= 0 && y >= 0 && x <= Bounds.width && y <= Bounds.height;
        else
            return true;
    }
    public bool ValidTile(Point tilePosition) { return ValidTile(tilePosition.x, tilePosition.y); }

    /* //Zac Lindsey
    public IEnumerable<Point> TilePositionsWithinRange(Point tile, float range)
    {
        int r = (int)range;
        for (int y = tile.y-r; y < tile.y+r; y++)
        {
            for (int x = tile.x-r; x < tile.x+r; x++)
            {
                if (ValidTile(x, y))
                    yield return new Point(x, y);
            }
        }
    }*/

    public IEnumerable<Point> AllTilePositions() // Zac Lindsey
    {
        List<Point> validPoints = new List<Point>();
        foreach (List<Tile> lt in Tiles)
        {
            foreach (Tile t in lt)
            {
                if (t!= null)
                {
                    validPoints.Add(new Point(t.TilePosition().x, t.TilePosition().y));
                }
            }
        }
        foreach (Point validPoint in validPoints)
            yield return new Point(validPoint.x, validPoint.y);
    }

    /* //Old
    public IEnumerable<Point> AllTilePositions()
    {
        for (int y = 0; y < Tiles.Count; y++)
        {
            for (int x = 0; x < Tiles[y].Count; x++)
            {
                yield return new Point(x, y);
            }
        }
    }*/
}
