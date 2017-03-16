using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	private IEnumerator moveEnum;
    private LayerMask mask = -1;
    private Unit unit;
    private HashSet<int> availableTiles;
	private UnityEngine.AI.NavMeshAgent nav;

    void Awake() {
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        unit = GetComponent<Unit>();
    }

    void Update() {
        if (!GameManager.Instance.CanMove(unit)) {
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

                if (availableTiles.Contains(tileId)) {
					TileManager.Instance.DeactivateTiles();
					this.moveEnum = Move(TileManager.Instance.GetTile (tileId));
					StartCoroutine(this.moveEnum);
                } else {
                    // Tile is out of range or occupied
                }
            } else {
                // Not a tile
            }
        }
    }

	IEnumerator Move(Tile tile) {
		GameManager.Instance.Lock();
		GameManager.Instance.MoveUnitToTile(unit, tile);
		Vector3 target = tile.GetPosition();
		nav.SetDestination(target);

        while (true) {
			float distance = Vector3.Distance(unit.GetPosition(), target);

            if (distance < 0.5f) {
				StopCoroutine(this.moveEnum);
                GameManager.Instance.Unlock();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void ToggleMovement() {
        if (GameManager.Instance.CanMove(unit)) {
            availableTiles = TileManager.Instance.TilesInRange(unit.GetTileId(), unit.GetMovementRange());
            TileManager.Instance.ActivateTiles(availableTiles, TileManager.Instance.moveMaterial);
        }
    }
}
