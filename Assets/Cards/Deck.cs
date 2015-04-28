using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public abstract class CardData {
	public abstract string pictureKey();
	public abstract List<Tile> findTargetableTiles(Stage level, Unit user);
	public abstract void execute(Tile t, Unit user);
}

/*
public class MagicTeleport : CardData {
	public override List<Tile> findTargetableTiles(Stage level, Unit user) {
		return level.myTiles.GetRange(0, level.myTiles.Count);
	}
	public override void execute(Tile t, Unit user) {
		user.MoveTo(t);
	}
}
*/

public class Movement : CardData {
	public int distance;
	public Movement(int distance) {
		this.distance = distance;
	}
	public override string pictureKey() {
		return "movement";
	}
	public override List<Tile> findTargetableTiles(Stage level, Unit user) {
		return level.path(user, 3);
	}
	public override void execute(Tile t, Unit user) {
		user.MoveTo(t);
	}
}

public class MeleeAttack : CardData {
	public int strength;
	public MeleeAttack(int strength) {
		this.strength = strength;
	}
	public override string pictureKey() {
		return "melee_attack";
	}
	public override List<Tile> findTargetableTiles(Stage level, Unit user) {
		return level.myTiles.FindAll (t => t.AdjacentTo (user.tile) && t.unit && t.unit != user);
	}
	public override void execute(Tile t, Unit user) {
		t.unit.TakeHit(strength);
	}
}

[System.Serializable]
public struct NamedImage {
	public string name;
	public Sprite image;
}

public class Deck : MonoBehaviour {
	public GameObject cardPrefab;
	public GameObject level;
	public NamedImage[] pictures;


	[HideInInspector]
	public List<CardData> cardsInDeck = new List<CardData>();
	[HideInInspector]
	public List<DragToUseCard> cardsInHand = new List<DragToUseCard>();
	[HideInInspector]
	public List<CardData> cardsInPlay = new List<CardData>();
	[HideInInspector]
	public List<CardData> cardsInDiscard = new List<CardData>();
	[HideInInspector]
	public List<CardData> cardsInExile = new List<CardData>();

	public void Start() {
	}

	public void SetUp() {
//		for(int i = 0; i < 10; i+=1) {
//			cardsInDeck.Add(new Movement(3));
//		}
//		Shuffle();
		cardsInDeck.Add(new Movement(3));
		cardsInDeck.Add(new Movement(3));
		cardsInDeck.Add(new Movement(3));
		cardsInDeck.Add(new MeleeAttack(3));
		cardsInDeck.Add(new MeleeAttack(3));
		DrawHand();
	}
	/*
	public void Consolidate() {
		List<CardData> allCards = new List<CardData>();
		allCards.AddRange(cardsInDeck);
		allCards.AddRange(cardDataInHand);
		allCards.AddRange(cardsInDiscard);
		allCards.AddRange(cardsInExile);

		cardsInDeck = allCards;
		cardDataInHand.Clear();
		cardsInDiscard.Clear();
		cardsInExile.Clear();
	}
	*/

	public void Shuffle() {
		for (int i = cardsInDeck.Count-1; i >= 0; i--) {
			int randomIndex = Random.Range(0, i);
			CardData card = cardsInDeck[randomIndex];
			cardsInDeck.RemoveAt(randomIndex);
			cardsInDeck.Add(card);
		}
	}

	private Sprite findCardSpriteByName(string name) {
		foreach (NamedImage n in pictures) {
			if (n.name.Equals(name)) {
				return n.image;
			}
		}
		return null;
	}

	public void DrawHand() {
		List<CardData> handOfCards = new List<CardData>();

		handOfCards.AddRange(cardsInDeck.GetRange(0, 5));
		cardsInDeck.RemoveRange(0, 5);
		int y = 0;
		foreach(CardData cardData in handOfCards) {
			DragToUseCard cardScript = (Instantiate(cardPrefab, new Vector3(0, y*-1, -2) + transform.position, Quaternion.identity) as GameObject).GetComponent<DragToUseCard>();
			cardScript.myCard = cardData;
			cardScript.myUnit = level.GetComponent<Stage>().player;
			cardScript.level = level.GetComponent<Stage>();
			cardScript.hand = this;
			cardScript.GetComponent<SpriteRenderer>().sprite = findCardSpriteByName(cardData.pictureKey());
			cardsInHand.Add(cardScript);
			y+=1;
		}
	}

	public void MoveIntoPlay(DragToUseCard card) {
		cardsInHand.Remove(card);
		cardsInPlay.Add(card.myCard);
	}

	public void RepositionAllCards() {
		int y = 0;
		foreach(DragToUseCard cardScript in cardsInHand) {
			Vector3 dest = new Vector3(0, y*-1, -2) + transform.position;
			StartCoroutine(RepositionCard(0.1f, cardScript, dest));
			y+=1;
		}
	}

	IEnumerator RepositionCard(float t, DragToUseCard card, Vector3 endPosition) {
		float dt = 0;
		Vector3 startPosition = card.transform.position;
		do {
			card.transform.position = Vector3.Lerp(startPosition, endPosition, dt/t);
			dt += Time.deltaTime;
			yield return null;
		} while (dt < t);
		card.transform.position = endPosition;
	}
}
