using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour {
    private int damage;
    private int corruption;
    private bool isCorrupted;
    private bool isInvulnerable;
    private int armor;

    public bool IsCorrupted() {
        return isCorrupted;
    }

    public bool IsInvulnerable() {
        return isInvulnerable;
    }

    public int GetArmor() {
        return armor;
    }
}
