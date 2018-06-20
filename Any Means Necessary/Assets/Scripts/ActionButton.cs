using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour {

	PlayerController player;
	Actions action;

	public void Initilise(PlayerController p_player, Actions p_action) {
		name = p_action.actionName;
		transform.GetChild(0).GetComponent<Text>().text = p_action.actionName;

		player = p_player;
		action = p_action;

		Button btn = transform.GetComponent<Button>();
		btn.onClick.AddListener(ButtonPress);
	}

	void ButtonPress() {
		action.SetAction(player);
	}
}
