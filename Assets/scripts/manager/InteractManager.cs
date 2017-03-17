using UnityEngine;

public class InteractManager : MonoBehaviour {
    private const int LEFT_MOUSE_BUTTON = 0;
    private const int RIGHT_MOUSE_BUTTON = 1;
    private LayerMask mask = -1;
    private Unit selectedUnit;
    public int teamId;

    private enum PlayerState {
        CreateReady,
        MoveReady,
        AttackReady,
        Default
    };

    private PlayerState playerState;

    void Awake() {
        SetPlayerState(PlayerState.Default);
        GameManager.OnTurnChanged += OnTurnChanged;
        GameManager.OnUnlock += OnUnlock;
    }

    void Update () {
        // Determine what was clicked on
        if (Input.GetMouseButtonDown(RIGHT_MOUSE_BUTTON)) {
            Collider col = CheckForHit(Input.mousePosition);

            if (col != null) {
                if (GetPlayerState() == PlayerState.MoveReady) {
                    // Check for tile hit
                    AttemptMove(col);
                }
            }
        }

        if (Input.GetMouseButtonDown(LEFT_MOUSE_BUTTON)) {
            Collider col = CheckForHit(Input.mousePosition);

            if (col != null) {
                if (GetPlayerState() == PlayerState.AttackReady) {
                    
                } else {
                    AttemptSelect(col);
                }
            }
        }
    }

    private void SetPlayerState(PlayerState playerState) {
        this.playerState = playerState;
    }

    private PlayerState GetPlayerState() {
        return this.playerState;
    }

    private Collider CheckForHit(Vector3 mousePosition) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value)) {
            return hit.collider;
        }
        return null;
    }

    private void AttemptMove(Collider col) {
        if (TileManager.Instance.IsTile(col)) {
            Tile tile = TileManager.Instance.GetTile(col);

            if (GameManager.Instance.IsTileValid(tile.GetTileId())) {
                GameManager.Instance.MoveUnitToTile(selectedUnit, tile);
                TileManager.Instance.DeactivateTiles();
            }
        } else {
            // Not tile
        }
    }

    private void AttemptSelect(Collider col) {
        Unit unit = col.GetComponent<Unit>();

        if (unit != null) {
            // Selected unit
            SelectUnit(unit);
        }
    }

    private void SelectUnit(Unit unit) {
        if (selectedUnit != null && unit.Equals(selectedUnit)) {
            // Selected same unit, do nothing
            return;
        }

        TileManager.Instance.DeactivateTiles();
        UnselectUnit();

        if (unit.IsFriendly(teamId)) {
            // Selected friendly unit
            unit.SetSelected(true);
            selectedUnit = unit;
            CheckForMovement();
        } else {
            // Selected hostile unit
            selectedUnit = unit;
        }
    }

    private void UnselectUnit() {
        if (selectedUnit != null) {
            selectedUnit.SetSelected(false);
            selectedUnit = null;
        }
    }

    private void CheckForMovement() {
        if (selectedUnit != null && selectedUnit.IsFriendly(teamId)) {
            if (GameManager.Instance.CanMove(selectedUnit)) {
                GameManager.Instance.ToggleUnitMovement(selectedUnit);
                SetPlayerState(PlayerState.MoveReady);
            }
        }
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
}
