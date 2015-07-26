using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
[RequireComponent(typeof(Seeker))]
public class Soldier : AIPath
{



	public float sleepVelocity = 0.4F;
	public float animationSpeed = 0.2F;
	public GameObject endOfPathEffect;

	public string nameOfTag;
	protected Vector3 lastTarget;


	List<GameObject> vistedVillages = new List<GameObject>();
	List<GameObject> villages = new List<GameObject>();

	// Use this for initialization
	void Start ()
	{
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag(nameOfTag)) {

			villages.Add (obj);

		}

		FindSecondVillage ().GetComponent<PopulationBuilding> ().secondVisit = true;
		FindThirdVillage ().GetComponent<PopulationBuilding> ().thirdVisit = true;
		animation.Play ("Walk");

		base.Start ();
	}

	public override void OnTargetReached ()
	{


		if (endOfPathEffect != null && Vector3.Distance (tr.position, lastTarget) > 1) {
			GameObject.Instantiate (endOfPathEffect, tr.position, tr.rotation);
			lastTarget = tr.position;
		}
		
	}

	public override Vector3 GetFeetPosition ()
	{
		return tr.position;
	}
	
	public GameObject FindClosestVillage ()
	{
		// Create a array to save all the food stores spawned
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag (nameOfTag);
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		// Find closest food store by minusing workers position by every 
		// Foodstore that is currently spawned
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (go.GetComponent <PopulationBuilding> ().inVillage == false && go.GetComponent <PopulationBuilding> ().hasVisited == false) {
				if (curDistance < distance) {
					closest = go;
					distance = curDistance;
				}
			}
		}
		return closest;

	}

	public GameObject FindSecondVillage ()
	{
		
		// Create a array to save all the food stores spawned
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag (nameOfTag);
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = FindClosestVillage ().transform.position;
		// Find closest food store by minusing workers position by every 
		// Foodstore that is currently spawned
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (go.GetComponent <PopulationBuilding> ().inVillage == false && go.GetComponent <PopulationBuilding> ().hasVisited == false) {
				if (curDistance < distance) {
					closest = go;
					distance = curDistance;
				}
			}
		}
		return closest;
		
	}

	public GameObject FindThirdVillage ()
	{
		
		// Create a array to save all the food stores spawned
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag (nameOfTag);
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = FindSecondVillage ().transform.position;
		// Find closest food store by minusing workers position by every 
		// Foodstore that is currently spawned
		foreach (GameObject go in gos) {
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (go.GetComponent <PopulationBuilding> ().inVillage == false && go.GetComponent <PopulationBuilding> ().hasVisited == false && go.GetComponent <PopulationBuilding> ().secondVisit == false) {
				if (curDistance < distance) {
					closest = go;
					distance = curDistance;
				}
			}
		}
		return closest;
		
	}



	// Update is called once per frame
	void Update ()
	{


		// target point is now the closest village
		target.position = FindClosestVillage ().transform.position;

		Vector3 velocity;


		Debug.DrawLine (FindSecondVillage ().transform.position, FindThirdVillage ().transform.position);
		Debug.DrawLine (transform.position, FindSecondVillage ().transform.position);

		if (canMove) {
			
			//Calculate desired velocity
			Vector3 dir = CalculateVelocity (GetFeetPosition ());
			
			//Rotate towards targetDirection (filled in by CalculateVelocity)
			RotateTowards (targetDirection);
			
			dir.y = 0;
			if (dir.sqrMagnitude > sleepVelocity * sleepVelocity) {
				//If the velocity is large enough, move
			} else {
				//Otherwise, just stand still (this ensures gravity is applied)
				dir = Vector3.zero;
			}
			
			if (navController != null) {
				velocity = Vector3.zero;
			} else if (controller != null) {
				controller.SimpleMove (dir);
				velocity = controller.velocity;
			} else {
				Debug.LogWarning ("No NavmeshController or CharacterController attached to GameObject");
				velocity = Vector3.zero;
			}
		} else {
			velocity = Vector3.zero;
		}

	}
	
	
	

	


	void OnTriggerEnter (Collider col)
	{
		if (col.gameObject.name == nameOfTag && col.gameObject.GetComponent<PopulationBuilding> ().inVillage == false) {
			//Debug.Log ("Visited Villages" + vistedVillages.Count);
			Debug.Log ("Village Count" + villages.Count);


			// Once the village is filled turrn the village into an AI village
			if (FindClosestVillage ().GetComponent<PopulationBuilding> ().inVillage == false) {
				foreach (GameObject obj in GameObject.FindGameObjectsWithTag(nameOfTag)) {
					obj.transform.GetComponent<PopulationBuilding> ().inVillage = false;
					obj.transform.GetComponent<PopulationBuilding> ().secondVisit = false;
					obj.transform.GetComponent<PopulationBuilding> ().thirdVisit = false;
				}
				col.transform.GetComponent<PopulationBuilding> ().inVillage = true;
				FindSecondVillage ().GetComponent<PopulationBuilding> ().secondVisit = true;
				FindThirdVillage ().GetComponent<PopulationBuilding> ().thirdVisit = true;
				GameObject.Find ("AIManager").GetComponent<AILogic> ().currentPhase = AILogic.Phase.Checking;
				GameObject.Find ("AIManager").GetComponent<AILogic> ().spawned = false;

				if(col.transform.GetComponent<PopulationBuilding> ().CurrentWood != 200){
					Debug.Log ("Suspicious");
				}
				if(col.transform.GetComponent<PopulationBuilding> ().CurrentPopulation != 2){
					Debug.Log ("Suspicious");
				}
			}


			Destroy (gameObject);
		}

		
	}
	
	
	void OnTriggerExit (Collider col)
	{
		if (col.gameObject.name == nameOfTag) {

			//Debug.Log ("Visited Villages" + vistedVillages.Count);
			col.transform.GetComponent<PopulationBuilding> ().hasVisited = true;
			col.transform.GetComponent<PopulationBuilding> ().inVillage = false;
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag(nameOfTag)) {
				if (obj.transform.GetComponent<PopulationBuilding> ().inVillage) {
					obj.GetComponent<PopulationBuilding> ().hasVisited = true;
				}

			}
			CheckVistedVillages();

		}
		
	}


	void CheckVistedVillages(){
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag(nameOfTag)) {
			if (obj.transform.GetComponent<PopulationBuilding> ().hasVisited) {
				Debug.Log ("Added Visited Village");
				vistedVillages.Add (obj);
				Debug.Log ("visitedVillage count " + vistedVillages.Count);
			}
		}

		if (vistedVillages.Count ==8) {
			Debug.Log("Finished First Round of Checks");
			foreach (GameObject obj in GameObject.FindGameObjectsWithTag(nameOfTag)) {
				obj.transform.GetComponent<PopulationBuilding> ().hasVisited = false;
			}
		}








	}
}
