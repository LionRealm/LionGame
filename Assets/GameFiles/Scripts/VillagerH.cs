﻿using UnityEngine;
using System.Collections;
using Pathfinding;
using System.Linq;
using System.Collections.Generic;
	[RequireComponent(typeof(Seeker))]
public class VillagerH : AIPath
{	

	
	public float sleepVelocity = 0.4F;
	public float animationSpeed = 0.2F;
	public GameObject endOfPathEffect;

	//Selection Glow
	public GameObject selectionGlow = null;
	public Transform[] resource;
	public Transform village;
	public float capacity, collectionAmount;
	public bool selected = false;
	public bool isSelectable = false;
	private float currentLoad = 0.0f;
	private bool selectedByClick = false;
	private bool Harvesting = false;
	private GameObject Glow = null;
	private float HarvestTime = 5f;
	private float StartDeposit = 0.0f;
	private int Rand;
	private bool moving = false;
	protected Vector3 lastTarget;

	private bool moveVillager;


	public new void Start () 
	{
		animation.Play ("Walk");
		capacity = 20.0f;
		int i = 0;
		moveVillager = true;
		

			try{
		print (GameObject.Find("Managers").GetComponent<Manager>().UniqueRandomInt(0,resource.Length));
		Rand = GameObject.Find("Managers").GetComponent<Manager>().UniqueRandomInt(0,resource.Length);
		
		StartHarvest ();
			}catch{
			print ("Crash");
			}
		base.Start ();
	}




	protected new void Update () {
		
			Vector3 velocity;
			GameObject.Find ("WoodText").GetComponent<GUIText>().text = "Wood: " + GameObject.Find ("Managers").GetComponent<Manager>().Wood;
			Selection ();
			ClickMove ();
			if (selected && Input.GetKeyDown(KeyCode.A)){
				moving = false;
				Harvesting = false;
				StartHarvest ();
				Debug.Log (target.name);
				
			}
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
				
				
				
			if (!moving) {
				if (Vector3.Distance (transform.position, resource [Rand].transform.position) < 5.0f) {
				
					animation.Play ("Lumbering");
					StartHarvest ();
					Harvesting = true;

				
				}
			
			
				if (Vector3.Distance (transform.position, village.transform.position) < 6.0f && Harvesting == true) {
					Harvesting = false;
					StartHarvest ();


				
				}
			}
		}

	




	private void Selection(){
		try{
		if(renderer.isVisible && Input.GetMouseButton(0) && isSelectable)
		{
			// Drag and select , if worker is in the rectangle
			// Turn selected to true
			if(!selectedByClick){
				Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
				camPos.y = CameraOperator.InvertMouseY(camPos.y);
				selected = CameraOperator.selection.Contains (camPos);
				//selectedByClick = true;
				//selected = false;
				
			}
			
			
			// Particle system for selection glow
			
			if(selected && Glow == null){
				
				Glow = (GameObject)GameObject.Instantiate(selectionGlow,transform.position,Quaternion.identity);
				Glow.transform.parent = transform;
				
				Glow.transform.localPosition = new Vector3(0,-GetComponent<MeshFilter>().mesh.bounds.extents.y,0);
				Debug.Log ("Selected");
				
				
				
			}
			
			else if (!selected && Glow !=null){
				GameObject.Destroy (Glow);
				
				Glow = null;
				Debug.Log ("Selected off");
				renderer.material.color = Color.white;
				
			}
			
			
		}
		}catch{
		}
	}

		private void ClickMove ()
		{
			// Right click movement
			RaycastHit hit;
			var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (selected && Input.GetMouseButton (1) && Physics.Raycast (ray, out hit, 600.0f)) {
				if(hit.collider.name == "TownCentre"){
				moving = true;
				target = hit.transform;
				if(moveVillager){
				GameObject.Find ("Managers").GetComponent<Manager>().playerTurns -= 1;
				moveVillager = false;
				}

				}
				
			}
			
		}

	public override void OnTargetReached () {
		animation.Play ("Idle");
		
		if (endOfPathEffect != null && Vector3.Distance (tr.position, lastTarget) > 1) {
			GameObject.Instantiate (endOfPathEffect, tr.position, tr.rotation);
			lastTarget = tr.position;


		}
		
	}
	public override Vector3 GetFeetPosition ()
	{
		return tr.position;
	}
	
	void OnMouseDown(){
		// Selects Unit and applys change to gui text
		selectedByClick = true;
		selected = true;
		
	}
	void OnMouseUp(){
		if(selectedByClick)
			selected = true;

		selectedByClick = false;
		
	}
	private void StartHarvest() {
		// Starts collection
		Collect();
		
		// If currentload is full walk to bank
			if (!moving) {
				if (Harvesting) {
					if (currentLoad >= capacity) {
						//make sure that we have a whole number to avoid bugs
						//caused by floating point numbers
						currentLoad = Mathf.Floor (currentLoad);
						animation.Play ("Walk");
						target = village;
						target.position = village.transform.position;
						Harvesting = false;
					}
				} else {
			
					// Once emptied walk back to resource and harvest again
					GameObject.Find ("Managers").GetComponent<Manager>().Wood += (int)currentLoad;
					currentLoad = 0;
					animation.Play ("Walk");
					target = resource [Rand];
					target.position = resource [Rand].transform.position;
			
					Harvesting = true;
		
				}
			}
	}
	
	
	
	
	
	private void Collect() {
		
		float collect = collectionAmount * Time.deltaTime;
		//make sure that the harvester cannot collect more than it can carry
		if(currentLoad + collect > capacity) collect = capacity - currentLoad;
		currentLoad += collect;
	
	}
	}
