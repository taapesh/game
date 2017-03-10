using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
    private int turnId = 0;
    private HashSet<int> validMovementTiles;

    public bool IsMyTurn(int teamId) {
        return turnId == teamId;
    }

    public void ToggleMovement(UnitAttributes attr) {
        if (CanMove(attr)) {
            validMovementTiles = TileManager.Instance.TilesInRange(attr.tileId, attr.movementRange);
            TileManager.Instance.ActivateTiles(validMovementTiles, TileManager.Instance.moveMaterial);
        }
    }

    public bool CanMove(UnitAttributes attr) {
        return (
            attr.isSelected &&
            IsMyTurn(attr.teamId) &&
            !attr.hasMoved &&
            !attr.hasAttacked
        );
    }

    public bool MoveUnitToTile(UnitAttributes attr, int tileId) {
        if (!validMovementTiles.Contains(tileId)) {
            return false;
        }

        TileManager.Instance.DeactivateTiles();
        TileManager.Instance.SetOccupant(tileId, attr.gameObject);
        TileManager.Instance.ClearOccupant(attr.tileId);
        attr.hasMoved = true;
        attr.tileId = tileId;
        UnityEngine.AI.NavMeshAgent nav = attr.GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav.SetDestination(TileManager.Instance.GetTile(tileId).position);
        return true;
    }
}
