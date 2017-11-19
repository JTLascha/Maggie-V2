using UnityEngine;
using System.Collections;

public class Saw : MonoBehaviour
{
    //This is the new script that originates from the Battery Bot script, but with added features

    public float delta = 1.5f;
    public float speed = 2.0f;
    private Vector3 center;
    public float chase_speed = 2.0f;
    private bool locked_on;
    private GameObject maggie;
    private Vector3 lockOnPosition;

    //To begin the object will have the center of its movement be set to its current position.
    //This way, no points have to be put in, you just put the prefab down and it moves in both directions.
    void Start()
    {
        maggie = GameObject.FindGameObjectWithTag("Player");
        center = transform.position;
        locked_on = false;              //This locked_on implies that there is no magnetic attraction to start
    }

    void Update()
    {
        //If there is a feeling of attraction from the magnet, the saw will move in the direction of the
        //last attraction it felt.
        if (locked_on) { transform.position += lockOnPosition * chase_speed * Time.deltaTime; }
        else
        {
            //By default the object will move back and forth around the center and slow near the edges
            Vector3 v = center;
            v.x = (v.x + delta * Mathf.Sin(Time.time * speed));
            transform.position = v;
        }
    }

    void LockOn()
    {
        //Once the magnetic attraction has been felt, it sets the locked_on variable to true and sets the current position to the
        //position that it was locked on at.
        locked_on = true;
        lockOnPosition = (maggie.transform.position - transform.position).normalized;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //On collision with the player, it will reset the level
        if (collision.gameObject.tag == "Player")
        {
            Application.LoadLevel(Application.loadedLevelName);
        }
    }
}