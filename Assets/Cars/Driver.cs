using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour {
	private Rigidbody2D carBody;
	public Vector2 velocity;
	private float acc;
	private float turnAngle;
	private float radius;
    private IEnumerator varSACoroutine;

	private float Truncate(float val, int digits){
		float mult = Mathf.Pow (10, digits);
		float result = Mathf.Round(mult * val) / mult;
		return result;
	}
	// Use this for initialization
	void Start () {
		turnAngle = 0;
		carBody = GetComponent<Rigidbody2D> ();
		carBody.velocity = velocity;
	}
	private void setAcceleration(float newAcc){
		acc = newAcc;
	}
	private void setTurnAngle(float newAng){
		turnAngle = newAng;
		Debug.Log ("Setting turn: " + turnAngle.ToString());
		radius = 2f / Mathf.Sin(turnAngle);
		radius = radius * Mathf.Cos (turnAngle);
		radius += 0.5f;
	}
    private void setTurnRadius(float turnRadius)
    {
        radius = turnRadius;
    }

	private void turn(){
		float xVel = transform.InverseTransformDirection (carBody.velocity).x;
		if (radius == 0 || xVel < 0.0001f) {
			return;
		}
		transform.Rotate (Vector3.forward * (360f * xVel/(2f * Mathf.PI * radius) * Time.deltaTime));
	}

    IEnumerator StopAcceleration(float time)
    {
        yield return new WaitForSeconds(time);
        setAcceleration(0);
        varSACoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RoadInfo check = other.GetComponent<RoadInfo>();
        if (check != null && !check.isEntrance())
        {
            setTurnRadius(check.getTurnRadiusDir());
            velocity = transform.InverseTransformDirection(carBody.velocity);
            this.transform.rotation = Quaternion.Euler(0, 0, check.getExitAngle());
            this.transform.position = check.getExitPos();
            velocity.x = Truncate(velocity.x, 2);
            velocity.y = 0;
        }
        if (check != null)
        {
            float speedLimit = check.getSpeedLimit();
            if (velocity.x != speedLimit)
            {
                float newAcc = (speedLimit - velocity.x) > 0 ? 3 : -3;
                float timeStop = (speedLimit - velocity.x) / newAcc;
                if (varSACoroutine != null)
                {
                    StopCoroutine(varSACoroutine);
                }
                setAcceleration(newAcc);
                varSACoroutine = StopAcceleration(timeStop);
                StartCoroutine(varSACoroutine);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        RoadInfo check = other.GetComponent<RoadInfo>();
        if (check != null && check.isEntrance())
        {
            setTurnRadius(check.getTurnRadiusDir());
        }
    }
    void FixedUpdate(){
		carBody.AddRelativeForce(new Vector2(acc*carBody.mass,0));
		velocity = transform.InverseTransformDirection(carBody.velocity);
		turn();
		velocity.x = Truncate (velocity.x, 2);
		velocity.y = 0;
		carBody.velocity = transform.TransformDirection(velocity);
	}
}
