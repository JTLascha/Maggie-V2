using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionBoundary : MonoBehaviour {

    public bool bullet;
                                                //destroys laser bullets after boundary
    void OnTriggerExit(Collider other)
    {
        Laser controlscript = other.GetComponent<Laser>();
        bullet = controlscript.bullet;
        if (bullet==true)
            Destroy(other.gameObject);
    }
}
