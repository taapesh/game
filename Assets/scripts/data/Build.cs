using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class Build {
    private Dictionary<int, UnitData> units = new Dictionary<int, UnitData>();

    public string name;
    public UnitChoice[] unitChoices;

    public string GetName() {
        return name;
    }

    public UnitChoice[] GetUnitChoices() {
        return unitChoices;
    }

    public void AddUnitData(UnitData unitData, int slotId) {
        units.Add(slotId, unitData);
    }

    public UnitData GetUnit(int slotId) {
        return units[slotId];
    }

    public Dictionary<int, UnitData> GetUnits() {
        return units;
    }

    public bool HasSlot(int slotId) {
        return units[slotId] != null;
    }
}
