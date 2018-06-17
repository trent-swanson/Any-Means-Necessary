using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Agent {

    [Space]
    [Space]
    [Header("Available Actions")]
    public bool hide;

    void Awake() {
        Init();
    }

    void Start() {
        if (hide) {
            HideAction hideAction = new HideAction(this, "Hide");
            actionList.Add(hideAction);
        }
    }

    void Update() {
        Debug.DrawRay(transform.position, transform.forward);

        //if not my turn then don't run Update()
        if (!turn)
            return;

        if (!moving && unitActions > 0) {
            FindSelectableTiles();
            MouseClick();
        }
        else {
            Move();
        }
    }

    void MouseClick() {
        if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "Tile") {
                    Tile t = hit.collider.GetComponent<Tile>();
                    if(t.selectable) {
                        MoveToTile(t);
                    }
                }
            }
        }
    }

    //Actions
    class HideAction : Actions {
        PlayerController unit;

        public HideAction(PlayerController p_unit, string p_name) {
            unit = p_unit;
            actionName = p_name;
        }

        public override void Action() {
            Debug.Log("HIDE");
        }
    }
}
