using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalObject : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<PlayerHealth>().ChangePlayerHealth(-1);
            Destroy(gameObject);
        }
    }
}
