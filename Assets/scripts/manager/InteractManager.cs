using UnityEngine;

public class InteractManager : MonoBehaviour {
    private UnitAttributes selectedUnitAttr;
    public int teamId;

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
        if (selectedUnitAttr == attr) {
            return;
        }

        TileManager.Instance.DeactivateTiles();
        UnselectUnit();

        if (attr.teamId == teamId) {
            attr.isSelected = true;
            GameManager.Instance.ToggleMovement(attr);
        } else {
            
        }

        selectedUnitAttr = attr;
    }

    private void UnselectUnit() {
        if (selectedUnitAttr != null) {
            selectedUnitAttr.isSelected = false;
            selectedUnitAttr = null;
        }
    }
}
