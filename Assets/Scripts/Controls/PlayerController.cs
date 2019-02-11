using UnityEngine;
using System.Collections.Generic;
using Navigation;
using UnityEngine.EventSystems;

public class PlayerController : UnitController {

    [SerializeField]
    private GameObject TargetCursorPrefab = null;
    private GameObject targetCursor = null;

    delegate void InputEventHandler();
    event InputEventHandler OnMouseClicked;
    event InputEventHandler OnDeletePressed;
    event InputEventHandler OnSelectAllPressed;
    event InputEventHandler OnLightCatPressed;
    event InputEventHandler OnMediumCatPressed;
    event InputEventHandler OnHeavyCatPressed;

    private GameObject GetTargetCursor()
    {
        if (targetCursor == null)
            targetCursor = Instantiate(TargetCursorPrefab);
        return targetCursor;
    }

    public FormationManager formationManager;

    // Use this for initialization
    override protected void Start()
    {
        base.Start();

        OnMouseClicked += () =>
        {
            // To disable raycast when clicking on button
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int uiLayer = 1 << LayerMask.NameToLayer("UI");
            int factoryLayer = 1 << LayerMask.NameToLayer("Factory");
            int unitLayer = 1 << LayerMask.NameToLayer("Unit");
            int floorLayer = 1 << LayerMask.NameToLayer("Floor");

            RaycastHit raycastInfo;
            // factory selection
            if (Physics.Raycast(ray, out raycastInfo, Mathf.Infinity, uiLayer))
                return;
            else if (Physics.Raycast(ray, out raycastInfo, Mathf.Infinity, factoryLayer))
            {
                UnitFactory factory = raycastInfo.transform.GetComponent<UnitFactory>();
                if (factory != null && factory.GetTeam == team)
                {
                    if (currentFactory == factory)
                    {
                        RequestFactoryBuild();
                    }
                    else
                    {
                        UnselectCurrentFactory();
                        SelectFactory(factory);
                    }
                }
            }
            // unit selection / unselection
            else if (Physics.Raycast(ray, out raycastInfo, Mathf.Infinity, unitLayer))
            {
                bool isShiftBtPressed = Input.GetKey(KeyCode.LeftShift);
                bool isCtrlBtPressed = Input.GetKey(KeyCode.LeftControl);

                UnselectCurrentFactory();

                Unit selectedUnit = raycastInfo.transform.GetComponent<Unit>();
                if (selectedUnit != null && selectedUnit.GetTeam == team)
                {
                    if (isShiftBtPressed)
                    {
                        UnseletecUnit(selectedUnit);
                    }
                    else if (isCtrlBtPressed)
                    {
                        SelectUnit(selectedUnit);
                    }
                    else
                    {
                        UnselectAllUnits();
                        SelectUnit(selectedUnit);
                    }
                }
            }
            // unit move target
            else if (Physics.Raycast(ray, out raycastInfo, Mathf.Infinity, floorLayer))
            {
                UnselectCurrentFactory();

                if (selectedUnitList.Count == 0)
                    return;

                Vector3 newPos = raycastInfo.point;
                Vector3 targetPos = newPos;
                targetPos.y += 0.1f;
                GetTargetCursor().transform.position = targetPos;
                if (TileNavGraph.Instance.IsPosValid(newPos))
                {
                    if (selectedUnitList.Count > 1)
                    {
                        formationManager.CreateFormation(selectedUnitList);
                        List<Vector3> positions = formationManager.GetUnitsPosition(selectedUnitList.Count, targetPos);
                        for (int idx = 0; idx < selectedUnitList.Count; ++idx)
                            selectedUnitList[idx].SetTargetPos(positions[idx]);
                    }
                    else
                        foreach (Unit unit in selectedUnitList)
                        {
                            if (unit.Formation != null)
                                formationManager.RemoveFromFormation(unit);
                            unit.SetTargetPos(newPos);
                        }
                }
            }
        };

        OnDeletePressed += () =>
        {
            foreach (Unit unit in unitList)
            {
                unit.AddDamages(100);
            }
            unitList.Clear();
        };

        OnSelectAllPressed += () =>
        {
            SelectAllUnits();
        };

        OnLightCatPressed += () =>
        {
            SelectAllUnitsByCategory(Unit.ECategory.Light);
        };

        OnMediumCatPressed += () =>
        {
            SelectAllUnitsByCategory(Unit.ECategory.Medium);
        };

        OnHeavyCatPressed += () =>
        {
            SelectAllUnitsByCategory(Unit.ECategory.Heavy);
        };

        formationManager.OnFormationChange += () =>
        {
            if (selectedUnitList.Count > 1)
            {
                List<Vector3> positions = formationManager.GetUnitsPosition(selectedUnitList.Count, selectedUnitList[0].transform.position);
                for (int idx = 0; idx < selectedUnitList.Count; ++idx)
                    selectedUnitList[idx].SetTargetPos(positions[idx]);
            }
        };
    }

    // Update is called once per frame
    override protected void Update ()
    {
        if (Input.GetMouseButtonDown(0))
            OnMouseClicked();

        if (Input.GetKeyDown(KeyCode.Delete))
            OnDeletePressed();

        if (Input.GetKeyDown(KeyCode.A))
            OnSelectAllPressed();

        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            OnLightCatPressed();

        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            OnMediumCatPressed();

        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            OnHeavyCatPressed();
    }
}
