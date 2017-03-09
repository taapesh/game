using UnityEngine;

public class GameManager : MonoBehaviour {
    private TileManager tileManager;
    private int turnId = 0;

    void Awake() {
        tileManager = GameObject.Find("TileManager").GetComponent<TileManager>();
    }

    void Update() {

    }

    public bool IsMyTurn(int teamId) {
        return turnId == teamId;
    }

    public bool MoveUnit(UnitAttributes attr, int tileId) {
        if (!tileManager.IsTileAvailable(tileId)) {
            return false;
        }

        tileManager.SetOccupant(tileId, attr.gameObject);
        tileManager.ClearOccupant(attr.tileId);
        attr.hasMoved = true;
        attr.tileId = tileId;
        UnityEngine.AI.NavMeshAgent nav = attr.GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav.SetDestination(tileManager.GetTile(tileId).position);
        return true;
    }
}
