using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    private LayerMask mask = -1;
    private UnitAttributes attr;
    private HashSet<int> availableTiles;

    void Awake() {
        attr = GetComponent<UnitAttributes>();
    }

    void Update() {
        if (!GameManager.Instance.CanMove(attr)) {
            return;
        }

        if (Input.GetMouseButtonDown(1)) {
            AttemptMove(Input.mousePosition);
        }
    }

    private void AttemptMove(Vector3 mousePosition) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value)) {
            if (TileManager.Instance.IsTile(hit.collider)) {
                int tileId = TileManager.Instance.GetTileId(hit.collider);
                bool moved = GameManager.Instance.MoveUnitToTile(attr, tileId);

                if (moved) {
                    
                } else {
                    // Tile is out of range or occupied
                }
            } else {
                // Not a tile
            }
        }
    }

    public void ToggleMovement() {
        
    }
}
