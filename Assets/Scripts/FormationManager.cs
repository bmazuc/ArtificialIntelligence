using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_Formation
{
    RECTANGLE = 0,
    CIRCLE
}

public class Formation
{
    public List<Unit> unitList;
    public float formationSpeed;
}

public class FormationManager : MonoBehaviour {
    [SerializeField]
    private float distanceBetweenUnits = 1f;
    [SerializeField]
    private int unitsPerRow;
    [SerializeField]
    private E_Formation actualFormation = E_Formation.RECTANGLE;
    [SerializeField]
    private float circleFormationRadius;

    public delegate void FormationHandler();
    public event FormationHandler OnFormationChange;

    void Start()
    {
        GuiManager.Instance.playerFormationMotionManager = this;
    }

    public void CreateFormation(List<Unit> units)
    {
        float speed = float.MaxValue;

        foreach (Unit unit in units)
        {
            if (speed > unit.movement.GetMaxSpeed)
                speed = unit.movement.GetMaxSpeed;
        }

        Formation formation = new Formation()
        {
            unitList = units,
            formationSpeed = speed
        };

        foreach (Unit unit in units)
            unit.Formation = formation;
    }

    public void AddToFormation(List<Unit> units, Unit unit)
    {
        bool changeSpeed = false;
        if (unit.movement.GetMaxSpeed < units[0].Formation.formationSpeed)
            changeSpeed = true;

        foreach (Unit formationUnit in units)
        {
            formationUnit.Formation.unitList.Add(unit);
            if (changeSpeed)
                formationUnit.Formation.formationSpeed = unit.movement.GetMaxSpeed;
        }

        unit.Formation = units[0].Formation;
    }

    public void RemoveFromFormation(Unit unit)
    {
        if (unit.Formation == null)
            return;

        float speed = float.MaxValue;

        foreach (Unit formationUnit in unit.Formation.unitList)
        {
            if (speed > formationUnit.movement.GetMaxSpeed && formationUnit != unit)
                speed = formationUnit.movement.GetMaxSpeed;
        }

        foreach (Unit formationUnit in unit.Formation.unitList)
        {
            if (formationUnit != unit)
            {
                formationUnit.Formation.unitList.Remove(unit);
                formationUnit.Formation.formationSpeed = speed;
            }
        }

        unit.Formation = null;
    }

    public List<Vector3> GetUnitsPosition(int unitNumber, Vector3 targetPosition)
    {
        switch(actualFormation)
        {
            case E_Formation.RECTANGLE: return GetRectangleFormationPositions(unitNumber, targetPosition);
            case E_Formation.CIRCLE : return GetCircleFormationPositions(unitNumber, targetPosition);
            default: return null;
        }
    }

    List<Vector3> GetRectangleFormationPositions(int unitNumber, Vector3 targetPosition)
    {
        List<Vector3> list = new List<Vector3>();
        List<Vector3> invalidLocation = new List<Vector3>();

        float LineNextRow = 0f;
        float LineNextRight = 0f;
        float LineNextLeft = 0f;

        for (int idx = 0; idx < unitNumber; ++idx)
        {
            Vector3 right = Vector3.zero;
            if (idx > 0)
            {
                if (idx % unitsPerRow == 0)
                {
                    LineNextRow += distanceBetweenUnits;
                    LineNextRight = 0f;
                    LineNextLeft = 0f;
                }
                else
                {
                    if (idx % 2 == 0)
                    {
                        LineNextRight += distanceBetweenUnits;
                        right = Vector3.right * LineNextRight;
                    }
                    else
                    {
                        LineNextLeft += distanceBetweenUnits;
                        right = Vector3.left * LineNextLeft;
                    }
                }
            }

            Vector3 forward = Vector3.forward * -1 * LineNextRow;
            Vector3 position = targetPosition + forward + right;
           

            if (!Navigation.TileNavGraph.Instance.IsPosValid(position))
                invalidLocation.Add(position);
            else
                list.Add(position);
        }

        return list;
    }

    List<Vector3> GetCircleFormationPositions(int unitNumber, Vector3 targetPosition)
    {
        List<Vector3> list = new List<Vector3>();
        List<Vector3> invalidLocation = new List<Vector3>();

        float theta = Mathf.Deg2Rad * (360f / unitNumber);
        float nextTheta = 0f;
        float radius = Mathf.Deg2Rad * (unitNumber / 4f) + circleFormationRadius;

        for (int idx = 0; idx < unitNumber; ++idx)
        {
            Vector3 forward = Vector3.forward * (Mathf.Cos(nextTheta) * radius);
            Vector3 right = Vector3.right * (Mathf.Sin(nextTheta) * radius);
            Vector3 position = targetPosition + forward + right;

            position.y = targetPosition.y;

            if (!Navigation.TileNavGraph.Instance.IsPosValid(position))
                position = RelocateCircleFormationPositions(position);

            list.Add(position);
            nextTheta += theta;
        }

        return list;
    }

    private Vector3 RelocateCircleFormationPositions(Vector3 invalidLocation)
    {
        GameObject trans = new GameObject();
        trans.transform.LookAt(invalidLocation);
        Vector3 position = invalidLocation + trans.transform.forward * -1 * circleFormationRadius;

        return position;
    }

    public void SetFormation(E_Formation newFormation)
    {
        actualFormation = newFormation;
        if (OnFormationChange != null)
            OnFormationChange();
    }
}
