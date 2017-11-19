using UnityEngine;
using System.Collections;

public class BatteryBot : MonoBehaviour {
    //This script has been modified to change how the movement works

    public float delta = 1.5f;
    public float speed = 2.0f;
    private Vector3 center;

    //To begin the object will have the center of its movement be set to its current position.
    //This way, no points have to be put in, you just put the prefab down and it moves in both directions.
    void Start()
    {
        center = transform.position;
    }

    void Update()
    {
        //By default the object will move back and forth around the center and slow near the edges
        Vector3 v = center;
        v.x = (v.x + delta * Mathf.Sin(Time.time * speed));
        transform.position = v;
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
