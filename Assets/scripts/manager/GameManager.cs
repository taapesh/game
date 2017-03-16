using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    private const int NUM_PLAYERS = 2;
    private int[] SPAWN_TILES = new int[] { 28, 29 };
    private bool locked;
    private int turnNumber;
    private int turnTimer;
    private int secondsPerTurn = 10;
    private int turnId = 0;

    public delegate void OnTurnChangedEvent(int turnId);
    public static event OnTurnChangedEvent OnTurnChanged;

    public delegate void OnUnlockEvent();
    public static event OnUnlockEvent OnUnlock;

    public GameObject playerPrefab;

    void Start() {
        InitGame();
    }

    void InitGame() {
        int numTiles = TileManager.Instance.GenerateTiles();
        TileManager.Instance.InitTileArray(numTiles);
        SpawnPlayers();
        turnNumber = 1;
        turnTimer = secondsPerTurn;
        InvokeRepeating("CountdownTurn", 0f, 1f);
    }

    public void Lock() {
        locked = true;
    }

    public void Unlock() {
        locked = false;
        OnUnlock();
    }

    private void CountdownTurn() {
        turnTimer -= 1;

        if (turnTimer == 0) {
            ChangeTurns();
        }
    }

    private void ChangeTurns() {
        turnNumber++;
        turnTimer = secondsPerTurn;
        turnId = 1 - turnId;
        OnTurnChanged(turnId);
    }

    public bool IsMyTurn(int teamId) {
        return turnId == teamId;
    }

    public bool CanMove(Unit unit) {
        return (
            unit.IsSelected() &&
            IsMyTurn(unit.GetTeamId()) &&
            !locked &&
            !unit.HasMoved() &&
            !unit.HasAttacked()
        );
    }

    public bool MoveUnitToTile(Unit unit, Tile tile) {      
        tile.SetOccupant(unit);
        tile.ClearOccupant();
        unit.SetTileId(tile.GetTileId());
        unit.SetHasMoved(true);      
        return true;
    }

    public void SpawnPlayers() {
        for (int i = 0; i < NUM_PLAYERS; ++i) {
            Tile spawnTile = TileManager.Instance.GetTile(SPAWN_TILES[i]);
            Vector3 spawnPosition = spawnTile.GetPosition();
            GameObject player = (GameObject) Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            Unit unit = player.GetComponent<Unit>();
            unit.SetTeamId(i);
            unit.SetTileId(SPAWN_TILES[i]);
            spawnTile.SetOccupant(unit);
        }
    }
}
