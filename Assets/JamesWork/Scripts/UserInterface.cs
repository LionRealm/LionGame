using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

	//Public:

	//Private:
	public int tempInt;
	//Components:

	//Public:

	//Private
	public PopulationBuilding SelectedBuilding{get;set;}
	private WorldManager worldManager;


	// Use this for initialization
	void Start () 
	{
		GatherStartUpComponents();
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdatePlayerData();
	}
	public void PlusButton()
	{
		if(SelectedBuilding)
		{
			if(tempInt < worldManager.CurrentPopulation)
			{
				tempInt += 1;
			}
			GameObject.Find("AmountOfVillagers").GetComponent<Text>().text = tempInt.ToString() + "/" + " " + worldManager.CurrentPopulation;
		}
	}
	public void MinusButton()
	{
		if(SelectedBuilding)
		{
			if(tempInt > 0)
			{
				tempInt -= 1;
			}
			GameObject.Find("AmountOfVillagers").GetComponent<Text>().text = tempInt.ToString() + "/" + " " + worldManager.CurrentPopulation;
		}
	}
	public void MarkLocation()
	{
		if(SelectedBuilding)
		{
			worldManager.markingVillage = true;
			print("mark button");
		}
	}
	public void SendPlayer()
	{
		if(SelectedBuilding)
		{
			worldManager.SendPlayer = true;
		}
	}
	public void SendVillagers()
	{
		if(SelectedBuilding)
		{
			worldManager.SendVillager = true;
			worldManager.markingVillage = false;
		}
	}
	public void HarvestWood()
	{
		if(SelectedBuilding)
		{

		}
	}
	private void UpdatePlayerData()
	{
		GameObject.Find("CurrentWood").GetComponent<Text>().text = worldManager.CurrentWood + " / " + worldManager.RequiredWood;
		GameObject.Find("CurrentPopulation").GetComponent<Text>().text = worldManager.CurrentPopulation + " / " + worldManager.RequiredPopulation;
		//GameObject.Find("CurrentTurn").GetComponent<Text>();
		//GameObject.Find("BoatProgress").GetComponent<Image>();

		if(worldManager.turnState == WorldManager.TurnState.PlayerTurn)
		{
			GameObject.Find("CurrentTurn").GetComponent<Text>().text = "Players Turn";
		}
		else
		{
			GameObject.Find("CurrentTurn").GetComponent<Text>().text = "Enemys Turn";

		}
	}
	private void GatherStartUpComponents()
	{
		worldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
	}
}
