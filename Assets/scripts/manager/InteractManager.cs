using UnityEngine;

public class InteractManager : MonoBehaviour {
    private Unit selectedUnit;
    public int teamId;

    void Awake() {
        GameManager.OnTurnChanged += OnTurnChanged;
        GameManager.OnUnlock += OnUnlock;
    }

    protected void OnUnlock() {
        if (GameManager.Instance.IsMyTurn(teamId)) {
            CheckForMovement();
        }
    }

    protected void OnTurnChanged(int turnId) {
        if (GameManager.Instance.IsMyTurn(teamId)) {
            CheckForMovement();
        } else {
            TileManager.Instance.DeactivateTiles();
        }
    }

    private void CheckForMovement() {
		if (selectedUnit != null && selectedUnit.GetTeamId() == teamId) {
			selectedUnit.GetMoveComponent().ToggleMovement();
        }
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
        Unit unit = col.GetComponent<Unit>();

        if (unit != null) {
            SelectUnit(unit);
        }
    }

	private void SelectUnit(Unit unit) {
		if (selectedUnit != null && unit.Equals(selectedUnit)) {
            return;
        }

        TileManager.Instance.DeactivateTiles();
        UnselectUnit();

		if (unit.GetTeamId() == teamId) {
			unit.SetSelected(true);
			unit.GetMoveComponent().ToggleMovement();
        } else {
            
        }

		selectedUnit = unit;
    }

    private void UnselectUnit() {
        if (selectedUnit != null) {
			selectedUnit.SetSelected(false);
            selectedUnit = null;
        }
    }
}
