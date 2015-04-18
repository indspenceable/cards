using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public int x;
	public int y;
	public Stage stage;
	public Unit unit;

	private bool selected;

	private Camera camera;
	private Collider2D collider;

	// Use this for initialization
	void Start () {
		this.camera = Camera.main;
		this.collider = GetComponent<Collider2D>();
	}

	void OnDrawGizmos() {
		if (selected) {
			Gizmos.DrawWireSphere(transform.position, 0.5f);
		}
	}

	public void Select() {
		selected = true;
	}

	public void Unselect() {
		selected = false;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseOver() {
		
	}

}
