using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public float speed;
    public bool bullet = true;
    private Rigidbody rb;
    
   
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.right * speed;
    }

    private void OnCollisionEnter()         //desruct on collision
    {
        Destroy(gameObject);
    }
    //on collision check if object is damageable
    //if damageable==true
    //Damage funct
}
