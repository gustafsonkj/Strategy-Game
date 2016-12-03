﻿using UnityEngine;

public class BuyMenu : Menu
{
	public Texture2D Icon_Red_Tank;
	public Texture2D Icon_Blue_Tank;

	private Building Building;

	private Rect Price_Rect;
	private GUIStyle Price_Style;

    private int[] prices = new int[]
    {
        10000, 12000, 14000, 18000, 20000, 24000
	};

	protected override void Init()
	{
		base.Init();

		BoxWidth = 200;

		Price_Style = new GUIStyle(ButtonStyle);
		Price_Style.alignment = TextAnchor.UpperRight;
		Price_Style.contentOffset = new Vector2(-2, 0);
	}

	public void SetBuilding(Building building)
	{
		Building = building; 

		ClearItems();

		AddItem("Basic Red", Building.Team == 2 ? Icon_Blue_Tank : Icon_Red_Tank);
        AddItem("Basic Green", Building.Team == 2 ? Icon_Blue_Tank : Icon_Red_Tank);
        AddItem("Basic Blue", Building.Team == 2 ? Icon_Blue_Tank : Icon_Red_Tank);
        AddItem("Ranged Red", Building.Team == 2 ? Icon_Blue_Tank : Icon_Red_Tank);
        AddItem("Ranged Green", Building.Team == 2 ? Icon_Blue_Tank : Icon_Red_Tank);
        AddItem("Ranged Blue", Building.Team == 2 ? Icon_Blue_Tank : Icon_Red_Tank);
    }

	public override void Show(bool middleOfScreen, Vector3 position)
	{
		for (int i = 0; i < prices.Length; i++)
		{
			if (Game.GetCurrentTeam().Resources < prices[i])
			{
				ButtonStyle.normal.textColor = Color.grey;
				IconColors[i] = new Color(1, 1, 1, 0.4f);
			}
			else
			{
				ButtonStyle.normal.textColor = Color.black;
				IconColors[i] = default(Color);
			}
		}
		Price_Style.normal.textColor = ButtonStyle.normal.textColor;

		base.Show(middleOfScreen, position);

		Price_Rect = new Rect();
		Price_Rect.height = ButtonHeight;
		Price_Rect.x = Rect.x + Rect.width - 2;
	}

	protected override void DrawButton(int i)
	{
		Price_Rect.y = Button_Rect.y + 35;
		GUI.TextArea(Price_Rect, prices[i].ToString(), Price_Style);

		base.DrawButton(i);
	}

	protected override void OnButtonPress(string item)
	{
		if (Building == null)
			return;

		switch (item)
		{
		    case "Basic":
			    if (Game.GetCurrentTeam ().Resources < prices [0])
				    break;
			    Game.GetCurrentTeam ().Resources -= prices [0];
			    Transform unitObject;
			    Unit unit;
			    Game.HUD.SetResources (Game.GetCurrentTeam ().Resources);
                if (Game.GetCurrentTeam().TeamNo == 1)
                {
                    unitObject = Instantiate(Game.Unit_TankA, Building.transform.position, Quaternion.identity) as Transform;
                    unitObject.parent = GameObject.Find("Units").transform;
                    unit = unitObject.GetComponent<Unit>();
                    unit.Init();
                    unit.SetTeam(Building.Team);
                    unit.AcceptMove();
                }
                else
                {
                    unitObject = Instantiate(Game.Unit_TankB, Building.transform.position, Quaternion.identity) as Transform;
                    unitObject.parent = GameObject.Find("Units").transform;
                    unit = unitObject.GetComponent<Unit>();
                    unit.Init();
                    unit.SetTeam(Building.Team);
                    unit.AcceptMove();
                }
			    break;
		}

		Game.Selector.UnselectCurrentBuilding();

		Hide();
	}
}