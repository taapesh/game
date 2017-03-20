using UnityEngine;

public class InteractManager : MonoBehaviour {
    private const int LEFT_MOUSE_BUTTON = 0;
    private const int RIGHT_MOUSE_BUTTON = 1;
    private LayerMask mask = -1;
    private Unit selectedUnit;
    private GameManager gm;
    private UserData user;
    public int teamId;

    public enum PlayerState {
        CreateReady,
        MoveReady,
        AbilityReady,
        Default
    };

    private PlayerState playerState = PlayerState.Default;

    void Awake() {
        this.gm = GameManager.Instance;
        GameManager.OnTurnChanged += OnTurnChanged;
        GameManager.OnUnlock += OnUnlock;
    }

    void Update () {
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
                if (GetPlayerState() == PlayerState.AbilityReady) {
                    
                } else {
                    AttemptSelect(col);
                }
            }
        }
    }

    void OnGUI() {
        if (gm.GetBuild().HasSlot(1) && GUI.Button(new Rect(10, Screen.height - 80, 60, 60),
            gm.GetBuild().GetUnit(1).name)) {
            
            if (gm.CanSummon(teamId, 1)) {
                Debug.Log("Creating " + gm.GetBuild().GetUnit(1).name);
            } else {
                Debug.Log("Can't create unit");
            }
        }

        if (gm.GetBuild().HasSlot(2) && GUI.Button(new Rect(100, Screen.height - 80, 60, 60),
            gm.GetBuild().GetUnit(2).name)) {
            
            if (gm.CanSummon(teamId, 1)) {
                Debug.Log("Creating " + gm.GetBuild().GetUnit(2).name);
            } else {
                Debug.Log("Can't create unit");
            }
        }
    }

    public void SetPlayerState(PlayerState playerState) {
        this.playerState = playerState;
    }

    private PlayerState GetPlayerState() {
        return this.playerState;
    }

    /*
     * Perform raycast and return collider if there was a hit
     * Otherwise, return null
     */
    private Collider CheckForHit(Vector3 mousePosition) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value)) {
            return hit.collider;
        }
        return null;
    }

    private void AttemptMove(Collider col) {
        // Check if object is tile
        if (TileManager.Instance.IsTile(col)) {
            // Get tile object
            Tile tile = TileManager.Instance.GetTile(col);

            // Check if tile is valid
            if (GameManager.Instance.IsTileValid(tile.GetTileId())) {
                // Move unit to tile
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
