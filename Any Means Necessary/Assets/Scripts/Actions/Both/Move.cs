using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Actions/Both/Move", order = 1)]
public class Move : Actions {
    public override void SetAction(Agent p_agent) {
        Debug.Log("Set up Move");
    }

    public override void DoAction(Agent p_agent) {

    }
}
