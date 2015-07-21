using UnityEngine;
using System.Collections;
using Pathfinding;

public class PathingScript : AIPath
{
		//Public:


		//Private:
		//Vectors for movement/Direction and targetPosition
		private Vector3 targetPosition;
		private Vector3 dir;

		//Components to be accessed from other gameobjects
//		private Seeker seeker;
//		private CharacterController controller;
		//private WorldManager worldManager;

		//variables to use in order to find path and move
		private Path path;
//		private float speed = 250;
		private float nextWayPointDistance = 0.1f;
		private int currentWayPoint = 0;

		public float sleepVelocity = 0.4F;
		
		/** Speed relative to velocity with which to play animations */
		public float animationSpeed = 0.2F;

		private bool moving = false;

		// Use this for initialization
		private void Start ()
		{
				GetStartUpComponents ();
				animation.Play ("Walk");
		}
	
		// Update is called once per frame
		private void Update ()
		{

		Vector3 velocity;

//		if (moving) {
//			animation.Play ("run");
//		}
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

//		private void FixedUpdate ()
//		{
//				if (path == null) {
//						return;
//				}
//				if (currentWayPoint >= path.vectorPath.Count) {
//						return;
//				}
//
//				dir = (path.vectorPath [currentWayPoint] - transform.position).normalized;
//				dir *= speed * Time.deltaTime;
//		RotateTowards (dir);
//				controller.SimpleMove(dir);
//				if (Vector3.Distance (transform.position, path.vectorPath [currentWayPoint]) < nextWayPointDistance) {
//						currentWayPoint++;
//				}
//		}
//
//		private void OnPathComplete (Path p)
//		{
//				if (!p.error) {
//						path = p;
//						currentWayPoint = 0;
//				}
//		moving = false;
//		}

		public void GetNewPath (Vector3 position)
		{
				target.position = position;
//				targetPosition = position;
//				print(targetPosition);
				seeker.StartPath (transform.position, target.position, OnPathComplete);
				moving = true;
		}
		public override Vector3 GetFeetPosition ()
		{
			return tr.position;
		}

		private void GetStartUpComponents ()
		{
				seeker = GetComponent<Seeker> ();
				controller = GetComponent<CharacterController> ();
				//worldManager = GameObject.Find("WorldManager").GetComponent<WorldManager>();
		}
}
