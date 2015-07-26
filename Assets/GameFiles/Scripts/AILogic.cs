using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class AILogic : MonoBehaviour
{


	public bool	AITurn = false;
	public int Turns = 4;
	public GameObject Villager;
	public Phase currentPhase;
	public bool spawned = false;

	bool Starting;

	public enum Phase
	{
		Start,
		SpawnUnits,
		Checking,
		Walking
		
	};
	
	// Use this for initialization
	void Start ()
	{
		Starting = true;
		AITurn = false;
	}


	
	// Update is called once per frame
	void Update ()
	{
		// Start AI turn


		// If no more turns stop logic

			// Phases of the AI

						switch (currentPhase) {
						//Spawn Units
						case Phase.SpawnUnits:
			if (AITurn) {
								Debug.Log ("Spawn");
								SpawnVillagers ();
								currentPhase = Phase.Walking;
								AITurn = false;
			}
								break;

						case Phase.Checking:
								StartCoroutine (CheckingVillage ());				
								break;

						case Phase.Walking:

								break;
		

						}
				


	
		// Start AI's turn
		if (AITurn) {
				currentPhase = Phase.SpawnUnits;
		

		}

	}


	// Choose the village for where the villagers will spawn
	GameObject ChooseVillage ()
	{

		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag ("TownCentre"); 
				
		GameObject chosenVillage = null;
				

		foreach (GameObject Village in gos) {
			// If the village is an AI village
			if (Village.GetComponent <PopulationBuilding> ().inVillage) {
				chosenVillage = Village;
			}
					
		}
			

		return chosenVillage;
	}

	GameObject ChooseStartingVillage ()
	{
		
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag ("AIVillage"); 
		
		GameObject chosenVillage = null;
		
		
		foreach (GameObject Village in gos) {
			// If the village is an AI village
			if (Village.GetComponent <VillageManager> ().inVillage) {
				chosenVillage = Village;
			}
			
		}
		return chosenVillage;
	}

	void SpawnVillagers ()
	{																																																									
		if (Starting) {
			GameObject soldier = (GameObject)Instantiate (Villager, ChooseStartingVillage ().transform.position, Quaternion.identity);
			Starting = false;
		} else {
			try{
			Instantiate (Villager, ChooseVillage ().transform.position, Quaternion.identity);
			}catch{
				}

						
		}
		currentPhase = Phase.Walking;

	}



	IEnumerator CheckingVillage ()
	{

		yield return new WaitForSeconds (4f);
		if (spawned == false) {
			if (currentPhase != Phase.SpawnUnits) {
				currentPhase = Phase.SpawnUnits;
		
			}
			spawned = true;
		}



	}


}
