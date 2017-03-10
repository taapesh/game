using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    private LayerMask mask = -1;
    private UnitAttributes attr;
    private Transform target;
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
                bool moved = GameManager.Instance.MoveUnitToTile(gameObject, attr, tileId);

                if (moved) {
                    
                } else {
                    // Tile is out of range or occupied
                }
            } else {
                // Not a tile
            }
        }
    }

    private void CheckForDestination() {
        Vector3 currentPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 targetPos = new Vector3(target.position.x, 0, target.position.z);

        float distance = Vector3.Distance(currentPos, targetPos);
        Debug.Log (distance);
        if (distance < 0.5f) {
            CancelInvoke("CheckForDestination");
            GameManager.Instance.Unlock();
        }
    }

    IEnumerator Move() {
        while (true) {
            Vector3 currentPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 targetPos = new Vector3(target.position.x, 0, target.position.z);
            float distance = Vector3.Distance(currentPos, targetPos);

            if (distance < 0.5f) {
                StopCoroutine("Move");
                GameManager.Instance.Unlock();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetDestination(Transform tile) {
        target = tile;
        StartCoroutine("Move");
    }
}
