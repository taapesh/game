using UnityEngine;

public class TileGenerator : MonoBehaviour {
    public GameObject tilePrefab;
    private int numRows = 12;
    private int numColumns = 12;

    void Start () {
        Debug.Log ("Tile size: " +  tilePrefab.GetComponent<Renderer>().bounds.size);

        Vector3 tileBounds = tilePrefab.GetComponent<Renderer>().bounds.size;
        float shiftWidth = tileBounds.x;
        float shiftHeight = (tileBounds.z / 2.0f) + (tileBounds.z / 4.0f);

        float currWidth, currHeight = 0.0f;
        bool evenRow = true;
        int numTilesGenerated = 0;

        for (int i = 0; i < numRows; ++i) {
            currWidth = evenRow ? 0 : currWidth = shiftWidth / 2.0f;
            int numTiles = evenRow ? numColumns + 1 : numColumns;
            evenRow = !evenRow;

            for (int j = 0; j < numTiles; ++j) {
                Vector3 spawn = new Vector3(currWidth, .01f, currHeight);
                GameObject tileInstance = (GameObject) Instantiate(tilePrefab, spawn, Quaternion.identity);
                tileInstance.name = "Tile_" + numTilesGenerated;
                tileInstance.transform.parent = transform;
                tileInstance.AddComponent<TileAttributes>();
                tileInstance.GetComponent<TileAttributes>().tileId = numTilesGenerated;
                currWidth += shiftWidth;
                numTilesGenerated++;
            }

            currHeight += shiftHeight;
        }

        Debug.Log ("Created " + numTilesGenerated + " tiles");
    }
}
