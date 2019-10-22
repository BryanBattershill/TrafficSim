using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {
	public GameObject car;
	public int carsToAdd;
	// Use this for initialization
	void Start () {
		for (int i = 0; i<carsToAdd; i++){
			Instantiate(car,new Vector3(0,i*2+2),Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
