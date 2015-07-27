using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CanvasScript : MonoBehaviour
{
	//Public:
	public GameObject spawnedVillager{get;set;}

	//Private:

	//Components etc

	//Public:

	//Private:
	private PopulationBuilding parentBuilding;
	private WorldManager worldManager;
	private Canvas canvasComponent;

	private Text currentWoodText;
	private Text currentPopulationText;
	private Text requiredWoodText;
	private Text requiredPopulationText;


	[SerializeField]private GameObject vilagerPrefab;


	// Use this for initialization
	void Start ()
	{
		GatherStartUpComponents();
		FindChildren();
	}
	
	// Update is called once per frame
	void Update () 
	{
		ControlledByPlayer();
		ButtonLookAt();
	}
	private void GatherStartUpComponents()
	{
		worldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
		parentBuilding = transform.parent.gameObject.GetComponent<PopulationBuilding>();
		canvasComponent = gameObject.GetComponent<Canvas>();
	}
	private void ButtonLookAt()
	{
		transform.LookAt(Camera.main.transform.position);
		transform.Rotate(new Vector3(180,0,180));
	}
	private void SpawnVillager()
	{
		spawnedVillager = (GameObject)Instantiate(vilagerPrefab,new Vector3(parentBuilding.transform.position.x+2,
		                                                                    parentBuilding.transform.position.y,
		                                                                    parentBuilding.transform.position.z+2),Quaternion.identity);
		worldManager.tempVillager = spawnedVillager;
	}
	private void ControlledByPlayer()
	{
		if(parentBuilding.IsControlledByPlayer)
		{
			UpdateChildren();
			canvasComponent.enabled = true;
		}
		else
		{
			canvasComponent.enabled = false;
		}
	}
	private void FindChildren()
	{
		foreach(Transform child in transform)
		{
			if(child.name == "CurrentWood")
			{
				currentWoodText = child.GetComponent<Text>();
			}
			if(child.name == "CurrentPopulation")
			{
				currentPopulationText = child.GetComponent<Text>();
			}
			if(child.name == "RequiredWood")
			{
				requiredWoodText = child.GetComponent<Text>();
			}
			if(child.name == "RequiredPopulation")
			{
				requiredPopulationText = child.GetComponent<Text>();
			}
		}
	}
	private void UpdateChildren()
	{
		currentWoodText.text = parentBuilding.CurrentWood.ToString();
		currentPopulationText.text = parentBuilding.CurrentPopulation.ToString();
		requiredWoodText.text = parentBuilding.RequiredWood.ToString();
		requiredPopulationText.text = parentBuilding.RequiredPopulation.ToString();
	}
	public void ButtonClicked()
	{
		print("buttonm");
		if(worldManager.buildingState == WorldManager.BuildingState.SecondWait)
		{
			if(worldManager.PlayerActions > 0)
			{
				if(parentBuilding.CurrentPopulation > 0)
				{
					SpawnVillager();
					parentBuilding.CurrentPopulation--;
					worldManager.buildingState = WorldManager.BuildingState.FirstClick;
					parentBuilding.HasBeenClicked = true;
					worldManager.LookingForVillagerTarget = true;
					if(parentBuilding.CurrentPopulation <= 2)
					{
						parentBuilding.CurrentWood -=50 ;

					}
				}
			}
		}
		else if(worldManager.buildingState == WorldManager.BuildingState.FirstWait)
		{
			if(!parentBuilding.HasBeenClicked)
			{
				worldManager.buildingState = WorldManager.BuildingState.SecondClick;
			}
		}
		parentBuilding.UpdateInteractions();

	}
}
