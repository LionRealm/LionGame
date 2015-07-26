using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	public float Speed = 0.1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.eulerAngles += new Vector3(0,Speed,0);
	}
}
