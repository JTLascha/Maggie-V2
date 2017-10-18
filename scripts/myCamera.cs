using UnityEngine;
using System.Collections;

public class myCamera : MonoBehaviour {
    public Transform target;
    public float offsetX, offsetY, offsetZ;
    public float minX = -2000;
    public float minY = -2000;
    public float minZ = -2000;
    public float maxX = 2000;
    public float maxY = 2000;
    public float maxZ = 2000;
    public bool autoOffset = true;
    // Use this for initialization
    void Start () {
        if (autoOffset == true)
        {
            offsetX = (transform.position - target.position).x;
            offsetY = (transform.position - target.position).y;
            offsetZ = (transform.position - target.position).z;
        }
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(Mathf.Clamp((target.position.x + offsetX), minX, maxX), 
            Mathf.Clamp((target.position.y + offsetY), minY, maxY), 
            Mathf.Clamp((target.position.z + offsetZ), minZ, maxZ));

    }
}
