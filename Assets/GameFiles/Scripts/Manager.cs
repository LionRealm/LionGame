using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Manager : MonoBehaviour {
	public bool searchingTown = false;
	public bool canMove = false;
	public Vector3 targetPosition;
	public int Wood;
	public int playerTurns = 3;
	// Use this for initialization
	void Start () {
		Wood = 0;
		playerTurns = 3;
	}

	void Update(){
		GameObject.Find ("TurnText").GetComponent<GUIText>().text = "Turns: " + playerTurns;
		if (playerTurns <= 0) {
			GameObject.Find ("AIManager").GetComponent<AILogic> ().AITurn = true;
			playerTurns = 3;
		} else{

		}

	}

	List<int> usedValues = new List<int>();
	public int UniqueRandomInt(int min, int max)
	{
		int val = Random.Range(min, max);
		do {
			val = Random.Range (min, max);
		} while(usedValues.Contains(val));
		return val;
	}
	




}
