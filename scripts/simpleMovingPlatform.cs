using UnityEngine;
using System.Collections;

public class simpleMovingPlatform : MonoBehaviour {

    public Vector3 pos1;
    public Vector3 pos2;
    public float speed = 2f;
    float timer = 0f;
    bool direction = true;
	// Use this for initialization
	void Start () {
        pos1 = GetComponent<Transform>().position;
    }
	
	// Update is called once per frame
	void Update () {

        if (GetComponent<Transform>().position == pos1)
        {
            direction = true;
        }

        if (GetComponent<Transform>().position == pos2)
        {
            direction = false;
        }

        if(direction == true)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer -= Time.deltaTime;
        }

        GetComponent<Transform>().position = Vector3.Lerp(pos1, pos2, timer * speed);
	}
}
