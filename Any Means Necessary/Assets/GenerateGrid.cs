using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenerateGrid : MonoBehaviour {

    public GameObject tilePrefab;

    public Vector2 gridSize;

    [Range(0,1)]
    public float tilePadding;

    [ContextMenu("GenerateGrid")]

    private void Start() {
        GameManager.grid = new GameObject[(int)gridSize.x, (int)gridSize.y];
    }

    public void Generate() {

        string holderName = "Generate Grid";
        if (transform.Find(holderName)) {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform gridHolder = new GameObject(holderName).transform;
        gridHolder.parent = transform;

        for (int x = 0; x < gridSize.x; x++) {
            for (int y = 0; y < gridSize.y; y++) {
                Vector3 tilePosition = new Vector3(-gridSize.x / 2 + 0.5f + x, 0.01f, -gridSize.y / 2 + 0.5f + y);
                GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.right*90));
                newTile.transform.localScale = Vector3.one*(1 - tilePadding);
                newTile.transform.SetParent(gridHolder);
                GameManager.grid[x, y] = newTile;
            }
        }
    }
}
