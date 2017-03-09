﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {
    private const int   TILES_IN_ROW = 12;
    private int         lenTileArray;
    private ArrayList   activeTiles = new ArrayList();
    public Transform[]  tilesArray;
    public GameObject[] occupants;
    public Material     originalMaterial;
    public Material     activeMaterial;
    public Material     summonMaterial;

    void Awake() {
        // Copy component values in production
        int i = 0;
        foreach (Transform child in gameObject.transform) {
            tilesArray[i++] = child;
        }

        lenTileArray = tilesArray.Length;
    }

    public ArrayList TilesInRange(int tileId, int range, bool includeCenter=false, bool includeBlocked=false)  {
        HashSet<int> tileIds = new HashSet<int>();
        ArrayList tiles = new ArrayList();

        if (range == 0) {
            return tiles;
        }

        if (includeCenter) {
            tileIds.Add(tileId);
        }

        ArrayList currentLevel = GetNeighbors(tileId, includeBlocked);

        foreach(int id in currentLevel) {
            tileIds.Add(id);
        }

        for (int i = 2; i <= range; ++i) {
            ArrayList nextLevel = new ArrayList();

            foreach (int id in currentLevel) {
                ArrayList neighbors = GetNeighbors(id, includeBlocked);

                foreach (int neighbor in neighbors) {
                    if (!tileIds.Contains(neighbor) && neighbor != tileId) {
                        nextLevel.Add(neighbor);
                        tileIds.Add(neighbor);
                    }
                }
            }

            currentLevel = nextLevel;
        }

        foreach(int id in tileIds) {
            tiles.Add(tilesArray[id]);
        }

        return tiles;
    }

    private ArrayList GetNeighbors(int id, bool includeBlocked) {
        int[] neighborIds = new int[] {
            id + 1,
            id - 1,
            id + TILES_IN_ROW,
            id + TILES_IN_ROW + 1,
            id - TILES_IN_ROW - 1,
            id - TILES_IN_ROW
        };

		ArrayList tiles = new ArrayList();

        foreach (int idx in neighborIds) {
            if (idx >= 0 && idx < lenTileArray) {
                if (includeBlocked || IsTileAvailable(idx)) {
                    tiles.Add(idx);
                }
            }
        }

        return tiles;
    }

    public Transform GetTile(int tileId) {
        return tilesArray[tileId];
    }

    public bool IsTileAvailable(int tileId) {
        return occupants[tileId] == null;
    }

    public void ClearOccupant(int tileId) {
        occupants[tileId] = null;    
    }

    public void SetOccupant(int tileId, GameObject unit) {
        occupants[tileId] = unit;
    }

    public void ActivateTiles(ArrayList tiles, Material material) {
        foreach (Transform tile in tiles) {
            tile.GetComponent<Renderer>().material = material;
            activeTiles.Add(tile);
        }
    }

    public void DeactivateTiles() {
        foreach(Transform tile in activeTiles) {
            tile.GetComponent<Renderer>().material = originalMaterial;
        }
        activeTiles.Clear();
    }
}