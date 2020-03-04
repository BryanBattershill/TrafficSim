using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Brain : MonoBehaviour {
	public GameObject car;
	private Text frameCounterText;
	public int carsToAdd;
	private float time;
	// Use this for initialization
	void Start () {
		for (int i = 0; i<carsToAdd; i++){
			Instantiate(car,new Vector3(0,i*2+2),Quaternion.identity);
		}
		frameCounterText = GameObject.Find("FrameCounterText").GetComponent<Text>(); 
	}
	
	// Update is called once per frame
	void Update () {
		time += (Time.unscaledDeltaTime - time) * 0.1f;
		float fps = 1.0f / time;
		frameCounterText.text = fps.ToString().Split('.')[0];
	}
}
