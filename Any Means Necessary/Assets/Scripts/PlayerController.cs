using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Agent {

    void Awake() {
        Init();
    }

    void Update() {
        Debug.DrawRay(transform.position, transform.forward);

        //if not my turn then don't run Update()
        if (!turn)
            return;

        if (!moving && unitActionPoints > 0) {
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
}
