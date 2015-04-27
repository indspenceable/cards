using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public int x;
	public int y;
	public Stage stage;
	public Unit unit;
	public int cost = 1;

	private bool selected;

	private Camera camera;
	private Collider2D collider;

	private Color originalColor;
	private Color selectedColor;
	public bool passable;

	// Use this for initialization
	void Start () {
		this.camera = Camera.main;
		this.collider = GetComponent<Collider2D>();
		this.originalColor = GetComponent<SpriteRenderer>().color;
		this.selectedColor = Color.Lerp(originalColor, Color.black, 0.25f);
	}

	void OnDrawGizmos() {
		if (selected) {
//			Gizmos.DrawWireSphere(transform.position, 0.5f);
		}
	}

	public GameObject selectorPrefab;
	private Object selector = null;
	public void Select() {
		selected = true;
//		GetComponent<SpriteRenderer>().color = selectedColor;
		if (selectorPrefab != null) {
			selector = Object.Instantiate(selectorPrefab, transform.position, Quaternion.identity);
			(selector as GameObject).transform.Translate(new Vector3(0,0,-3));
		}
	}

	public bool AdjacentTo(Tile other) {
		return Mathf.Abs(this.x - other.x) + Mathf.Abs(this.y - other.y) == 1;
	}
	
	public void Unselect() {
		selected = false;
//		GetComponent<SpriteRenderer>().color = originalColor;
		if (selector != null) {
			Object.Destroy(selector);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseOver() {
		
	}

}
