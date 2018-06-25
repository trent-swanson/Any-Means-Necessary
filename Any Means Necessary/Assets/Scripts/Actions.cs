using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actions : ScriptableObject {
	public string actionName;
	public int actionCost;
    public Agent agent;
	public abstract void SetAction (Agent p_agent);
    public abstract void DoAction();
}