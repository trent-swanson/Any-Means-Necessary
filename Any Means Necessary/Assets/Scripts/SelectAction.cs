using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAction : MonoBehaviour {

	public GameObject actionsPanel;
	public GameObject actionsButton;

	void OnEnable()
    {
        TurnManager.OnUnitSelect += UnitSelected;
		TurnManager.OnUnitDeselect += UnitUnselected;
    }    
    
    void OnDisable()
    {
        TurnManager.OnUnitSelect -= UnitSelected;
		TurnManager.OnUnitDeselect -= UnitUnselected;
    }


	void UnitSelected(PlayerController p_unit) {
		actionsPanel.SetActive(true);
		for (int i = 0; i < p_unit.actionList.Count; i++) {
			GameObject temp = Instantiate(actionsButton);
			temp.transform.SetParent(actionsPanel.transform);
			temp.GetComponent<ActionButton>().Initilise(p_unit, p_unit.actionList[i]);
		}
	}

	

	void UnitUnselected() {
		foreach (Transform child in actionsPanel.transform) {
			Destroy(child.gameObject);
		}
		actionsPanel.SetActive(false);
	}

}
