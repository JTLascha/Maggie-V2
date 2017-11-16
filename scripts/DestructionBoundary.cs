using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionBoundary : MonoBehaviour {

                                                //destroys laser bullets after boundary
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
