using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : MonoBehaviour
{

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<PlayerHealth>().ChangePlayerHealth(-1);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "AsteroidKillZone")
        {
            Destroy(gameObject);
        }

        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<PlayerController>().AddPoints();
            //Destroy(gameObject); 
        }
    }
}
