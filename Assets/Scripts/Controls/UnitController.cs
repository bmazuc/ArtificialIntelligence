using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// points system for units creation (Ex : light units = 1 pt, medium = 2pts, heavy = 3 pts)
// max points can be increased by capturing map points
public class UnitController : MonoBehaviour
{
    [SerializeField]
    protected Team team;
    public Team GetTeam { get { return team; } }

    [SerializeField]
    protected int StartingBuildPoints = 15;
    protected int totalBuildPoints = 0;
    public int TotalBuildPoints { get { return totalBuildPoints; } }

    protected List<Unit> unitList = new List<Unit>();
    protected UnitFactory currentFactory;
    protected List<Unit> selectedUnitList = new List<Unit>();

    #region unit methods
    protected void UnselectAllUnits()
    {
        foreach (Unit unit in selectedUnitList)
            unit.SetSelected(false);
        selectedUnitList.Clear();
        GuiManager.Instance.ShowFormationPanel(false);
    }

    protected void SelectAllUnits()
    {
        foreach (Unit unit in unitList)
            unit.SetSelected(true);

        selectedUnitList.Clear();
        selectedUnitList.AddRange(unitList);
        GuiManager.Instance.ShowFormationPanel(true);
    }

    protected void SelectAllUnitsByCategory(Unit.ECategory cat)
    {
        UnselectAllUnits();
        selectedUnitList = unitList.FindAll(delegate (Unit unit)
            {
                return unit.GetCategory == cat;
            }
        );
        foreach(Unit unit in selectedUnitList)
            unit.SetSelected(true);

        GuiManager.Instance.ShowFormationPanel(true);
      }

    protected void SelectUnitList(List<Unit> units)
    {
        foreach (Unit unit in units)
            unit.SetSelected(true);
        selectedUnitList.AddRange(units);
        GuiManager.Instance.ShowFormationPanel(true);
    }

    protected void SelectUnit(Unit unit)
    {
        unit.SetSelected(true);
        selectedUnitList.Add(unit);

        if (selectedUnitList.Count > 1)
            GuiManager.Instance.ShowFormationPanel(true);
    }

    protected void UnseletecUnit(Unit unit)
    {
        unit.SetSelected(false);
        selectedUnitList.Remove(unit);
        if (selectedUnitList.Count < 2)
            GuiManager.Instance.ShowFormationPanel(false);
    }

    private void AddUnit(Unit unit)
    {
        unit.OnDeadEvent += (object sender) =>
        {
            totalBuildPoints += unit.GetCost;
        };
        unitList.Add(unit);
    }

    public void AddBuildPoints(int points)
    {
        totalBuildPoints += points;
    }
    #endregion

    #region factory methods
    protected void SelectFactory(UnitFactory factory)
    {
        currentFactory = factory;
        currentFactory.SetSelected(true);
        UnselectAllUnits();
    }

    protected void UnselectCurrentFactory()
    {
        if (currentFactory != null)
            currentFactory.SetSelected(false);
        currentFactory = null;
    }

    protected void RequestFactoryBuild()
    {
        if (currentFactory == null)
            return;

        if (totalBuildPoints < currentFactory.UnitCost)
            return;

        totalBuildPoints -= currentFactory.UnitCost;

        currentFactory.OnUnitBuilt += (Unit unit) =>
        {
            if (unit != null)
                AddUnit(unit);
        };
        currentFactory.StartBuildUnit();
    }

    #endregion

    // Use this for initialization
    virtual protected void Start ()
    {
        totalBuildPoints = StartingBuildPoints;
    }

    // Update is called once per frame
    virtual protected void Update () {
		
	}
}
