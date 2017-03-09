using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour {
    private GameManager gameManager;
    private TileManager tileManager;
    private LayerMask mask = -1;
    private UnitAttributes attr;
    private ArrayList tilesInRange;

    void Awake() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
        attr = GetComponent<UnitAttributes>();
    }

    void Update() {
        if (!CanMove()) {
            return;
        }

        if (Input.GetMouseButtonDown(1)) {
            CheckForMovement(Input.mousePosition);
        }
    }

    private void CheckForMovement(Vector3 mousePosition) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value)) {
            if (CanMoveHere(hit.collider)) {
                // TODO: Visual effects for turn player
                tileManager.DeactivateTiles();
                int tileId = hit.collider.GetComponent<TileAttributes>().tileId;
                bool moved = gameManager.MoveUnit(attr, tileId);

                if (!moved) {

                } else {

                }
            }
        }
    }

    public void ToggleMovement() {
        if (CanMove()) {
            tilesInRange = tileManager.TilesInRange(attr.tileId, attr.movementRange);
            tileManager.ActivateTiles(tilesInRange, tileManager.activeMaterial);
        }
    }

    private bool CanMoveHere(Collider col) {
        return (
            col.GetComponent<TileAttributes>() != null &&
            tilesInRange.Contains(col.transform)
        );
    }

    public bool CanMove() {
        return (
            attr.isSelected &&
            gameManager.IsMyTurn(attr.teamId) &&
            !attr.hasMoved &&
            !attr.hasAttacked
        );
    }
}
