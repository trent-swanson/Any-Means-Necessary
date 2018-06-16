﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public bool walkable = true;
    public bool current = false;
    public bool target = false;
    public bool selectable = false;

    public List<Tile> adjacencyList = new List<Tile>();

    //BFS vars
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;

    //0 defualt, 1 active, 2 sprint, 3 blocked, 4 target, 5 current
    public List<Material> matList = new List<Material>();
    public enum matEnum {defualt, active, sprint, blocked, target, current};

    Renderer myRenderer;

    private void Start() {
        myRenderer = GetComponent<Renderer>();
        CheckAllIfOccupied();
    }

    void Update() {
        //Updates tile material to display its state
        if (current) {
            myRenderer.material = matList[(int)matEnum.current];
        }
        else if (target) {
            myRenderer.material = matList[(int)matEnum.target];
        }
        else if (selectable) {
            myRenderer.material = matList[(int)matEnum.active];
        }
        else {
            myRenderer.material = matList[(int)matEnum.defualt];
        }
    }

    //find all adjacent tiles
    public void FindNeighbors(float p_jumpHeight) {
        Reset();

        CheckTile(Vector3.forward, p_jumpHeight);
        CheckTile(-Vector3.forward, p_jumpHeight);
        CheckTile(Vector3.right, p_jumpHeight);
        CheckTile(Vector3.left, p_jumpHeight);
    }

    //check if a adjacent tile is walkable and not occupied and add it to adjacentcy list
    public void CheckTile(Vector3 p_direction, float p_jumpHeight) {
        Vector3 halfExtents = new Vector3(0.5f, (1 + p_jumpHeight) / 2.0f, 0.5f);
        Collider[] colliders = Physics.OverlapBox(transform.position + p_direction, halfExtents);

        foreach (Collider item in colliders) {
            Tile tile = item.GetComponent<Tile>();
            if (tile != null && tile.walkable) {
                RaycastHit hit;
                if (!Physics.Raycast(tile.transform.position, Vector3.up, out hit, 1.0f)) {
                    adjacencyList.Add(tile);
                }
            }
        }
    }

    //check all tiles to see if they are occupied and set walkable and assaign tile to player
    public void CheckAllIfOccupied() {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 5, transform.position.z), Vector3.up, out hit, 10)) {
            if (hit.transform.tag == "Obsticle" || hit.transform.tag == "Enemy") {
                walkable = false;
            }
            else if (hit.transform.tag == "Player") {
                myRenderer.material = matList[(int)matEnum.active];
            }
        }
    }

    //reset tile values
    public void Reset() {
        adjacencyList.Clear();

        current = false;
        target = false;
        selectable = false;

        visited = false;
        parent = null;
        distance = 0;
    }
}
