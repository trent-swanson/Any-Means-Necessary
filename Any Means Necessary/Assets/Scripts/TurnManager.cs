using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

	static Dictionary<string, List<Agent>> units = new Dictionary<string, List<Agent>>();
	static Queue<string> turnKey = new Queue<string>();
	static Queue<Agent> turnTeam = new Queue<Agent>();

	void Update() {
		if (turnTeam.Count == 0) {
			InitTeamTurnMove();
		}
	}

	static void InitTeamTurnMove() {
		List<Agent> teamList = units[turnKey.Peek()];

		foreach (Agent unit in teamList) {
			turnTeam.Enqueue(unit);
		}

		StartTurn();
	}

	public static void StartTurn() {
		if (turnTeam.Count > 0) {
			turnTeam.Peek().BeginTurn();
		}
	}

	public static void EndTurn() {
		Agent unit = turnTeam.Dequeue();
		unit.EndTurn();

		if (turnTeam.Count > 0) {
			StartTurn();
		}
		else {
			string team = turnKey.Dequeue();
			turnKey.Enqueue(team);
			InitTeamTurnMove();
		}
	}

	public static void AddUnit(Agent p_unit) {
		List<Agent> list;

		if (!units.ContainsKey(p_unit.tag)) {
			list = new List<Agent>();
			units[p_unit.tag] = list;

			if (!turnKey.Contains(p_unit.tag)) {
				turnKey.Enqueue(p_unit.tag);
			}
		}
		else {
			list = units[p_unit.tag];
		}

		list.Add(p_unit);
	}
}
