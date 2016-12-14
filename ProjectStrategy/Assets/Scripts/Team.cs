using System.Collections.Generic;

public class Team
{
	public int TeamNo = -1;

	public int Resources = 1000;
	public List<Unit> Units = new List<Unit>();
	public List<Building> Buildings = new List<Building>();

	private const int INCOME_PER_RESOURCE = 5000;

	public void GainIncome()
	{
        int numResourceNodes = 0;
        foreach (Building b in Buildings)
        {
            if (b.Type == Building.RESOURCE)
            {
                numResourceNodes++;
            }
        }
        Resources += 1000;
        Resources += INCOME_PER_RESOURCE * numResourceNodes;
	}

	public void ResetUnits()
	{
		for (int i = 0; i < Units.Count; i++)
		{
			Units[i].Reset();
		}
		for (int i = 0; i < Buildings.Count; i++)
		{
			Buildings[i].Reset();
		}
	}

	public void HealUnitsInCities()
	{
		for (int i = 0; i < Units.Count; i++)
		{
			if (Units[i].GetHitPoints() != 10 && Units[i].BuildingOn != null && Units[i].BuildingOn.Team == Units[i].Team)
			{
				if (Units[i].BuildingOn.Type == Building.RESOURCE)
					Units[i].Heal(1);
				else if (Units[i].BuildingOn.Type == Building.GENERATOR)
					Units[i].Heal(2);
			}
		}
	}
	public void HealUncontestedBuildings()
	{
		for (int i = 0; i < Buildings.Count; i++)
		{
			if (Buildings[i].GetHitPoints() < Buildings[i].GetHitPointsMax() && (Buildings[i].UnitOnTop == null || Buildings[i].UnitOnTop.Team == TeamNo))
				Buildings[i].Heal(2);
		}
	}
}