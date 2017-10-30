using System.Collections;
using UnityEngine;

public class BombBot : MonoBehaviour {
	public GameObject maggie;
	public float chase_speed = 10f;
	public bool locked_on;

	void Start () {
		locked_on = false;
	}
	

	void Update () {
		if (locked_on) { Chase (); }
	}

	void Chase() {
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();

		if ((maggie.transform.position - transform.position).magnitude < 1f) {
			// stops moving and chasing. Only for testing right now
			locked_on = false;
			rb.velocity = new Vector2(0,0);

		} else {
			rb.velocity = chase_speed * (Vector2)(maggie.transform.position - transform.position).normalized;
		}
	}

	void LockOn() {
		locked_on = true;
	}
}	