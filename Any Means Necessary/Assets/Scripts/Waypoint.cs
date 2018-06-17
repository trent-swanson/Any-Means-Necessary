using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {

	public Tile waypointTile;

	void Start() {
		RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2.0f)) {
			if (hit.transform.tag == "Tile") {
				waypointTile = hit.transform.GetComponent<Tile>();
			}
        }
	}
}
