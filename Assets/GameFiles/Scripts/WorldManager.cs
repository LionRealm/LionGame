using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {


	//Public:
	public enum BuildingState
	{
		FirstClick,
		FirstWait,
		SecondClick,
		SecondWait,
	}
	public BuildingState buildingState;

	public bool SendPlayer{set{sendPlayer = value;}}

	public bool SendVillager{get{return sendVillager;}set{sendVillager = value;}}

	public bool PlayerSent{get;set;}

	public bool LookingForVillagerTarget{get{return lookingForVilagerTarget;}set{lookingForVilagerTarget = value;}}

	public int PlayerActions{get{return playerActions;}set{playerActions = value;}}

	public int RequiredWood{get{return clickedBuildingRequiredWood;}set{clickedBuildingRequiredWood = value;}}
	public int CurrentWood{get{return clickedBuildingCurrentWood;}set{clickedBuildingCurrentWood = value;}}
	public int RequiredPopulation{get{return clickedBuildingRequiredPopulation;}set{clickedBuildingRequiredPopulation = value;}}
	public int CurrentPopulation{get{return clickedBuildingCurrentPopulation;}set{clickedBuildingCurrentPopulation = value;}}


	public GameObject tempVillager{get;set;}

	public Transform TargetLocation{set{targetLocation = value;}}

	public Transform VillagerTargetLocation{get{return villagerTargetLocation;} set{villagerTargetLocation = value;}}

	//Private:
	private enum TurnState
	{
		PlayerTurn,
		EnemyTurn,
	}
	[SerializeField]private TurnState turnState;
	
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
		buildingState = BuildingState.SecondWait;
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
			tempVillager.GetComponent<PathingScript>().GetNewPath(villagerTargetLocation.position);
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
		if(Input.GetKeyDown(KeyCode.A))
		{

			turnState = TurnState.PlayerTurn;
			playerActions = 3;
		}
	}

}
