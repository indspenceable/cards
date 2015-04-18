using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class Stage : MonoBehaviour {
	// Prefabs
	public GameObject wallPrefab;
	public GameObject floorPrefab;
	public GameObject playerPrefab;
	public GameObject ratPrefab;

	public GameObject hand;
	public GameObject Preview;

	public Unit player;
	public List<Tile> myTiles = new List<Tile>();

	public const int WIDTH = 10;
	public const int HEIGHT = 10;

	// Use this for initialization
	void Awake () {
		this.ConstructRandomTileMap();
		this.PlacePlayer();
		this.PlaceSomeRats();
		hand.GetComponent<Deck>().SetUp();
	}

	void PlacePlayer() {
		int x = Random.Range(1, WIDTH-1);
		int y = Random.Range(1, HEIGHT-1);
		Tile t = findTile (x, y);

		player = ((GameObject)Instantiate(playerPrefab, t.transform.position, Quaternion.identity)).GetComponent<Unit>();
		player.MoveTo(t);
	}

	void PlaceSomeRats() {
		for (int i = 0; i < 3; i++) {
			PlaceRat();
		}
	}

	void PlaceRat() {
		while (true) {
			int x = Random.Range(1, WIDTH-1);
			int y = Random.Range(1, HEIGHT-1);
			Tile t = findTile (x, y);
			if (t.unit == null) {
				Unit rat = ((GameObject)Instantiate(ratPrefab, t.transform.position, Quaternion.identity)).GetComponent<Unit>();
				rat.MoveTo(t);
				return;
			}
		}
	}


	void ConstructRandomTileMap() {
		/* TODO build tiles ETC */
		for (int x = 0; x < WIDTH; x++) {
			for (int y = 0; y < HEIGHT; y++) {
				myTiles.Add(buildTile(x, y));
			}
		}
	}

	Tile findTile(int x, int y) {
		for (int i = 0; i < myTiles.Count; ++i) {
			if (myTiles[i].x == x && myTiles[i].y == y) {
				return myTiles[i];
			}
		}
		Debug.Log("Trying to get a tile not in the level: " + x + ", " + y);
		return null;
	}

	Tile buildTile(int x, int y) {
		GameObject tilePrefab;
		if (x == 0 || y == 0 || x == 9 || y == 9 || Random.Range(0,10) < 1) {
			tilePrefab = wallPrefab;
		} else {
			tilePrefab = floorPrefab;
		}

		Tile tile = ((GameObject)Instantiate(tilePrefab, new Vector3(x-4.5f, y-4.5f), Quaternion.identity)).GetComponent<Tile>();
		tile.x = x;
		tile.y = y;
		tile.stage = this;

		return tile;
	}
}