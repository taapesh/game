using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    private UnityEngine.AI.NavMeshAgent nav;
    private int teamId;
    private int unitId;
    private int objectId;
    private int tileId;
    private bool hasMoved;
    private bool hasAttacked;
    private bool isDead;
    private bool isSelected;
    public float stopDistance;
    public string name;
    public int id;
    public int health;
    public int maxHealth;
    public int movementRange;
    public int activeLimit;
    public int energyCost;

    void Awake() {
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public bool IsFriendly(int teamId) {
        return this.teamId == teamId;
    }

    public void SetDestination(Vector3 target) {
        nav.SetDestination(target);
    }

    public float GetStopDistance() {
        return stopDistance;
    }

    public void SetSelected(bool selected) {
        this.isSelected = selected;
    }

    public int GetId() {
        return id;    
    }

    public bool IsSelected() {
        return isSelected;
    }

    public int GetUnitId() {
        return unitId;
    }

    public void SetUnitId(int unitId) {
        this.unitId = unitId;
    }

    public int GetTeamId() {
        return teamId;
    }

    public void SetTeamId(int teamId) {
        this.teamId = teamId;
    }

    public int GetTileId() {
        return tileId;
    }

    public void SetTileId(int tileId) {
        this.tileId = tileId;
    }

    public int GetObjectId() {
        return objectId;
    }

    public void SetObjectId(int objectId) {
        this.objectId = objectId;
    }

    public bool HasMoved() {
        return hasMoved;
    }

    public void SetHasMoved(bool moved) {
        this.hasMoved = moved;
    }

    public bool HasAttacked() {
        return hasAttacked;
    }

    public void SetHasAttacked(bool hasAttacked) {
        this.hasAttacked = false;
    }

    public bool IsDead() {
        return isDead;
    }

    public int GetMovementRange() {
        return movementRange;
    }

    public int GetActiveLimit() {
        return activeLimit;
    }

    public int GetEnergyCost() {
        return energyCost;
    }

    public bool Equals(Unit unit) {
        return (
            unit.GetTeamId() == GetTeamId() &&
            unit.GetUnitId() == GetUnitId()
        );
    }

    public Vector3 GetPosition() {
        return new Vector3(transform.position.x, 0, transform.position.z);
    }
}
