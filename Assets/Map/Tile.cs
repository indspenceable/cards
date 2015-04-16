using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public int x;
	public int y;
	public Stage stage;
	public Unit unit;

	private Camera camera;
	private Collider2D collider;

	// Use this for initialization
	void Start () {
		this.camera = Camera.main;
		this.collider = GetComponent<Collider2D>();
	}

	void OnDrawGizmos() {
		Vector2 v = camera.ViewportToWorldPoint(camera.ScreenToViewportPoint(Input.mousePosition));
		if (this.collider.OverlapPoint(v)) {
			Gizmos.DrawWireSphere( v, 1f );
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseOver() {
		
	}

}
