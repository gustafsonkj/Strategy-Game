using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{

    protected Game Game;

    public Building BuildingOnTop;
    private Transform Arrow;
    private int ArrowCount;

    // Distance from current Unit
    public int DistanceSteps = 10000;
    public bool PartOfCurrentPath = false;
    public bool InRange = false;

    public int Type = 0;

    public const int GRASS = 1;
    public const int ROAD = 2;
    public const int WATER = 3;
    public const int RAMP = 4;
    public const int BRIDGE = 5;

    public const int SPECIAL = 7;
    public const int RGB = 0;
    public const int RG = 1;
    public const int RB = 2;
    public const int GB = 3;
    public const int R = 4;
    public const int G = 5;
    public const int B = 6;

    // Use this for initialization
    void Start()
    {
        Game = GameObject.Find("Game").GetComponent<Game>();
    }

    void OnMouseDown()
    {
        if (Game.HUD.InMenu())
            return;
        if (Game.Selector.CurrentUnit != null && Game.Selector.CurrentUnit.IsWaitingForActionAccept())
            return;

        if (Game.Selector.CurrentUnit != null && CanWalkOn())
        {
            Game.Selector.CurrentUnit.MoveToTile(TilePosition());
        }
        else if (Game.Selector.CurrentBuilding != null)
        {
            Game.Selector.UnselectCurrentBuilding();
        }
        else if (Game.Selector.CurrentUnit == null)
        {
            Game.HUD.ActionPopup.SetItems("Options", "End");
            Game.HUD.ActionPopup.Show(true);
        }
    }
    void OnMouseEnter()
    {
        if (Game.HUD.ActionPopup.Visible)
            return;

        Game.Selector.SelectTile(transform);
    }

    public void Select()
    {
        if (BuildingOnTop)
            Game.HUD.SetTileInfo(BuildingOnTop.GetTypeName(), BuildingOnTop.Team, BuildingOnTop.GetHitPoints());
        else
            Game.HUD.SetTileInfo(GetTypeName());

        TintAsSelected();
    }

    public void OnUnitEnter(Unit unit)
    {
        if (BuildingOnTop != null)
            BuildingOnTop.OnUnitEnter(unit);
    }
    public void OnUnitLeave()
    {
        if (BuildingOnTop != null)
            BuildingOnTop.OnUnitLeave();
    }


    public void SetAsPath(bool firstTile, Point prevTile, bool lastTile = true, Point nextTile = default(Point))
    {
        PartOfCurrentPath = true;

        AddPathArrow(firstTile, prevTile, lastTile, nextTile);

        //Tint(Color.cyan);
    }
    public void AddPathArrow(bool firstTile, Point prevTile, bool lastTile = true, Point nextTile = default(Point))
    {
        if (Arrow != null)
            RemovePathArrow();

        Transform arrow = Game.Arrow_Line;
        float angle = 0;
        Point tilePos = TilePosition();

        if (lastTile)
        {
            // Arrow End
            arrow = Game.Arrow_End;
            if (prevTile.y > tilePos.y)
                angle = 180;
            else if (prevTile.x > tilePos.x)
                angle = -90;
            else if (prevTile.x < tilePos.x)
                angle = 90;
        }
        else if (!firstTile && prevTile.x != nextTile.x && prevTile.y != nextTile.y)
        {
            // Arrow Corner
            arrow = Game.Arrow_Corner;

            if ((prevTile.x < tilePos.x && prevTile.y == tilePos.y && nextTile.x == tilePos.x && nextTile.y < tilePos.y) ||
                (prevTile.x == tilePos.x && prevTile.y < tilePos.y && nextTile.x == tilePos.x && nextTile.y == tilePos.y) ||
                (prevTile.x == tilePos.x && prevTile.y < tilePos.y && nextTile.x < tilePos.x && nextTile.y == tilePos.y))
                angle = 90;
            else if ((prevTile.x < tilePos.x && prevTile.y == tilePos.y && nextTile.x == tilePos.x && nextTile.y > tilePos.y) ||
                     (prevTile.x == tilePos.x && prevTile.y > tilePos.y && nextTile.x < tilePos.x && nextTile.y == tilePos.y))
                angle = 180;
            else if ((prevTile.x > tilePos.x && prevTile.y == tilePos.y && nextTile.x == tilePos.x && nextTile.y > tilePos.y) ||
                     (prevTile.x == tilePos.x && prevTile.y > tilePos.y && nextTile.x > tilePos.x && nextTile.y == tilePos.y))
                angle = 270;
        }
        else
        {
            // Arrow Line
            if (tilePos.y == nextTile.y && tilePos.x != nextTile.x)
                angle = 90;
        }

        Arrow = Instantiate(arrow, transform.position + new Vector3(0, 0.55f, 0), Quaternion.Euler(90, angle, 0)) as Transform;
    }
    public void RemovePathArrow()
    {
        if (Arrow == null)
            return;

        GameObject.Destroy(Arrow.gameObject);
        Arrow = null;
    }


    public void TintAsInRange(int Team=1)
    {
        InRange = true;

        if (Team == 1)
            Tint(Color.yellow, 1); // second parameter set to true to brighten
        else
            Tint(Color.magenta, 1);
    }
    public void TintAsSelected()
    {
        Color c;
        if (GetComponent<Renderer>() != null)
            c = GetComponent<Renderer>().material.color;
        else
            c = transform.GetChild(0).GetComponent<Renderer>().material.color;
        Tint(c * 2.0f,1);
    }
    public void UnTint(int Team=1)
    {
        if (PartOfCurrentPath)
        {
            //SetAsPath();
            return;
        }
        else if (InRange)
        {
            TintAsInRange(Team);
            return;
        }
        Tint(Color.white, 2); //2 used to reduce emission
    }

    private void Tint(Color color, int brighten=0) // Tint 2.0 - Zac Lindsey
    {
        // brighten == 0: no brighten
        // brighten == 1: brighten
        // brighten == 2: unbrighten 
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            if (r.tag == "Top")
            {
                r.material.SetColor("_Color", color);

                if (brighten==1)
                    r.material.SetColor("_EmissionColor", color*0.25f);

                if (brighten==2)
                    r.material.SetColor("_EmissionColor", color*0.0f);
            }
        }
    }

    /*private void Tint(Color color) // This tint caused performance issues - ZL
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Renderer>() != null)
                transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_Color", color);
            else
            {
                for (int p = 0; p < transform.GetChild(i).childCount; p++)
                    transform.GetChild(i).transform.GetChild(p).GetComponent<Renderer>().material.SetColor("_Color", color);
            }
        }
        if (GetComponent<Renderer>() != null)
            GetComponent<Renderer>().material.SetColor("_Color", color);
    }*/



    public void ClearPathFindingInfo(int Team)
    {
        DistanceSteps = 1000;
        PartOfCurrentPath = false;
        RemovePathArrow();
        UnTint(Team);
    }
    public bool ValidPath() { return CanWalkOn() && !PartOfCurrentPath; }

    //public bool CanWalkOn() { return Type != WATER && Type != RAMP; }

    public bool CanWalkOn()
    {
        switch (Game.Selector.CurrentUnit.UnitColor)
        {
            case 0: return Type == R || Type == RG || Type == RB || Type == RGB || Type == SPECIAL;
            case 1: return Type == G || Type == RG || Type == GB || Type == RGB || Type == SPECIAL;
            case 2: return Type == B || Type == RB || Type == GB || Type == RGB || Type == SPECIAL;
            default:
                return true;
        }
    }

    public Point TilePosition() { return new Point(Mathf.RoundToInt(this.gameObject.transform.position.x), Mathf.RoundToInt(this.gameObject.transform.position.z)); }

    public string GetTypeName()
    {
        switch (Type)
        {
            case R: return "Red";
            case G: return "Green";
            case B: return "Blue";
            case RG: return "Red-Green";
            case RB: return "Red-Blue";
            case GB: return "Green-Blue";
            case RGB: return "RGB";
            case SPECIAL: return "Special";
        }
        return "";
    }
}
