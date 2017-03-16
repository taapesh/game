﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : Singleton<TileManager> {
    private const int   NUM_ROWS = 12;
    private const int   TILES_IN_ROW = 12;
    public GameObject   tilePrefab;
    private ArrayList   activeTiles = new ArrayList();
    public Tile[]       tilesArray;
    public Material     originalMaterial;
    public Material     moveMaterial;
    public Material     summonMaterial;

    public void InitTileArray(int numTiles) {
        tilesArray = new Tile[numTiles];
        int i = 0;

        foreach (Transform tileObj in transform) {
            Tile tile = tileObj.GetComponent<Tile>();
            tilesArray[i] = tile;
            tilesArray[i].SetTileId(i);
            i++;
        }
    }

    public HashSet<int> TilesInRange(int tileId, int range, bool includeCenter=false, bool includeBlocked=false)  {
        HashSet<int> tileIds = new HashSet<int>();

        if (range == 0) {
            return tileIds;
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

        return tileIds;
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
            if (idx >= 0 && idx < tilesArray.Length) {
                if (includeBlocked || IsTileAvailable(idx)) {
                    tiles.Add(idx);
                }
            }
        }

        return tiles;
    }

    public bool IsTile(Collider col) {
        return col.gameObject.tag == "tile";
    }

    public int GetTileId(Collider col) {
        return col.GetComponent<Tile>().GetTileId();
    }

    public Tile GetTile(int tileId) {
        return tilesArray[tileId];
    }

    public bool IsTileAvailable(int tileId) {
        return tilesArray[tileId].IsAvailable();
    }

    public void ActivateTiles(HashSet<int> tileIds, Material material) {
        foreach (int tileId in tileIds) {
            tilesArray[tileId].SetMaterial(material);
            activeTiles.Add(tileId);
        }
    }

    public void DeactivateTiles() {
        foreach (int tileId in activeTiles) {
            tilesArray[tileId].SetMaterial(originalMaterial);
        }
        activeTiles.Clear();
    }

    public int GenerateTiles() {
        //Debug.Log ("Tile size: " +  tilePrefab.GetComponent<Renderer>().bounds.size);

        Vector3 tileBounds = tilePrefab.GetComponent<Renderer>().bounds.size;
        float shiftWidth = tileBounds.x;
        float shiftHeight = (tileBounds.z / 2.0f) + (tileBounds.z / 4.0f);

        float currWidth, currHeight = 0.0f;
        bool evenRow = true;
        int numTilesGenerated = 0;

        for (int i = 0; i < NUM_ROWS; ++i) {
            currWidth = evenRow ? 0 : currWidth = shiftWidth / 2.0f;
            int numTiles = evenRow ? TILES_IN_ROW + 1 : TILES_IN_ROW;

            for (int j = 0; j < numTiles; ++j) {
                Vector3 spawn = new Vector3(currWidth, .01f, currHeight);
                GameObject tileInstance = (GameObject) Instantiate(tilePrefab, spawn, Quaternion.identity);
                tileInstance.name = "Tile_" + numTilesGenerated;
                tileInstance.transform.parent = transform;
                currWidth += shiftWidth;
                numTilesGenerated++;
            }

            currHeight += shiftHeight;
            evenRow = !evenRow;
        }

        Debug.Log ("Created " + numTilesGenerated + " tiles");
        return numTilesGenerated;
    }
}
