using System.Collections;
using System.Collections.Generic;

public class Player {
	private Unit[] units = new Unit[25];
	private int maxEnergy;
    private int energy;
	private bool hasMoved;
	private int level;
	private int corruptedResource;

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
}
