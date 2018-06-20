using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI : MonoBehaviour {

    public GameObject actionButton;

    private void OnEnable() {
        TurnManager.OnUnitSelect += UnitSelected;
        TurnManager.OnUnitDeselect += UnitDeselected;
    }

    private void OnDisable() {
        TurnManager.OnUnitSelect -= UnitSelected;
        TurnManager.OnUnitDeselect -= UnitDeselected;
    }

    void UnitSelected(PlayerController p_unit) {
        foreach (Actions action in p_unit.actionList) {
            GameObject btn = Instantiate(actionButton, this.transform);
            btn.GetComponent<ActionButton>().Initilise(p_unit, action);
        }
    }

    void UnitDeselected() {
        foreach (Transform btn in transform) {
            DestroyObject(btn.gameObject);
        }
    }
}
