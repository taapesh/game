using System.Collections;
using System.Collections.Generic;

public class Player {
	private ArrayList units;
    private int energy;
	private bool hasMoved;
	private int level;
	private int corruptedResource;

    public ArrayList GetUnits() {
        return units;
    }

	public void ResetUnits() {
		foreach (Unit unit in units) {
			unit.SetHasMoved(false);
			unit.SetHasAttacked(false);
		}	
	}

	public bool HasMoved() {
		return hasMoved;
	}

	public void SetHasMoved(bool moved) {
		this.hasMoved = moved;
	}

	public int GetEnergy() {
		return energy;
	}

	public void SetEnergy(int energy) {
		this.energy = energy;
	}

	public int GetLevel() {
		return level;
	}

	public int LevelUp() {
		this.level++;
	}

	public int GetCorruptedResource() {
		return corruptedResource;
	}

	public int GainCorruptedResouce(int amount) {
		this.corruptedResource += amount;
	}
}
