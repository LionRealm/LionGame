using UnityEngine;
using System.Collections;
<<<<<<< HEAD
using System.Collections.Generic;
=======

>>>>>>> 3244cd1f2d83c74cabe1a3bba9914f7d5be3aa88
public class WorldManager : MonoBehaviour {


	//Public:
<<<<<<< HEAD
=======
	public enum BuildingState
	{
		FirstClick,
		FirstWait,
		SecondClick,
		SecondWait,
	}
	public BuildingState buildingState;

>>>>>>> 3244cd1f2d83c74cabe1a3bba9914f7d5be3aa88
	public bool SendPlayer{set{sendPlayer = value;}}

	public bool SendVillager{get{return sendVillager;}set{sendVillager = value;}}

	public bool PlayerSent{get;set;}

<<<<<<< HEAD
	public bool markingVillage;
=======
	public bool LookingForVillagerTarget{get{return lookingForVilagerTarget;}set{lookingForVilagerTarget = value;}}

>>>>>>> 3244cd1f2d83c74cabe1a3bba9914f7d5be3aa88
	public int PlayerActions{get{return playerActions;}set{playerActions = value;}}

	public int RequiredWood{get{return clickedBuildingRequiredWood;}set{clickedBuildingRequiredWood = value;}}
	public int CurrentWood{get{return clickedBuildingCurrentWood;}set{clickedBuildingCurrentWood = value;}}
	public int RequiredPopulation{get{return clickedBuildingRequiredPopulation;}set{clickedBuildingRequiredPopulation = value;}}
	public int CurrentPopulation{get{return clickedBuildingCurrentPopulation;}set{clickedBuildingCurrentPopulation = value;}}


	public GameObject tempVillager{get;set;}

	public Transform TargetLocation{set{targetLocation = value;}}

	public Transform VillagerTargetLocation{get{return villagerTargetLocation;} set{villagerTargetLocation = value;}}

<<<<<<< HEAD
	public Transform MarkedVillage{set{targetLocation = value;villagerTargetLocation = value;}}
	//Private:
	private UserInterface userInterface;
	public enum TurnState
=======
	//Private:
	private enum TurnState
>>>>>>> 3244cd1f2d83c74cabe1a3bba9914f7d5be3aa88
	{
		PlayerTurn,
		EnemyTurn,
	}
<<<<<<< HEAD
	public TurnState turnState;
=======
	[SerializeField]private TurnState turnState;
>>>>>>> 3244cd1f2d83c74cabe1a3bba9914f7d5be3aa88
	
	private KnightMove player;

	private Transform targetLocation;

	private Transform villagerTargetLocation;

	private int playerActions;

	private bool sendPlayer;

	private bool sendVillager;

	private bool allPlayersStationary;

	private bool lookingForVilagerTarget;

	public int clickedBuildingCurrentWood;
	public int clickedBuildingRequiredWood;

	public int clickedBuildingCurrentPopulation;
	public int clickedBuildingRequiredPopulation;

	
	//Components etc.

	//Public: 

	//Private:

	// Use this for initialization
	void Start () 
	{
		GatherStartUpComponents();
<<<<<<< HEAD
=======
		buildingState = BuildingState.SecondWait;
>>>>>>> 3244cd1f2d83c74cabe1a3bba9914f7d5be3aa88
		playerActions = 3;
	}
	
	// Update is called once per frame
	void Update () 
	{
		TurnChecking();
	}
	private void GatherStartUpComponents()
	{
		player = GameObject.Find("Player").GetComponent<KnightMove>();
<<<<<<< HEAD
		userInterface = GameObject.Find("UserInterFace").GetComponent<UserInterface>();
=======
>>>>>>> 3244cd1f2d83c74cabe1a3bba9914f7d5be3aa88
	}
	private void TurnChecking()
	{
		switch(turnState)
		{
		case TurnState.PlayerTurn:
			PlayerTurn();
			break;
		case TurnState.EnemyTurn:
			EnemyTurn();
			break;
		}
	}
	private void MovePlayer()
	{
		if(sendPlayer)
		{
			player.GetNewPath(targetLocation.position);
			playerActions--;
			sendPlayer = false;
		}
	}
	private void MoveVillager()
	{
		if(sendVillager)
		{
			playerActions--;
<<<<<<< HEAD
			userInterface.SelectedBuilding.GetComponent<PopulationBuilding>().InstantiateVillagers(userInterface.tempInt);
			foreach(GameObject currentVillager in userInterface.SelectedBuilding.GetComponent<PopulationBuilding>().villagers)
			{

			}
=======
			tempVillager.GetComponent<PathingScript>().GetNewPath(villagerTargetLocation.position);
>>>>>>> 3244cd1f2d83c74cabe1a3bba9914f7d5be3aa88
			sendVillager = false;
		}
	}
	private void PlayerTurn()
	{
		MovePlayer();
		MoveVillager();
		if(playerActions <= 0)
		{
			turnState = TurnState.EnemyTurn;
		}
	}
	private void EnemyTurn()
	{
		GameObject.Find ("AIManager").GetComponent<AILogic> ().AITurn = true;
		turnState = TurnState.PlayerTurn;
		playerActions = 3;
		if(Input.GetKeyDown(KeyCode.A))
		{

			turnState = TurnState.PlayerTurn;
			playerActions = 3;
		}
	}

<<<<<<< HEAD

=======
>>>>>>> 3244cd1f2d83c74cabe1a3bba9914f7d5be3aa88
}
