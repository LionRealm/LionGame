using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PopulationBuilding : MonoBehaviour {

	//Public:
	public bool isControlledByPlayer;
	public bool isWoodHouse;
	public bool hasBeenClicked;
	public bool IsControlledByPlayer{get{return isControlledByPlayer;}}
	public int CurrentWood{get{return currentWood;}set{currentWood = value;}}
	public int CurrentPopulation{get{return currentPopulation;}set{currentPopulation = value;}}
	public int RequiredWood{get{return requiredWood;}set{requiredWood = value;}}
	public int RequiredPopulation{get{return requiredPopulation;}set{requiredPopulation = value;}}
	public bool inVillage = false;
	public bool hasVisited = false;
	public bool secondVisit = false;
	public bool	thirdVisit = false;

	[SerializeField]private Transform villagerPrefab;
	public List<GameObject> villagers = new List<GameObject>();

	//Private:
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
	private UserInterface userInterface;

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
			UpdateWorldManager();
			if(isControlledByPlayer)
			{
				userInterface.SelectedBuilding = transform.GetComponent<PopulationBuilding>();
			}
			if(!hasBeenClicked)
			{
				if(worldManager.markingVillage)
				{
					worldManager.MarkedVillage = transform;
					worldManager.markingVillage = false;
				}
			}
		}
	}
	private void GatherStartUpComponents()
	{
		worldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
		userInterface = GameObject.Find("UserInterFace").GetComponent<UserInterface>();
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
	public void InstantiateVillagers(int count)
	{
		for(int i = 0; i <= count; i++)
		{
			//villagers.Add(Instantiate(villagerPrefab,new Vector3(transform.position.x + i,transform.position.z + i,transform.position.y)));
		}
	}
}
