using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
    
    GameObject point;
    SpriteRenderer color;
    bool active = false;

	// Use this for initialization
	void Start () {
        point = GetComponent<GameObject>();
        color = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            active = true;
        }
    }

    // Update is called once per frame
    void Update () {
        if (active)
        {
            // color.color = new Color(0, 149, 255, 255);
            Color setCol;
            ColorUtility.TryParseHtmlString("#0094FFFF", out setCol);
            color.color = setCol;
        }
        else
        {
            // color.color = new Color(0, 149, 255, 255);
            Color setCol;
            ColorUtility.TryParseHtmlString("#00FFAAFF", out setCol);
            color.color = setCol;
        }
	}
}
