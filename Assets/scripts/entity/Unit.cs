using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    private int teamId;
    private int unitId;
    private int objectId;
    private int tileId;
    private bool hasMoved;
    private bool hasAttacked;
    private bool isDead;
    private bool isSelected;
    private Movement moveComponent;
    public int health;
    public int maxHealth;
    public int movementRange;
    public int activeLimit;
    public int energyCost;

    void Awake() {
        moveComponent = GetComponent<Movement>();
    }

    public void SetSelected(bool selected) {
        this.isSelected = selected;
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

    public bool Equals(Unit unit) {
        return (
            unit.GetTeamId() == GetTeamId() &&
            unit.GetUnitId() == GetUnitId()
        );
    }

    public Vector3 GetPosition() {
        return new Vector3(transform.position.x, 0, transform.position.z);
    }

    public Movement GetMoveComponent() {
        return moveComponent;
    }
}
