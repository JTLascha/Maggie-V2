using UnityEngine;
using System.Collections;

public class StaticBatteryBot : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //On collision with the player, it will reset the level
        if (collision.gameObject.tag == "Player")
        {
            Application.LoadLevel(Application.loadedLevelName);
        }
        //On collision with a moving platform, it will become a child of the platform
        if (collision.transform.tag == "MovingPlatform")
        {
            transform.parent = collision.transform;
        }
    }
}
