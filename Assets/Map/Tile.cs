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

	public Color originalColor;
	public Color selectedColor;
	public bool passable;

	// Use this for initialization
	void Start () {
		this.camera = Camera.main;
		this.collider = GetComponent<Collider2D>();
		this.originalColor = GetComponent<SpriteRenderer>().color;
		this.selectedColor = Color.Lerp(originalColor, Color.white, 0.25f);
	}

	void OnDrawGizmos() {
		if (selected) {
//			Gizmos.DrawWireSphere(transform.position, 0.5f);
		}
	}

	public void Select() {
		selected = true;
		GetComponent<SpriteRenderer>().color = selectedColor;
	}

	public void Unselect() {
		selected = false;
		GetComponent<SpriteRenderer>().color = originalColor;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseOver() {
		
	}

}
