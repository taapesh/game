using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Unit[] units = new Unit[25];
    private Dictionary<UnitData, int> activeUnitsMap = new Dictionary<UnitData, int>();
    private int teamId;
    private int maxEnergy;
    private int energy;
    private int level;
    private int corruptedResource;

    public void InitPlayer() {
        // Initialize player
        this.energy = 10;
    }

    public void SetTeamId(int teamId) {
        this.teamId = teamId;
    }

    public int GetTeamId() {
        return teamId;
    }

    public Unit[] GetUnits() {
        return units;
    }

    public void ResetUnits() {
        foreach (Unit unit in units) {
            unit.SetHasMoved(false);
            unit.SetHasAttacked(false);
        }
    }

    public int GetEnergy() {
        return energy;
    }

    public void SetEnergy(int energy) {
        this.energy = energy;
    }

    public bool EnoughEnergy(int energy) {
        return this.energy >= energy;
    }

    public void SpendEnergy(int amount) {
        this.energy -= amount;
    }

    public int GetLevel() {
        return level;
    }

    public void LevelUp() {
        this.level++;
    }

    public int GetCorruptedResource() {
        return corruptedResource;
    }

    public void GainCorruptedResouce(int amount) {
        this.corruptedResource += amount;
    }

    public int GetUnitActiveCount(UnitData unit) {
        if (activeUnitsMap[unit] == null) {
            Debug.Log("LOL");
            return 10;
        }
        return 10;
    }
}
