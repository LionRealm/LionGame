using UnityEngine;
using System.Collections;

public class VillageManager : MonoBehaviour {
	public Transform Villager;
	public int villagerCount;
	// Use this for initialization
	void Start () {
		StartCoroutine (SpawnVillagers (villagerCount - 1));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator SpawnVillagers(int count) {
		// Spawn troops every 0.2 seconds
		for(int x = 0; x <= count; x++){

			Instantiate(Villager,transform.position,Quaternion.identity);
			yield return new WaitForSeconds(0.2f);
		}
		
		
	}
}
