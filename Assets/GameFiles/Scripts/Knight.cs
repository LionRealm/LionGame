using UnityEngine;
using System.Collections;
using Pathfinding;


[RequireComponent (typeof (Seeker))]
public class Knight: MonoBehaviour
{

	public float speed = 20.0f;
	public bool selected = false;
	public GameObject selectionGlow = null;

	private float curSpeed;
	private Vector3 targetPoint;
	private bool selectedByClick = false;
	private GameObject Glow = null;
	private bool moving = false;


	// Pathfinding Variables
	public Transform target;
	public float updateRate = 2f;
	
	private Seeker seeker;
	
	
	public Path path;
	[HideInInspector]
	public bool pathIsEnded = false;
	
	public float nextWaypointDistance = 3;
	
	private int currentWaypoint = 0;



	// Use this for initialization
	void Start () 
	{

		seeker = GetComponent <Seeker> ();
		if (target == null) {
			Debug.LogError ("No Target");
			return;
		}
		seeker.StartPath (transform.position, target.position, OnPathComplete);

			StartCoroutine (UpdatePath ());

	}

	// Update is called once per frame
	void Update () 
	{
		Selection ();
		ClickMove ();		
		PathFinding ();
	}
	#region PathFinding

	IEnumerator UpdatePath (){

			if (target == null) {
				return false;
			}
		
			seeker.StartPath (transform.position, target.position, OnPathComplete);
		
			yield return new WaitForSeconds (1f / updateRate);
		
			StartCoroutine (UpdatePath ());

		
	}
	
	public void OnPathComplete(Path p){
		Debug.Log ("We got a path. Did it have an error?" + p.error);
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}
	

	private void PathFinding(){
	if (moving) {
			if (target == null) {
				return;
			}
		
		
			if (path == null)
				return;
		
			if (currentWaypoint >= path.vectorPath.Count) {
				moving = false;
				if (pathIsEnded)
					return;
			
				Debug.Log ("End of path reached.");
				
				animation.Play ("idle");
				pathIsEnded = true;
				return;
			}
		
			pathIsEnded = false;
		
			Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
		
			dir *= speed * Time.fixedDeltaTime;




				



//			//Rotate the worker to its target directional vector


			transform.position += dir * 3 * Time.deltaTime;
			float dist = Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]);

			var rot = Quaternion.LookRotation (dir);
			transform.rotation = Quaternion.Slerp (transform.rotation, rot, 6.0f * Time.deltaTime);
			if (dist < nextWaypointDistance) {
				currentWaypoint ++;
				return;
			}
		}

	}



	#endregion
	#region Selection and Moving
	private void Selection(){
		if(renderer.isVisible && Input.GetMouseButton(0))
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
			try{
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
					GameObject.Find("CapacityText").guiText.enabled = false;
					GameObject.Find("HungerText").guiText.enabled = false;
				}
			}catch{
				
			}
			
		}

	}



	private void ClickMove(){
		// Right click movement
		RaycastHit hit;
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if(selected && Input.GetMouseButton (1) && Physics.Raycast(ray, out hit, 100.0f))
		{
			moving = true;

			target.position	= hit.point;

			
		}
		if (moving) {
			animation.Play ("run");
		}
	}




	#endregion

	#region Mouse Input
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
	#endregion

}