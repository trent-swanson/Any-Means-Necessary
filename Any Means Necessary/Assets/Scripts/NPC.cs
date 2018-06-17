using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Agent {

	GameObject target;

	[Space]
	[Space]
	[Header("Waypoints")]
	public List<GameObject> waypoints = new List<GameObject>();
	public int currentWaypoint = 0;

	void Start() {
		Init();
	}

	void Update() {
		Debug.DrawRay(transform.position, transform.forward);

        //if not my turn then don't run Update()
        if (!turn)
            return;

        if (!moving && unitActions > 0) {
			//FindNearestTarget();
			NextWaypoint();
			CalculatePath();
            FindSelectableTiles();
			actualTargetTile.target = true;
        }
        else {
            Move();
        }
    }

	void CalculatePath() {
		Tile targetTile = GetTargetTile(target);
		if (waypoints.Count > 0)
			FindPath(targetTile, true);
		else
			FindPath(targetTile, false);
	}

	void NextWaypoint() {
		if (waypoints.Count == 0) {
			FindNearestTarget();
			return;
		}
		if (Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position) < 0.15f) {
			currentWaypoint++;
			if (currentWaypoint >= waypoints.Count) {
				currentWaypoint = 0;
			}
		}
		target = waypoints[currentWaypoint];
	}

	void FindNearestTarget() {
		//find all players, change this to accept waypoints
		GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

		GameObject nearest = null;
		float distance = Mathf.Infinity;

		//find closes target in targets array
		foreach (GameObject obj in targets) {
			//SqrMagnitude is more efficent than vector3.Distance
			float dis = Vector3.SqrMagnitude(transform.position - obj.transform.position);
			if (dis < distance) {
				distance = dis;
				nearest = obj;
			}
		}

		target = nearest;
	}
}
