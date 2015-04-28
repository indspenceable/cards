using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

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

	public List<Tile> path(Unit u, int distance) {
		Dictionary<Tile, int> knownDistances = new Dictionary<Tile, int>();
	    List<Tile> openList = new List<Tile>();
		knownDistances.Add(u.tile, 0);
		openList.Add(u.tile);

		while (openList.Count > 0) {
			Tile currentTile = openList[0];
			openList.RemoveAt(0);
			int currentTileDistance = knownDistances[currentTile];

			Tile[] tiles = {
				findTile(currentTile.x-1, currentTile.y),
				findTile(currentTile.x+1, currentTile.y),
				findTile(currentTile.x, currentTile.y-1),
				findTile(currentTile.x, currentTile.y+1),
			};

			foreach (Tile adjacent in tiles) {
				if (canPassTile(u, adjacent) && currentTileDistance + adjacent.cost <= distance) {
					if (! knownDistances.ContainsKey(adjacent)) {
						knownDistances.Add (adjacent, currentTileDistance+1);
						openList.Add (adjacent);
					}
				}
			}
		}

		return knownDistances.Keys.ToList();
	}

	bool canPassTile(Unit u, Tile t) {
		return (t != null &&
		        t.passable &&
		        (t.unit == null || t.unit == u));
	}

	void PlacePlayer() {
		int x = Random.Range(1, WIDTH-1);
		int y = Random.Range(1, HEIGHT-1);
		Tile t = findTile (x, y);

		player = ((GameObject)Instantiate(playerPrefab, t.transform.position, Quaternion.identity)).GetComponent<Unit>();
		player.hp = 1;
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
				rat.hp = 1;
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
			Tile tile = ((GameObject)Instantiate(tilePrefab, new Vector3(x-4.5f, y-4.5f), Quaternion.identity)).GetComponent<Tile>();
			tile.x = x;
			tile.y = y;
			tile.passable = false;
			tile.stage = this;
			
			return tile;
		} else {
			tilePrefab = floorPrefab;
			Tile tile = ((GameObject)Instantiate(tilePrefab, new Vector3(x-4.5f, y-4.5f), Quaternion.identity)).GetComponent<Tile>();
			tile.x = x;
			tile.y = y;
			tile.passable = true;
			tile.stage = this;
			
			return tile;
		}
	}
}