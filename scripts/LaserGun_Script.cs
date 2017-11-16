using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun_Script : MonoBehaviour {
    public GameObject shot;				//put Bolt into field
    public float fireRate;				//input # fireRate							
    private float nextFire=0;
	
	//public GameObject gun;			//create vector point for shotSpawn
    //public Transform shotSpawn;		//input laser GO into shot variable
   
    void Update()
    {
        float angle;
        Vector3 mousePos;
        Vector3 objectPos;

        //constant shooting lasers
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, transform.position, transform.rotation);
        }

        //rotation of laser gun
        mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle, 0));
    }
}
