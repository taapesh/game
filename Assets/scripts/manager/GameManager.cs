using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    private const int NUM_PLAYERS = 2;
    private int[] spawnTiles = new int[] { 28, 29 };
    private ArrayList players = new ArrayList();
    private bool locked;
    private Player turnPlayer;
    private int turnNumber;
    private int turnTimer;
    private int secondsPerTurn = 10;
    private int turnId = 0;
    private HashSet<int> validTiles;
    private IEnumerator moveEnum;

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

    public void EndTurn() {
        return;
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
            !unit.HasMoved() &&
            !unit.HasAttacked() &&
            !locked
        );
    }

    public bool CanSummon(Unit unit, Player player) {
        return (
            IsMyTurn(player.GetTeamId()) &&
            player.EnoughEnergy(unit.GetEnergyCost()) &&
            player.GetUnitActiveCount(unit) < unit.GetActiveLimit() &&
            !locked
        );
    }

    public bool IsTileValid(int tileId) {
        return validTiles != null && validTiles.Contains(tileId);    
    }

    public void ToggleUnitMovement(Unit unit) {
        if (CanMove(unit)) {
            this.validTiles = TileManager.Instance
                .TilesInRange(unit.GetTileId(), unit.GetMovementRange());
            TileManager.Instance
                .ActivateTiles(this.validTiles, TileManager.Instance.moveMaterial);
        }
    }

    IEnumerator Move(Unit unit, Tile tile) {
        Vector3 target = tile.GetPosition();
        unit.SetDestination(target);

        while (true) {
            float magnitude = (unit.GetPosition() - target).sqrMagnitude;

            if (magnitude <= unit.GetStopDistance()) {
                StopCoroutine(this.moveEnum);
                this.Unlock();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void MoveUnitToTile(Unit unit, Tile tile) {
        this.Lock();
        tile.SetOccupant(unit);
        tile.ClearOccupant();
        unit.SetTileId(tile.GetTileId());
        unit.SetHasMoved(true);
        this.moveEnum = Move(unit, tile);
        StartCoroutine(this.moveEnum);
    }

    public Player GetPlayer(int teamId) {
        return (Player) players[teamId];
    }

    public void SpawnPlayers() {
        for (int i = 0; i < NUM_PLAYERS; ++i) {
            Tile spawnTile = TileManager.Instance.GetTile(spawnTiles[i]);

            GameObject playerObj = (GameObject) Instantiate(
                playerPrefab,
                spawnTile.GetPosition(),
                Quaternion.identity);
            
            Unit unit = playerObj.GetComponent<Unit>();
            Player player = playerObj.GetComponent<Player>();
            player.SetTeamId(i);
            unit.SetTeamId(i);
            unit.SetTileId(spawnTiles[i]);
            spawnTile.SetOccupant(unit);
            players.Add(player);
        }
    }
}
