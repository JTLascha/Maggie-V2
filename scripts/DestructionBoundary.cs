using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionBoundary : MonoBehaviour {

/*Attach to 2D plane w/ Box Collider
 lasers will delete when contact is made with edge of plane */
 
                                               
    void OnTriggerExit(Collider other)
    {
        Laser controlscript = other.GetComponent<Laser>();
		if(controlscript!=null){				//returns not null if compenent is a laser
			//bullet = controlscript.bullet;	//if it is a laser, then the boundary deletes it
			//if (bullet==true)					//commented out because all lasers should have bullet==true
				Destroy(other.gameObject);
		}
    }
}
