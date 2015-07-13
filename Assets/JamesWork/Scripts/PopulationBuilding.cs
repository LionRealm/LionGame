using UnityEngine;
using System.Collections;

public class PopulationBuilding : MonoBehaviour {

	//Public:
	public bool HasBeenClicked{get{return hasBeenClicked;}set{hasBeenClicked = value;}}
	public bool isControlledByPlayer;
	public bool isWoodHouse;
	public bool IsControlledByPlayer{get{return isControlledByPlayer;}}
	public int CurrentWood{get{return currentWood;}set{currentWood = value;}}
	public int CurrentPopulation{get{return currentPopulation;}set{currentPopulation = value;}}
	public int RequiredWood{get{return requiredWood;}set{requiredWood = value;}}
	public int RequiredPopulation{get{return requiredPopulation;}set{requiredPopulation = value;}}

	private int productionInt = 0;
	//Private:
	private bool hasBeenClicked;

	private bool isPlayersTurn;
	
	private bool stayed;

	private int currentWood;
	private int requiredWood;

	private int currentPopulation;
	private int requiredPopulation;
	//Components etc

	//Public:

	//Private:
	private WorldManager worldManager;

	// Use this for initialization
	void Start () 
	{
		RequiredWood = 200;
		currentWood = 200;
		RequiredPopulation = 2;
		currentPopulation = 2;
		GatherStartUpComponents();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(worldManager.buildingState == WorldManager.BuildingState.SecondWait)
		{
			hasBeenClicked = false;
		}
	}
	private void OnTriggerEnter(Collider collider)
	{
		if(collider.name == "Player")
		{
			isControlledByPlayer = true;
		}
		if(collider.gameObject.tag == "Villager")
		{
			stayed = true;
			StartCoroutine(WaitThenDelete(collider.gameObject));
		}
	}
	private void OnTriggerExit(Collider collider)
	{
		if(collider.name == "Player")
		{
			isControlledByPlayer = false;
		}
		if(collider.gameObject.tag == "Villager")
		{
			stayed = false;
		}
	}
	private void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(0))
		{

			if(worldManager.buildingState == WorldManager.BuildingState.SecondWait)
			{
				worldManager.buildingState = WorldManager.BuildingState.FirstClick;
				hasBeenClicked = true;

			}
			else if(worldManager.buildingState == WorldManager.BuildingState.FirstWait)
			{
				if(!hasBeenClicked)
				{
					worldManager.buildingState = WorldManager.BuildingState.SecondClick;
				}
			}
			UpdateInteractions();
		}
		else if(Input.GetMouseButtonDown(1))
		{
			worldManager.buildingState = WorldManager.BuildingState.SecondWait;
		}
	}
	private void GatherStartUpComponents()
	{
		worldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
	}
	public void UpdateInteractions()
	{
		switch(worldManager.buildingState)
		{
		case WorldManager.BuildingState.FirstClick:
			UpdateWorldManager();
			worldManager.buildingState = WorldManager.BuildingState.FirstWait;
		break;
		case WorldManager.BuildingState.SecondClick:
			if(isWoodHouse)
			{
				if(productionInt < 2)
				{
					productionInt++;
				}
				if(productionInt == 2)
				{
					currentWood++;
					productionInt = 0;
				}
			}
			if(worldManager.LookingForVillagerTarget)
			{
				worldManager.VillagerTargetLocation = transform;
				worldManager.SendVillager = true;
				worldManager.LookingForVillagerTarget = false;
			}
			else
			{
				worldManager.TargetLocation = transform;
				worldManager.SendPlayer = true;
			}
				
				worldManager.buildingState = WorldManager.BuildingState.SecondWait;
		break;
		}
	}
	private IEnumerator WaitThenDelete(GameObject tempObject)
	{
		yield return new WaitForSeconds(3f);
		if(stayed)
		{
			currentWood += 50;
			currentPopulation++;
			Destroy(tempObject);
		}
	}
	private void UpdateWorldManager()
	{
		worldManager.CurrentWood = currentWood;
		worldManager.CurrentPopulation = currentPopulation;
		worldManager.RequiredWood = requiredWood;
		worldManager.RequiredPopulation = requiredPopulation;
	}

}
