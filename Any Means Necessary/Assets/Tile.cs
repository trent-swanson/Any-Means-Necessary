using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public bool walkable = true;
    GameObject occupingObject;

    //0 defualt, 1 active, 2 sprint, 3 blocked
    public List<Material> matList = new List<Material>();

    Renderer myRenderer;

    private void Start() {
        myRenderer = GetComponent<Renderer>();
        CheckAllIfOccupied();
    }

    public void CheckAllIfOccupied() {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 5, transform.position.z), Vector3.up, out hit, 10)) {
            if (hit.transform.tag == "Obsticle" || hit.transform.tag == "Enemy") {
                occupingObject = hit.transform.gameObject;
                walkable = false;
                myRenderer.material = matList[3];
            }
            else if (hit.transform.tag == "Player") {
                occupingObject = hit.transform.gameObject;
                occupingObject.GetComponent<PlayerController>().onTile = this;
                myRenderer.material = matList[1];
            }
        }
    }
}
