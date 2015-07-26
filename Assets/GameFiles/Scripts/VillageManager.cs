using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillageManager : MonoBehaviour
{
		//public int villagerCount;
		public bool selected;
		public bool secondSelect;
		public Transform Villager;
		public GameObject House;
		public GameObject selectionGlow = null;
		public GameObject secondSelectionGlow = null;
		public GameObject inVillageGlow = null;
		public bool playerInVillage = false;
		public Transform villagePoint;
		public bool inVillage = false;
		public bool hasVisited = false;
		public bool secondVisit = false;
		public bool	thirdVisit = false;

		public int wood;

		private bool selectedByClick = false;
		private bool searchingTown = false;
		private GameObject Glow = null;
		private GameObject Glow2 = null;
		private GameObject Glow3 = null;
		private Vector3 spawnPosition; 

		
	// Use this for initialization
		void Start ()
		{

		}

		void OnMouseDown ()
		{
				// Selects Unit and applys change to gui text
				if (!selected && !GameObject.Find ("Managers").GetComponent<Manager> ().searchingTown) {
						if (playerInVillage) {
								foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TownCentre")) {
										obj.GetComponent<VillageManager> ().selected = false;
										obj.GetComponent<VillageManager> ().secondSelect = false;
										obj.GetComponent<VillageManager> ().selectedByClick = false;
								}		
								secondSelect = false;
								selected = true;
								selectedByClick = true;
								GameObject.Find ("Managers").GetComponent<Manager> ().searchingTown = true;
						}
				} else if (!selected && GameObject.Find ("Managers").GetComponent<Manager> ().searchingTown) {
						
						foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TownCentre")) {
								obj.GetComponent<VillageManager> ().selected = false;
								obj.GetComponent<VillageManager> ().secondSelect = false;
								obj.GetComponent<VillageManager> ().selectedByClick = false;
						}
						selected = false;
						secondSelect = true;
						GameObject.Find ("Managers").GetComponent<Manager> ().searchingTown = false;
				}
		}

		void OnMouseUp ()
		{
				// Selects Unit and applys change to gui text
				foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TownCentre")) {
						if (obj.GetComponent<VillageManager> ().selectedByClick) {
								obj.GetComponent<VillageManager> ().selected = true;
						}
						
						secondSelect = false;
						obj.GetComponent<VillageManager> ().selectedByClick = false;
				}

		}

		// Update is called once per frame
		void Update ()
		{

				Selection ();
		}

		
		private void Selection ()
		{

		if (playerInVillage && Glow3 == null) {
			
						Glow3 = (GameObject)GameObject.Instantiate (inVillageGlow, transform.position, Quaternion.identity);
			
						Glow3.transform.parent = transform;
						Glow3.transform.localPosition = new Vector3 (0, 7, 0);
			
			
			
				} else if (!playerInVillage && Glow3 != null) {
			
			
						GameObject.Destroy (Glow3);
						Glow3 = null;
						renderer.material.color = Color.white;
				} else if (selected) {
						GameObject.Destroy (Glow3);
						Glow3 = null;
						renderer.material.color = Color.white;
				}
				if (renderer.isVisible && Input.GetMouseButton (0)) {
						// Drag and select , if worker is in the rectangle
						// Turn selected to true
						
			
			
						// Particle system for selection glow
						try {
								
								if (selected && Glow == null) {
					
										Glow = (GameObject)GameObject.Instantiate (selectionGlow, transform.position, Quaternion.identity);
					
										Glow.transform.parent = transform;
										Glow.transform.localPosition = new Vector3 (0, 7, 0);
					
					
					
								} else if (!selected && Glow != null) {
					
					
										GameObject.Destroy (Glow);
										Glow = null;
										renderer.material.color = Color.white;
								}


								if (secondSelect && Glow2 == null) {
									
										Glow2 = (GameObject)GameObject.Instantiate (secondSelectionGlow, transform.position, Quaternion.identity);
									
										Glow2.transform.parent = transform;
										Glow2.transform.localPosition = new Vector3 (0, 7, 0);
									
									
									
								} else if (!secondSelect && Glow2 != null ) {
									
									
										GameObject.Destroy (Glow2);
										Glow2 = null;
										renderer.material.color = Color.white;
								}


						} catch {
				
						}
		}
				
		
		}

		void OnTriggerEnter (Collider col)
		{
				if (col.gameObject.tag == "Player") {
						//Debug.Log ("Visited Villages" + vistedVillages.Count);
						playerInVillage = true;
						
						GameObject.Destroy (Glow2);
						Glow2 = null;
						renderer.material.color = Color.white;
						GameObject.Find ("Managers").GetComponent<Manager> ().canMove = true;
			try{
			House.GetComponent<House>().Villager.GetComponent<VillagerH>().isSelectable = true;
			Debug.Log (House.GetComponent<House>().Villager.GetComponent<VillagerH>().isSelectable);
			}catch{

			}
				}
		
		}
	
		void OnTriggerExit (Collider col)
		{
				if (col.gameObject.tag == "Player") {
						
						//Debug.Log ("Visited Villages" + vistedVillages.Count);
						playerInVillage = false;
						GameObject.Find ("Managers").GetComponent<Manager> ().canMove = false;
			try{
			House.GetComponent<House>().Villager.GetComponent<VillagerH>().isSelectable = false;
			Debug.Log (House.GetComponent<House>().Villager.GetComponent<VillagerH>().isSelectable);
			}catch{

			}
			
				}
		
		}
}
