using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DragToUseCard : MonoBehaviour {
	private Vector2 startingPosition;
	private Vector2 startingMousePosition;


	public Deck hand;
	public Stage level;
	public Unit myUnit;
	public CardData myCard;

	private List<Tile> targetableTiles;

	// Use this for initialization
	void Start () {
	
	}

	void OnMouseDown() {
		Camera camera = Camera.main;
		// Record the starting position
		startingPosition = transform.position;
		startingMousePosition = camera.ViewportToWorldPoint(camera.ScreenToViewportPoint(Input.mousePosition));

		// Find valid tiles to drag onto
		targetableTiles = myCard.findTargetableTiles(level.myTiles, myUnit);
		foreach (Tile t in targetableTiles) {
			t.Select();
		}
	}

	void OnMouseDrag() {
		Camera camera = Camera.main;
		Vector2 currentMousePosition = camera.ViewportToWorldPoint(camera.ScreenToViewportPoint(Input.mousePosition));
		// Move card to the correct place
		transform.position = startingPosition + (currentMousePosition - startingMousePosition);
		transform.Translate(0,0,-2);
	}

	Tile targettedTile() {
		Camera camera = Camera.main;
		Vector2 currentMousePosition = camera.ViewportToWorldPoint(camera.ScreenToViewportPoint(Input.mousePosition));
		foreach (Tile t in targetableTiles) {
			if (t.GetComponent<Collider2D>().OverlapPoint(currentMousePosition)) {
				return t;
			}
		}
		return null;
	}

	IEnumerator ReturnToStart(float t) {
		float dt = 0;
		Vector3 endPosition = transform.position;
		do {
			transform.position = Vector3.Lerp(endPosition, startingPosition, dt/t);
			dt += Time.deltaTime;
			yield return null;
		} while (dt < t);
		transform.position = startingPosition;
	}

	void OnMouseUp() {
		Tile target = targettedTile();
		foreach (Tile t in targetableTiles) {
			t.Unselect();
		}

		// did we drag onto a valid tile?
		if (target != null) {
			Debug.Log ("Woot");
			// apply this card to that target
			myCard.execute(target, myUnit);
			hand.MoveIntoPlay(this);
			Destroy(gameObject);
			hand.RepositionAllCards();
		} else {
			Debug.Log("NOOP");
			// Start the coroutine to return this to it's starting location
			StartCoroutine(ReturnToStart(0.1f));
		}
	}
}
