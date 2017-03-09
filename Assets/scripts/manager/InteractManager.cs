using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManager : MonoBehaviour {
    private TileManager tileManager;
    private GameObject selectedUnit;
    public int teamId;

    void Awake () {
        tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
    }

    void Update () {
        if (Input.GetMouseButtonDown(0)) {
            CheckForHit(Input.mousePosition);
        }
    }

    private void CheckForHit(Vector3 mousePosition) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            Select(hit.collider);
        }
    }

    private void Select(Collider col) {
        UnitAttributes attr = col.GetComponent<UnitAttributes>();

        if (attr != null) {
            SelectUnit(col.gameObject, attr);
        }
    }

    private void SelectUnit(GameObject unit, UnitAttributes attr) {
        // If selecting same unit, do nothing
        if (selectedUnit == unit) {
            return;
        }

        UnselectUnit();
        selectedUnit = unit;
        tileManager.DeactivateTiles();

        if (attr.teamId == teamId) {
            attr.isSelected = true;
            attr.GetComponent<Movement>().ToggleMovement();
        } else {
            
        }
    }

    private void UnselectUnit() {
        if (selectedUnit != null) {
            UnitAttributes attr = selectedUnit.GetComponent<UnitAttributes>();
            attr.isSelected = false;
        }
    }
}
