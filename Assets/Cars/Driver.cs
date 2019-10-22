using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour {
	private Rigidbody2D carBody;
	private float time;
	public Vector2 velocity;
	private float acc;
	private float turnAngle;
	private float radius;

	private float Truncate(float val, int digits){
		float mult = Mathf.Pow (10, digits);
		float result = Mathf.Round(mult * val) / mult;
		return result;
	}
	// Use this for initialization
	void Start () {
		turnAngle = 0;
		time = 0;
		carBody = GetComponent<Rigidbody2D> ();
		carBody.velocity = velocity;
		setTurnAngle(0.785398f);
		setAcceleration(1f);
	}
	private void setAcceleration(float newAcc){
		acc = newAcc;
		Debug.Log ("Setting acceleration: " + newAcc.ToString());
	}
	private void setTurnAngle(float newAng){
		turnAngle = newAng;
		Debug.Log ("Setting turn: " + turnAngle.ToString());
		radius = 2f / Mathf.Sin(turnAngle);
		radius = radius * Mathf.Cos (turnAngle);
		radius += 0.5f;
	}
	private void turn(){
		float xVel = transform.InverseTransformDirection (carBody.velocity).x;
		if (turnAngle == 0 || xVel < 0.0001f) {
			return;
		}
		transform.Rotate (Vector3.forward * (360f * xVel/(2f * Mathf.PI * radius) * Time.deltaTime));
	}
	void FixedUpdate(){
		time += Time.deltaTime;
		if (time >= 1f) {
			time -= 1f;
			//setAcceleration (0);
			print(velocity);
		}
		carBody.AddRelativeForce(new Vector2(acc*carBody.mass,0));
		velocity = transform.InverseTransformDirection(carBody.velocity);
		turn();
		velocity.x = Truncate (velocity.x, 2);
		velocity.y = 0;
		carBody.velocity = transform.TransformDirection(velocity);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
