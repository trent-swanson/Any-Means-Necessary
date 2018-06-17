using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Agent {

	GameObject target;

	void Start() {
		Init();
	}

	void Update() {
		Debug.DrawRay(transform.position, transform.forward);

        //if not my turn then don't run Update()
        if (!turn)
            return;

        if (!moving) {
			FindNearestTarget();
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
		FindPath(targetTile);
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
