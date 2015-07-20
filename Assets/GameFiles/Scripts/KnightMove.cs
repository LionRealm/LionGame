using UnityEngine;
using System.Collections;
using Pathfinding;


		/** AI controller specifically made for the spider robot.
	 * The spider robot (or mine-bot) which is got from the Unity Example Project
	 * can have this script attached to be able to pathfind around with animations working properly.\n
	 * This script should be attached to a parent GameObject however since the original bot has Z+ as up.
	 * This component requires Z+ to be forward and Y+ to be up.\n
	 * 
	 * It overrides the AIPath class, see that class's documentation for more information on most variables.\n
	 * Animation is handled by this component. The Animation component refered to in #anim should have animations named "awake" and "forward".
	 * The forward animation will have it's speed modified by the velocity and scaled by #animationSpeed to adjust it to look good.
	 * The awake animation will only be sampled at the end frame and will not play.\n
	 * When the end of path is reached, if the #endOfPathEffect is not null, it will be instantiated at the current position. However a check will be
	 * done so that it won't spawn effects too close to the previous spawn-point.
	 * \shadowimage{mine-bot.png}
	 * 
	 * \note This script assumes Y is up and that character movement is mostly on the XZ plane.
	 */
		[RequireComponent(typeof(Seeker))]
		public class KnightMove : AIPath
		{
		
				/** Animation component.
		 * Should hold animations "awake" and "forward"
		 */

				/** Minimum velocity for moving */
				public float sleepVelocity = 0.4F;
		
				/** Speed relative to velocity with which to play animations */
				public float animationSpeed = 0.2F;
		
				/** Effect which will be instantiated when end of path is reached.
		 * \see OnTargetReached */
				public GameObject endOfPathEffect;
				public bool selected = false;
				private bool selectedByClick = false;
				public GameObject selectionGlow = null;
				private bool moving = false;
				private GameObject Glow = null;

				public new void Start ()
				{

						animation.Play ("idle");
						moving = true;
						//Call Start in base script (AIPath)
						base.Start ();
				}
		
				/** Point for the last spawn of #endOfPathEffect */
				protected Vector3 lastTarget;
		
				/**
		 * Called when the end of path has been reached.
		 * An effect (#endOfPathEffect) is spawned when this function is called
		 * However, since paths are recalculated quite often, we only spawn the effect
		 * when the current position is some distance away from the previous spawn-point
		*/
				public override void OnTargetReached ()
				{
						animation.Play ("idle");
						moving = false;
						if (endOfPathEffect != null && Vector3.Distance (tr.position, lastTarget) > 1) {
								GameObject.Instantiate (endOfPathEffect, tr.position, tr.rotation);
								lastTarget = tr.position;
						}

				}
		
				public override Vector3 GetFeetPosition ()
				{
						return tr.position;
				}

				private void ClickMove ()
				{
						// Right click movement
						RaycastHit hit;
						var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						if (selected && Input.GetMouseButton (1) && Physics.Raycast (ray, out hit, 600.0f)) {
								moving = true;

								target.position = hit.point;
				
				
						}
						
				}
		
				void OnMouseDown ()
				{
						// Selects Unit and applys change to gui text
						selectedByClick = true;
						selected = true;

				}

				void OnMouseUp ()
				{
						if (selectedByClick)
								selected = true;
			
						selectedByClick = false;


			
				}

				public void GetNewPath (Vector3 position)
				{
					
					target.position = position;
					print(target.position);
					seeker.StartPath (transform.position, target.position, OnPathComplete);
					moving = true;
				}

				private void Selection ()
				{
						if (renderer.isVisible && Input.GetMouseButton (0)) {
								// Drag and select , if worker is in the rectangle
								// Turn selected to true
								if (!selectedByClick) {
										Vector3 camPos = Camera.main.WorldToScreenPoint (transform.position);
										camPos.y = CameraOperator.InvertMouseY (camPos.y);
										selected = CameraOperator.selection.Contains (camPos);
										//selectedByClick = true;
										//selected = false;
					
								}
				
				
								// Particle system for selection glow
								try {
										if (selected && Glow == null) {

												Glow = (GameObject)GameObject.Instantiate (selectionGlow, transform.position, Quaternion.identity);
					
												Glow.transform.parent = transform;
												Glow.transform.localPosition = new Vector3 (0, -GetComponent<MeshFilter> ().mesh.bounds.extents.y, 0);
												Debug.Log ("Selected");

					
						
										} else if (!selected && Glow != null) {

						
												GameObject.Destroy (Glow);
												Glow = null;
												Debug.Log ("Selected off");
												renderer.material.color = Color.white;
												GameObject.Find ("CapacityText").guiText.enabled = false;
												GameObject.Find ("HungerText").guiText.enabled = false;
										}
								} catch {
					
								}
				
						}
			
				}

				protected new void Update ()
				{

						//Get velocity in world-space
						Vector3 velocity;
						//Selection ();
						//ClickMove ();
						if (moving) {
							animation.Play ("run");
						}
//			if (GameObject.Find ("Managers").GetComponent<Manager> ().canMove) {
//				foreach (GameObject obj in GameObject.FindGameObjectsWithTag("TownCentre")) {
//					if (obj.GetComponent<VillageManager> ().secondSelect) {
//						target.position = obj.GetComponent<VillageManager> ().villagePoint.position;
//						GameObject.Find ("Managers").GetComponent<Manager>().playerTurns -= 1;
//						moving = true;
//						obj.GetComponent<VillageManager> ().secondSelect = false;
//					}
//				}
//				
//				
//			}
//			
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
		}



