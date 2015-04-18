using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {
	public Tile tile;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MoveTo(Tile t) {
		if (tile != null) {
			tile.unit = null;
		}
		tile = t;
		t.unit = this;

		transform.position = t.transform.position;
		transform.Translate(0,0,-1);
	}
}
