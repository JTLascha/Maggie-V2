//Cody O'Connor
//
//NewMovingPlatform.cs


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMovingPlatform : MonoBehaviour {

    public GameObject platform;
    public float MoveSpeed;
    public Transform CurrentPos;    //current position
    public Transform[] points;      //allows for multiple points on the level for the platform to move to.
    public int PointSelection;

	// Use this for initialization
	void Start () {
        CurrentPos = points[PointSelection];
	}
	
	// Update is called once per frame
	void Update () {
        platform.transform.position = Vector3.MoveTowards(platform.transform.position, CurrentPos.position, Time.deltaTime * MoveSpeed);    //platform will move to next point

        if(platform.transform.position == CurrentPos.position)      //when it has reached the point it will either go to the next point or back to the beginning to the first point so
        {                                                           //platform will move in an infinite loop
            PointSelection++;
            if (PointSelection == points.Length)
            {
                PointSelection = 0;
            }
            CurrentPos = points[PointSelection];
        }
     
	}
}