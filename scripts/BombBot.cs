/* File:	BombBot.cs
 * Author:	Homer McMillan
 * 
 * Notes:	This script defines the behavior of the Bomb Bot enemy.
 * 
 * TODO:	Add further interactions with Maggie's magnet (repulsive/attractive force)
 */




using System.Collections;
using UnityEngine;

public class BombBot : damageable {
	public GameObject maggie;
	public float chase_speed = 10f;

	// current_destination is where the bomb bot is currently headed (either to patrol_start or patrol_end)
	public float patrol_speed = 2f;
	public Vector3 patrol_start;
	public Vector3 current_destination;
	public Vector3 patrol_end;


	public bool locked_on = false;
	public bool patrolling = true;
	private float explosion_radius = 3f;	

	void Start () {
		patrol_start = transform.position;
		current_destination = patrol_end;
	}

	void FixedUpdate() {
		if (locked_on) {
			Chase ();
		} else if (patrolling) {
			Patrol ();
		}
	}

	// Give chase to Maggie!
	void Chase() {
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();
		rb.velocity = chase_speed * (Vector2)(maggie.transform.position - transform.position).normalized;
	}

	// move back and forth between patrol_start and patrol_end
	void Patrol() {
		if (transform.position == current_destination && current_destination == patrol_end) {
			current_destination = patrol_start;
			transform.position = Vector2.MoveTowards (transform.position, current_destination, Time.deltaTime * patrol_speed);
		} else if (transform.position == current_destination) {
			current_destination = patrol_end;
			transform.position = Vector2.MoveTowards (transform.position, current_destination, Time.deltaTime * patrol_speed);
		} else {
			transform.position = Vector2.MoveTowards (transform.position, current_destination, Time.deltaTime * patrol_speed);
		}
	}

	void LockOn() {
		locked_on = true;
		patrolling = false;
	}

	// On collision, calls damage() on anything within the explosion radius
	private void OnCollisionEnter2D () {
		locked_on = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosion_radius);
		for (int i = 0; i < colliders.Length; i++) {
			damageable target = colliders [i].GetComponent<damageable>();
			if (target) {
				target.damage ();
			}
		}
	}

	override public void damage() {
		Destroy (gameObject);
	}
}	