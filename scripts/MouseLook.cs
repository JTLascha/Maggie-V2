using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour
{
	
	// speed is the rate at which the object will rotate
	public Vector3 look;
	public Vector3 loc; 
	void FixedUpdate () 
	{
		loc = transform.position;
		look =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		float x = look.x - loc.x;
		float y = look.y - loc.y;
		float z = 0;
		look = new Vector3 (x, y, z);

		Vector3 here = transform.position;
		float rot = Mathf.Atan2 (y,x);
		rot = Mathf.Rad2Deg * rot;

        if (transform.parent.transform.localScale.x < 0)
        {
            rot += 180;
        } // attempt at making the magnet not flip with maggie when she goes left
          

        Quaternion newrot = Quaternion.Euler (0, 0, rot);
		transform.rotation = newrot;
	}
}