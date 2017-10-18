using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    public Rigidbody2D rb;
    public float speed = 1f;
    public float point1;
    public float point2;                // point2 must always be further right or up 
    public bool horizontal = true;
    public int direction = 1;           // positive for point1 to point2, negative for point2 to p1

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        float here;                     // this is where the platform is
        if (horizontal){here = transform.position.x;}
        else { here = transform.position.y; }

        if(here > point2) { direction = -1; }
        else if(here < point1) { direction = 1; }

        float x = 0;
        float y = 0;
        if (horizontal) { x = speed * direction; }
        else { y = speed * direction; }

        rb.velocity = new Vector2(x, y);
	}
}
