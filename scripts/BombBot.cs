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
	public bool locked_on;
	private float explosion_radius = 3f;

	void Start () {
		locked_on = false;
	}

	void FixedUpdate() {
		if (locked_on) {
			Chase ();
		}
	}

	void Chase() {
		Rigidbody2D rb = GetComponent<Rigidbody2D> ();

		rb.velocity = chase_speed * (Vector2)(maggie.transform.position - transform.position).normalized;
	}

	void LockOn() {
		locked_on = true;
	}

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