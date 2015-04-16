using UnityEngine;
using System.Collections;

public class DragToUseCard : MonoBehaviour {
	private Vector2 startingPosition;
	private Vector2 startingMousePosition;


	// Use this for initialization
	void Start () {
	
	}

	void OnMouseDown() {
		// Record the starting position
		// Find valid tiles to drag onto

		startingPosition = transform.position;
		startingMousePosition = Camera.main.ViewportToWorldPoint(Camera.main.ScreenToViewportPoint(Input.mousePosition));
	}

	void OnMouseDrag() {
		Camera camera = Camera.main;
		// Move card to the correct place

		Vector2 currentMousePosition = camera.ViewportToWorldPoint(camera.ScreenToViewportPoint(Input.mousePosition));
		transform.position = startingPosition + (currentMousePosition - startingMousePosition);
	}

	bool validTarget() { return false; }

	void OnMouseUp() {
		// did we drag onto a valid tile?
		if (validTarget()) {
			// apply this card to that target
		} else {
			// Start the coroutine to return this to it's starting location
		}
	}
}
