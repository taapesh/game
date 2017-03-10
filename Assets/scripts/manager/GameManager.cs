using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    private GameObject architectA;
    private GameObject architectB;
    private GameObject[] unitsA = new GameObject[25];
    private GameObject[] unitsB = new GameObject[25];
    private HashSet<int> validMovementTiles;
    private int turnNumber;
    private int turnTimer;
    private int secondsPerTurn = 3;
    private int energyA;
    private int energyB;
    private int turnId = 0;
    public delegate void OnTurnChangedEvent(int turnId);
    public static event OnTurnChangedEvent OnTurnChanged;

    protected void Start() {
        turnNumber = 1;
        turnTimer = secondsPerTurn;
        InvokeRepeating("CountdownTurn", 0f, 1f);
    }

    private void CountdownTurn() {
        turnTimer -= 1;

        if (turnTimer == 0) {
            ChangeTurns();
        }
    }

    private void ChangeTurns() {
        turnNumber++;
        turnTimer = secondsPerTurn;
        turnId = 1 - turnId;
        ResetUnits(turnId);
        OnTurnChanged(turnId);
    }

    private void ResetUnits(int teamId) {
        foreach (GameObject unit in GetUnits(teamId)) {
            if (unit == null) {
                continue;
            }

            ResetAttributes(unit);
        }
    }

    private void ResetAttributes(GameObject unit) {
        UnitAttributes attr = unit.GetComponent<UnitAttributes>();
        attr.hasMoved = false;
        attr.hasAttacked = false;
    }

    public bool IsMyTurn(int teamId) {
        return turnId == teamId;
    }

    public GameObject[] GetUnits(int teamId) {
        return (teamId == 0) ? unitsA : unitsB;
    }

    public GameObject GetUnit(int teamId, int unitId) {
        return GetUnits(teamId)[unitId];
    }

    public int AddUnit(int teamId, GameObject unit) {
        GameObject[] units = GetUnits(teamId);
        int index = -1;

        for (int i = 0; i < units.Length; ++i) {
            if (units[i] == null) {
                index = i;
                break;
            }
        }

        if (index == -1) {
            System.Array.Resize(ref units, units.Length * 2);
            index = units.Length;
        }

        units[index] = unit;
        unit.GetComponent<UnitAttributes>().unitId = index;
        return index;
    }

    public void ToggleMovement(UnitAttributes attr) {
        if (CanMove(attr)) {
            validMovementTiles = TileManager.Instance.TilesInRange(attr.tileId, attr.movementRange);
            TileManager.Instance.ActivateTiles(validMovementTiles, TileManager.Instance.moveMaterial);
        }
    }

    public bool CanMove(UnitAttributes attr) {
        return (
            attr.isSelected &&
            IsMyTurn(attr.teamId) &&
            !attr.hasMoved &&
            !attr.hasAttacked
        );
    }

    public bool MoveUnitToTile(GameObject unit, UnitAttributes attr, int tileId) {
        if (!validMovementTiles.Contains(tileId)) {
            return false;
        }

        TileManager.Instance.DeactivateTiles();
        TileManager.Instance.SetOccupant(tileId, unit);
        TileManager.Instance.ClearOccupant(attr.tileId);
        attr.hasMoved = true;
        attr.tileId = tileId;
        UnityEngine.AI.NavMeshAgent nav = unit.GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav.SetDestination(TileManager.Instance.GetTile(tileId).position);
        return true;
    }

    public bool CanSummon(UnitAttributes attr) {
        return true;
    }
}
