using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public float speed;					//input speed into var field
    public bool bullet = true;			//bool for use in Boundary script
    private Rigidbody rb;
    
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.right * speed;
    }

    private void OnCollisionEnter()         //destruct on collision with anything
    {
        Destroy(gameObject);
    }
    //on collision check if object is damageable
    //if damageable==true
    //Damage funct
}
