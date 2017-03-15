using System.Collections;
using System.Collections.Generic;

public class Unit {
    private bool hasMoved;
    private bool hasAttacked;
	private bool isDead;

    public int health;
    public int maxHealth;
    public int movementRange;
	public int activeLimit;
	public int energyCost;

	public void SetHasMoved(bool moved) {
		this.hasMoved = moved;
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
}
