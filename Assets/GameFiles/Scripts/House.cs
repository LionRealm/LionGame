using UnityEngine;
using System.Collections;

public class House : MonoBehaviour {

	public GameObject Villager;

	GameObject villagerObj;
	// Use this for initialization
	void Start () {
		SpawnGatherer();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void SpawnGatherer(){
			villagerObj = (GameObject)Instantiate (Villager, transform.position, Quaternion.identity);
	}



}
