using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WorldManager : MonoBehaviour {


	//Public:
	public bool SendPlayer{set{sendPlayer = value;}}

	public bool SendVillager{get{return sendVillager;}set{sendVillager = value;}}

	public bool PlayerSent{get;set;}

	public bool markingVillage;
	public int PlayerActions{get{return playerActions;}set{playerActions = value;}}

	public int RequiredWood{get{return clickedBuildingRequiredWood;}set{clickedBuildingRequiredWood = value;}}
	public int CurrentWood{get{return clickedBuildingCurrentWood;}set{clickedBuildingCurrentWood = value;}}
	public int RequiredPopulation{get{return clickedBuildingRequiredPopulation;}set{clickedBuildingRequiredPopulation = value;}}
	public int CurrentPopulation{get{return clickedBuildingCurrentPopulation;}set{clickedBuildingCurrentPopulation = value;}}


	public GameObject tempVillager{get;set;}

	public Transform TargetLocation{set{targetLocation = value;}}

	public Transform VillagerTargetLocation{get{return villagerTargetLocation;} set{villagerTargetLocation = value;}}

	public Transform MarkedVillage{set{targetLocation = value;villagerTargetLocation = value;}}
	//Private:
	private UserInterface userInterface;
	public enum TurnState
	{
		PlayerTurn,
		EnemyTurn,
	}
	public TurnState turnState;
	
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
		userInterface = GameObject.Find("UserInterFace").GetComponent<UserInterface>();
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
			userInterface.SelectedBuilding.GetComponent<PopulationBuilding>().InstantiateVillagers(userInterface.tempInt);
			foreach(GameObject currentVillager in userInterface.SelectedBuilding.GetComponent<PopulationBuilding>().villagers)
			{

			}
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


}
