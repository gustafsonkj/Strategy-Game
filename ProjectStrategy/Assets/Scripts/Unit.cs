using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Unit : MonoBehaviour
{
    protected Game Game;
    private bool Selected = false;
    private bool Moved = false;
    private bool WaitingForMoveAccept = false;
    private bool WaitingForActionAccept = false;
    public ParticleSystem system;
    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];

    public Building BuildingOn;

    private float Range;
    private float AttackRange;
    private float HitPoints = 10;
	public GameObject ranged;

    public int Team = 0;

    public int Type = 0;
    public int UnitColor = 0;

    public const int TANK = 1;

    private List<Unit> UnitsInAttackRange = new List<Unit>();
    private Unit CurrentAttackTarget;

    // Movement
    private Vector3 StartPosition;
    private Quaternion StartRotation;
    private List<Vector3> Waypoints = new List<Vector3>();
    private Point[] MoveDirections = new Point[]
    {
        new Point(0, -1),
        new Point(1, 0),
        new Point(0, 1),
        new Point(-1, 0)
    };

    // Animation
    private float Speed = 10;
    private float AnimBounceSpeed = 0.35f;
    private float AnimBounceDir = 1;
    private float ScaleY = 1;
    private float PosY;
    public float GetPosY() { return PosY; }
    private float ColorMultiplier = 1;

    // Sound
    public AudioClip Sound_Fire;

    // Use this for initialization
    void Start()
    {
        Init();
    }
    public void Init()
    {
        if (Game != null)
            return;

        Game = GameObject.Find("Game").GetComponent<Game>();

        PosY = transform.position.y;

        SetTeam(Team, true);

        switch (Type)
        {
            case 0: //Basic
                Range = 4;
                AttackRange = 1;
                break;
            case 1: //Ranged
                Range = 2;
                AttackRange = 4;
                break;
            case 2: //Harvester
                Range = 2;
                AttackRange = 1;
                break;
            default:
                break;
    }
        

        if (Game.Level != null)
            Game.Level.GetTile(TilePosition()).OnUnitEnter(this);
    }
    public void Remove()
    {
        Game.Level.GetTile(TilePosition()).OnUnitLeave();

        Game.Teams[Team - 1].Units.Remove(this);
        GameObject.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Waypoints
        if (Waypoints.Count != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, Waypoints[0], Speed * Time.deltaTime);
            transform.LookAt(Waypoints[0]);

            // Reached Waypoint
            if (transform.position.Equals(Waypoints[0]))
                Waypoints.RemoveAt(0);

            // Reached Destination
            if (Waypoints.Count == 0)
                OnReachedWaypoint();
        }
        // Animated on Selected
        else if (!Selected)
        {
            ScaleY += (AnimBounceSpeed * AnimBounceDir) * Time.deltaTime;
            if (AnimBounceDir == -1 && ScaleY <= 1)
            {
                ScaleY = 1;
                AnimBounceDir = 1;
            }
            else if (AnimBounceDir == 1 && ScaleY >= 1.1f)
            {
                ScaleY = 1.1f;
                AnimBounceDir = -1;
            }

            transform.localScale = new Vector3(1, ScaleY, 1);
            transform.position = new Vector3(transform.position.x, (PosY / 2) + (transform.localScale.y / 2), transform.position.z);
            transform.Rotate(0.0f, Time.deltaTime * 180.0f, 0.0f);
        }
    }

    void OnMouseDown()
    {
        if (Game.HUD.ActionPopup.Visible || IsMoving())
            return;

        // On Left-MouseDown
        if (!Selected)
        {
            //Debug.Log("SELECTING UNIT");
            Game.Selector.SelectUnit(this);
        }
        else if (!IsWaitingForActionAccept())
            OnReachedWaypoint();
    }
    void OnMouseEnter()
    {
        if (Game.HUD.ActionPopup.Visible)
            return;

        Game.Selector.SelectTile(TilePosition());

        // In attack range of current unit - select
        if (Game.Selector.CurrentUnit != null && Game.Selector.CurrentUnit.IsWaitingForMoveAccept() && Game.Selector.CurrentUnit.InAttackRangeList(this))
        {
            SetAttackTarget(this);
        }
    }

    public void Select()
    {
        Selected = true;

        ScaleY = 1;
        transform.localScale = new Vector3(1, ScaleY, 1);
        transform.position = new Vector3(transform.position.x, PosY, transform.position.z);

        StartPosition = transform.position;
        StartRotation = transform.rotation;

        HighlightTilesInRange();

        if (Game != null)
            Game.HUD.SetCurrentHelp(HeadsUpDisplay.PROMPT_RIGHTMOUSE, "CANCEL");
    }
    public void Unselect()
    {
        Selected = false;
        foreach (Point pos in Game.Level.AllTilePositions())
        {
            if (!IsMoving())
                Game.Level.GetTile(pos).ClearPathFindingInfo(Team);
            Game.Level.GetTile(pos).InRange = false;
            Game.Level.GetTile(pos).UnTint(Team);
        }

        if (Game != null && !IsMoving())
            Game.HUD.SetCurrentHelp(HeadsUpDisplay.PROMPT_LEFTMOUSE, "MENU");
    }

    public Point TilePosition() { return new Point(Mathf.RoundToInt(this.gameObject.transform.position.x), Mathf.RoundToInt(this.gameObject.transform.position.z)); }

    public bool IsMoving() { return Waypoints.Count != 0; }

    public void SetTeam(int team = -1, bool init = false)
    {
        if (team == -1)
            team = Team;

        if (Team != team || init)
        {
            if (Team != 0 && Game.Teams[Team - 1].Units.IndexOf(this) != -1)
                Game.Teams[Team - 1].Units.Remove(this);

            Team = team;

            if (Team != 0 && Game.Teams[Team - 1].Units.IndexOf(this) == -1)
                Game.Teams[Team - 1].Units.Add(this);
        }

        // Set Colour
        GetComponentInChildren<TeamColour>().SetTeam(Team, colorMultiplier: ColorMultiplier);
    }

    // Health

    public void Damage(float amount)
    {
        if (HitPoints <= 1 || HitPoints - amount < 1)
        {
            // Death
           Instantiate(Game.Effect_Explosion, transform.position, Quaternion.identity);

            Remove();

            Game.CheckWinLoseConditions(-1, -1);
        }
        else
            HitPoints -= amount;

        ///////I DONT KNOW WHY BUT THIS CODE CAUSES THE ENTIRE SCRIPT TO BE BUGGED
        //ParticleSystem.Particle[] m_Particles = new ParticleSystem.Particle[this.GetComponent<ParticleSystem>().particleCount];
        //system.GetParticles(m_Particles);

        //foreach (Particle p in m_Particles)
        //{
        //    var particle = p;
        //    float distance = Vector3.Distance(CurrentAttackTarget.transform.position, p.position);
        //    if (distance > 0.1f)
        //    {
        //        p.position = Vector3.Lerp(p.position, CurrentAttackTarget.transform.position, Time.deltaTime / 1.0f);
        //        p = particle;
        //    }
        //    system.SetParticles(m_Particles, m_Particles.Length);
        //}
        ////system.Stop();
        ////print ("Firing the particle system");
        //system.Play();
        //}

    }
    public void Heal(float amount)
    {
        if (HitPoints + amount > 10)
            HitPoints = 10;
        else
            HitPoints += amount;
    }
    public int GetHitPoints() { return Mathf.RoundToInt(HitPoints); }

    // Actions

    public void SetAttackTarget(Unit unit = null)
    {
        if (UnitsInAttackRange.Count == 0)
            GetUnitsInAttackRange();

        if (unit == null && UnitsInAttackRange.Count > 0)
            CurrentAttackTarget = UnitsInAttackRange[0];
        else
            CurrentAttackTarget = unit;

        //print(CurrentAttackTarget);

        WaitingForActionAccept = true;

        Game.HUD.ShowCrosshair(new Vector3(CurrentAttackTarget.transform.position.x, CurrentAttackTarget.GetPosY(), CurrentAttackTarget.transform.position.z));

        if (Game != null)
            Game.HUD.SetCurrentHelp(HeadsUpDisplay.PROMPT_RIGHTMOUSE, "CANCEL");
    }
    public Unit GetAttackTarget() { return CurrentAttackTarget; }

    public void AcceptAttack()
    {
        if (CurrentAttackTarget == null)
            return;

        WaitingForActionAccept = false;
        Game.HUD.HideCrosshair();

        // Face Each Other
        CurrentAttackTarget.transform.LookAt(transform.position);
        transform.LookAt(CurrentAttackTarget.transform.position);
	
        // Sound Effect
        if (Sound_Fire != null)
            GetComponent<AudioSource>().PlayOneShot(Sound_Fire);

        // Damage Enemy
        if (Game.Level.GetTile(CurrentAttackTarget.TilePosition()).tag.Equals("Deflect")) //Zac Lindsey
            CurrentAttackTarget.Damage(GetHitPoints() * 0.3f);
        else //More damage if enemy not on deflect
            CurrentAttackTarget.Damage(GetHitPoints() * 0.6f);

        //Bonus damage if the current unit on deflect
        if (Game.Level.GetTile(TilePosition()).tag.Equals("Deflect"))
        {
            CurrentAttackTarget.Damage(GetHitPoints() * 0.3f);
        }

       
		if (system == null&&Type!=1)
            system = GetComponent<ParticleSystem>();

		int count = 100;

        
		if (Type == 0 || Type == 2) {

			for (int i = 0; i < count; i++) {
				system.Emit (i);
				var particle = particles [i];
				float distance = Vector3.Distance (CurrentAttackTarget.transform.position, particle.position);


				//print ("Firing the particle system: Line 301");

				if (distance > 0.1f) {
					particle.position = Vector3.Lerp (particle.position, CurrentAttackTarget.transform.position, Time.deltaTime / 2.0f);
					particles [i] = particle;

				}
				system.SetParticles (particles, count);
		
			}
		}
        //float speed = 1;
        //ranged.transform.position = this.transform.position;
        //Vector3 target = CurrentAttackTarget.transform.position;


        //ranged.transform.forward = target;
        //Vector3 direction = this.transform.postion - target;
        //ranged.transform.position = Vector3.Lerp (this.transform.position, CurrentAttackTarget.transform.position, Time.deltaTime / 2.0f);

        //ranged.transform.L((direction.x * speed * Time.deltaTime), (direction.y * speed * Time.deltaTime), (direction.z * speed * Time.deltaTime),Space.World);

        //Used to do ranged attack. Sets color depending on what team is attacking. ~Erik
        
		if (Type == 1) {
            GameObject Ranged = Instantiate(ranged);
            Ranged.transform.position = CurrentAttackTarget.transform.position;
            system = Ranged.GetComponent<ParticleSystem> ();
			var col = system.colorOverLifetime;
			col.enabled = true;
			Gradient grad = new Gradient ();
			if (Team == 1) {
				grad.SetKeys (new GradientColorKey[] {
					new GradientColorKey (Color.yellow, 0.0f),
					new GradientColorKey (Color.white, 1.0f),
				}, new GradientAlphaKey[] {
					new GradientAlphaKey (2.0f, 0.0f),
					new GradientAlphaKey (0.0f, 0.0f)
				});
			} 
			else {
				grad.SetKeys (new GradientColorKey[] {
					new GradientColorKey (Color.blue, 0.0f),
					new GradientColorKey (Color.magenta, 1.0f)
				}, new GradientAlphaKey[] {
					new GradientAlphaKey (2.0f, 0.0f),
					new GradientAlphaKey (0.0f, 1.0f)
				});
				
			}
			col.color = grad;
			system.Emit (100);
            CurrentAttackTarget = null; // ?


            //ranged.transform.localRotation = 0;
            //ranged.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(ranged.transform.position, CurrentAttackTarget.transform.position,Time.deltaTime/2.0f));
            //ranged.transform.position = CurrentAttackTarget.transform.position; 
            //	system.Emit (count);
            //} 

            StartCoroutine(DestroyRanged(Ranged));
            AcceptMove();
        }
        
    }
	IEnumerator DestroyRanged(GameObject ranged)	//Used to destroy ranged attack object after X amount of time ~ Erik 
	{
		yield return new WaitForSeconds(3);
		DestroyImmediate(ranged.gameObject);
	}
    private void GetUnitsInAttackRange()
    {
        UnitsInAttackRange.Clear();

        List<Unit> enemyUnits = Game.GetEnemyUnits();
        for (int u = 0; u < enemyUnits.Count; u++)
        {
            if (InAttackRange(enemyUnits[u].TilePosition()))
                UnitsInAttackRange.Add(enemyUnits[u]);
        }
    }
    public bool InAttackRangeList(Unit unit) { return UnitsInAttackRange.IndexOf(unit) != -1; }


    public void CaptureBuilding()
    {
        if (BuildingOn == null)
            return;

        BuildingOn.Capture(GetHitPoints(), Team);

        AcceptMove();
    }


    public void ShowActions()
    {
        List<string> items = new List<string>();

        if (UnitsInAttackRange.Count != 0)
            items.Add("Emit");
        if (BuildingOn != null && BuildingOn.Team != Team)
            items.Add("Capture");
        items.Add("Wait");

        Game.HUD.ActionPopup.SetItems(items.ToArray());
        Game.HUD.ActionPopup.Show(false, transform.position);
    }
    public void UndoAction()
    {
        WaitingForActionAccept = false;

        Game.HUD.HideCrosshair();

        ShowActions();
    }
    public bool IsWaitingForActionAccept() { return WaitingForActionAccept; }

    // Pathfinding

    public void MoveToTile(Point tilePos)
    {
        if (Waypoints.Count != 0)
            return;

        StartPosition = transform.position;
        StartRotation = transform.rotation;

        bool valid = CalculatePathTo(new Vector3(tilePos.x, transform.position.y, tilePos.y), true);

        if (!valid)
            return;

        Game.Level.GetTile(TilePosition()).OnUnitLeave();

        if (Game != null)
            Game.HUD.SetCurrentHelp(HeadsUpDisplay.PROMPT_RIGHTMOUSE, "CANCEL");
    }
    public void UndoMove()
    {
        WaitingForMoveAccept = false;

        Game.Level.GetTile(TilePosition()).OnUnitLeave();

        transform.position = StartPosition;
        transform.rotation = StartRotation;
        Game.Selector.SelectUnit(this);

        Game.Level.GetTile(TilePosition()).OnUnitEnter(this);

        Game.HUD.HideCrosshair();
    }
    public void AcceptMove()
    {
        WaitingForMoveAccept = false;
        Moved = true;

        Game.Selector.UnselectCurrentUnit();

        ColorMultiplier = 0.35f;
        GetComponentInChildren<TeamColour>().SetTeam(Team, colorMultiplier: ColorMultiplier);
    }
    public void Reset()
    {
        Moved = false;
        WaitingForActionAccept = false;
        WaitingForMoveAccept = false;

        ColorMultiplier = 1;
        GetComponentInChildren<TeamColour>().SetTeam(Team, colorMultiplier: ColorMultiplier);
    }
    public bool IsWaitingForMoveAccept() { return WaitingForMoveAccept; }
    public bool HasMoved() { return Moved; }

    private void OnReachedWaypoint()
    {
        WaitingForMoveAccept = true;

        Game.Level.GetTile(TilePosition()).OnUnitEnter(this);

        GetUnitsInAttackRange();

        ShowActions();
    }

    public bool CalculatePathTo(Vector3 destination, bool setWaypoints = false)
    {
        int posX = Mathf.RoundToInt(destination.x);
        int posY = Mathf.RoundToInt(destination.z);

        if (Mathf.RoundToInt(transform.position.x) == posX && Mathf.RoundToInt(transform.position.z) == posY)
        {
            foreach (Point pos in Game.Level.AllTilePositions())
            {
                Game.Level.GetTile(pos).ClearPathFindingInfo(Team);
                //Debug.Log("POS:"+pos);
                //Debug.Log("TILE:"+Game.Level.GetTile(pos));
            }
            return false;
        }

        if (!Game.Level.ValidTile(posX, posY) || !Game.Level.GetTile(posX, posY).CanWalkOn() || !InRange(new Point(posX, posY)))
            return false;

        Waypoints.Clear();
        if (setWaypoints)
        {
            Unselect();
            Selected = true;
        }


        foreach (Point pos in Game.Level.AllTilePositions())
            Game.Level.GetTile(pos).ClearPathFindingInfo(Team);

        Game.Level.GetTile(posX, posY).DistanceSteps = 0;

        // Calculate distance between tiles
        int counter = 0;
        while (true)
        {
            bool madeProgress = false;

            foreach (Point pos in Game.Level.AllTilePositions())
            {
                int x = pos.x;
                int y = pos.y;

                if (!Game.Level.GetTile(x, y).CanWalkOn())
                    continue;

                int passHere = Game.Level.GetTile(x, y).DistanceSteps;

                foreach (Point movePos in ValidMoves(x, y))
                {
                    int newX = movePos.x;
                    int newY = movePos.y;
                    int newPass = passHere + 1;

                    if (Game.Level.GetTile(newX, newY).DistanceSteps > newPass)
                    {
                        Game.Level.GetTile(newX, newY).DistanceSteps = newPass;
                        madeProgress = true;
                    }
                }
            }
            if (!madeProgress)
                break;
            //counter++;
        }

        // Calculate Path
        posX = Mathf.RoundToInt(transform.position.x);
        posY = Mathf.RoundToInt(transform.position.z);

        if (!setWaypoints)
            Waypoints.Add(new Vector3(posX, transform.position.y, posY));
        //Game.Level.GetTile(posX, posY).SetAsPath();

        while (true)
        {
            // Look through each direction and fine the tile with the lowest number of steps
            Point lowestPoint = new Point();
            int lowest = 10000;

            foreach (Point movePoint in ValidMoves(posX, posY))
            {
                int count = Game.Level.GetTile(movePoint.x, movePoint.y).DistanceSteps;
                if (count < lowest)
                {
                    lowest = count;
                    lowestPoint.x = movePoint.x;
                    lowestPoint.y = movePoint.y;
                }
            }
            if (lowest != 10000)
            {
                // If lowest number, add Waypoint.
                posX = lowestPoint.x;
                posY = lowestPoint.y;
                if (Game.Level.GetTile(posX, posY).CanWalkOn())
                    Waypoints.Add(new Vector3(posX, transform.position.y, posY));
            }
            else
                break;

            // Hit Destination
            if (posX == Mathf.RoundToInt(destination.x) && posY == Mathf.RoundToInt(destination.z))
                break;
        }

        if (setWaypoints)
        {
            foreach (Point pos in Game.Level.AllTilePositions())
                Game.Level.GetTile(pos).InRange = false;
        }
        else
        {
            // Draw Path
            bool lastTile = false;
            for (int i = 0; i < Waypoints.Count; i++)
            {
                lastTile = (i == Waypoints.Count - 1);
                Game.Level.GetTile((int)Waypoints[i].x, (int)Waypoints[i].z).SetAsPath(
                    i == 0,
                    i == 0 ? default(Point) : new Point((int)Waypoints[i - 1].x, (int)Waypoints[i - 1].z),
                    lastTile,
                    lastTile ? default(Point) : new Point((int)Waypoints[i + 1].x, (int)Waypoints[i + 1].z));
            }

            Waypoints.Clear();
        }

        return true;
    }

    private IEnumerable<Point> ValidMoves(int x, int y)
    {
        foreach (Point movePos in MoveDirections)
        {
            int newX = x + movePos.x; //roundtoint was here before??
            int newY = y + movePos.y;

            if (Game.Level.levelNumber == 1)
            {
                if (Game.Level.ValidTile(newX, newY) && Game.Level.GetTile(newX, newY).ValidPath())
                    yield return new Point(newX, newY);
            }
            else
            {
                if (Game.Level.GetTile(newX, newY) != null)
                {
                    if (Game.Level.ValidTile(newX, newY) && Game.Level.GetTile(newX, newY).ValidPath())
                        yield return new Point(newX, newY);
                }
            }
        }
    }


    private void HighlightTilesInRange()
    {
        Game.Level.GetTile(TilePosition()).TintAsInRange(Team);

        foreach (Point pos in Game.Level.AllTilePositions())
        {
            if (!InRange(pos))
                continue;

            if (Game.Level.ValidTile(pos) && Game.Level.GetTile(pos).CanWalkOn())
                Game.Level.GetTile(pos).TintAsInRange(Team);
        }
    }

    public bool InRange(Point pos)
    {
        Point unitPos = TilePosition();
        double square_dist = Mathf.Pow(pos.x - unitPos.x, 2) + Mathf.Pow(pos.y - unitPos.y, 2);
        return square_dist <= Mathf.Pow(Range + 0.99f, 2);
    }

    public bool InAttackRange(Point pos)
    {
        Point unitPos = TilePosition();
        double square_dist = Mathf.Pow(pos.x - unitPos.x, 2) + Mathf.Pow(pos.y - unitPos.y, 2);
        return square_dist <= Mathf.Pow(AttackRange + 0.99f, 2);
    }
}
