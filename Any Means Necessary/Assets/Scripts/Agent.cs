using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {

	public bool turn = false;

	List<Tile> selectableTiles = new List<Tile>();
	GameObject[] tiles;

	Stack<Tile> path = new Stack<Tile>();
	public Tile currentTile;

	public bool moving = false;
	public int move = 2;
	public int sprint = 4;
	public float jumpHeight = 1;
	public float moveSpeed = 2;
	public float jumpVelocity = 4.5f;
	public float jumpUpPop = 0.7f;
	public float jumpUpVelSlow = 1.2f;
	public float jumpDownPop = 2.7f;
	public float jumpDownVelSlow = 2.5f;

	Vector3 velocity = new Vector3();
	Vector3 heading = new Vector3();

	float halfHeight;

	bool fallingDown = false;
	bool jumpingUp = false;
	bool movingToEdge = false;
	Vector3 jumpTarget;

	//Initialise agents
	protected void Init() {
		halfHeight = GetComponent<Collider>().bounds.extents.y; 
		tiles = GameObject.FindGameObjectsWithTag("Tile");
		TurnManager.AddUnit(this);
	}

	public void GetCurrentTile() {
		currentTile = GetTargetTile(gameObject);
		currentTile.current = true;
	}

	public Tile GetTargetTile(GameObject p_target) {
		RaycastHit hit;
		Tile tile = null;
		if (Physics.Raycast(p_target.transform.position, Vector3.down, out hit, 2.0f)) {
			tile = hit.collider.GetComponent<Tile>();
		}
		return tile;
	}

	//get all adjacent tiles for each tile in grid and assign them to that tiles adjacentcy list
	public void ComputeAdjacentcyLists() {
		foreach (GameObject tile in tiles) {
			Tile t = tile.GetComponent<Tile>();
			t.FindNeighbors(jumpHeight);
		}
	}

	//process the current tile and its adjacent tiles and their adjacent tiles if in move range to find selectable tiles
	public void FindSelectableTiles() {
		ComputeAdjacentcyLists();
		GetCurrentTile();

		Queue<Tile> process = new Queue<Tile>();

		process.Enqueue(currentTile);
		currentTile.visited = true;

		while (process.Count > 0) {
			Tile t = process.Dequeue();

			selectableTiles.Add(t);
			t.selectable = true;

			if(t.distance < move) {
				foreach (Tile tile in t.adjacencyList) {
					if (!tile.visited) {
						tile.parent = t;
						tile.visited = true;
						tile.distance = 1 + t.distance;
						process.Enqueue(tile);
					}
				}
			}
		}
	}

	//Get Path in reverse order
	public void MoveToTile(Tile p_tile) {
        path.Clear();
		p_tile.target = true;
        moving = true;

		Tile next = p_tile;
		while (next != null) {
			path.Push(next);
			next = next.parent;
		}
    }

	public void Move() {
		if (path.Count > 0) {
			Tile t = path.Peek();
			Vector3 targetPos = t.transform.position;

			//calculate the agents position on top of the target tile
			targetPos.y += halfHeight + t.GetComponent<Collider>().bounds.extents.y;

			if (Vector3.Distance(transform.position, targetPos) >= 0.15f) {
				bool jump = transform.position.y != targetPos.y;

				if (jump) {
					Jump(targetPos);
				} else {
					//calculate move forward
					CalculateHeading(targetPos);
					SetHorizontalVelocity();
				}
				
				//Locomotion (add animations here)
				transform.forward = heading;
				transform.position += velocity * Time.deltaTime;
			}
			else {
				//tile center reached
				transform.position = targetPos;
				path.Pop();
			}
		}
		else {
			RemoveSelectableTiles();
			moving = false;

			//move EndTurn() to after action is complete instead of end of move
			TurnManager.EndTurn();
		}
	}

	protected void RemoveSelectableTiles() {
		if (currentTile != null) {
			currentTile.current = false;
			currentTile = null;
		}
		foreach (Tile tile in selectableTiles) {
			tile.Reset();
		}
		selectableTiles.Clear();
	}

	//calculate the direction we have to head to reach target
	void CalculateHeading(Vector3 p_target) {
		heading = p_target - transform.position;
		heading.Normalize();
	}

	//set the velocity of agent to the heading direction
	void SetHorizontalVelocity() {
		velocity = heading * moveSpeed;
	}

	void Jump(Vector3 p_target) {
		if (fallingDown)
			FallDownward(p_target);
		else if (jumpingUp)
			JumpUpward(p_target);
		else if (movingToEdge)
			MoveToEdge();
		else
			PrepareJump(p_target);
	}

	void PrepareJump(Vector3 p_target) {
		float targetY = p_target.y;
		p_target.y = transform.position.y;

		CalculateHeading(p_target);

		//if heigher
		if (transform.position.y > targetY) {
			fallingDown = false;
			jumpingUp = false;
			movingToEdge = true;

			jumpTarget = transform.position + (p_target - transform.position) / 2.0f;
		}
		//if lower
		else {
			fallingDown = false;
			jumpingUp = true;
			movingToEdge = false;

			//devide velocity to slow down movement while jumping
			velocity = heading * moveSpeed / jumpUpVelSlow;

			float difference = targetY - transform.position.y;
			//jump velocity
			velocity.y = jumpVelocity * (jumpUpPop + difference / 2.0f);
		}
	}

	void FallDownward(Vector3 p_target) {
		velocity += Physics.gravity * Time.deltaTime;

		if (transform.position.y <= p_target.y) {
			fallingDown = false;
			jumpingUp = false;
			movingToEdge = false;

			Vector3 pos = transform.position;
			pos.y = p_target.y;
			transform.position = pos;

			velocity = new Vector3();
		}
	}

	void JumpUpward(Vector3 p_target) {
		velocity += Physics.gravity * Time.deltaTime;

		if (transform.position.y > p_target.y) {
			jumpingUp = false;
			fallingDown = true;
		}
	}

	void MoveToEdge() {
		if (Vector3.Distance(transform.position, jumpTarget) >= 0.05f) {
			SetHorizontalVelocity();
		}
		else {
			movingToEdge = false;
			fallingDown = true;

			//devide velocity to slow down movement while falling
			velocity /= jumpDownVelSlow;
			//add small vertical velocity for 'hop' off edge
			velocity.y = jumpDownPop;
		}
	}

	public void BeginTurn() {
		turn = true;
	}

	public void EndTurn() {
		turn = false;
	}
}
