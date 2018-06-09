using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

[RequireComponent(typeof(Rigidbody2D))]

public class AsteroidObject : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector3 _rotate;

    AsteroidInfo asteroid;
    private string[] types = { "Small", "Medium", "Large" };

    // Use this for initialization
    public void StartAsteroid(Transform pTarget)
    {
        asteroid = Asteroids.Get(types[Random.Range(0, 3)]);
        transform.localScale = asteroid.scale;
        _rb = GetComponent<Rigidbody2D>();

        _rb.AddForce((pTarget.position - transform.position).normalized * asteroid.speed * Time.smoothDeltaTime);
        _rotate = new Vector3(0.0f, 0.0f, Random.Range(-1.0f, 1.0f));

        Destroy(gameObject, 30.0f); // incase object misses killzone
    }

    void Update()
    {
        transform.Rotate(_rotate);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<PlayerHealth>().ChangePlayerHealth(-1 * asteroid.damage);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "AsteroidKillZone")
        {
            //c.gameObject.GetComponent<DataManager>().OnAddPoints((int) Random.Range(100.0f, 200.0f));
            Destroy(gameObject);
        }

        if (c.gameObject.tag == "Player")
        {
            c.gameObject.GetComponent<PlayerController>().AddPoints();
            //Destroy(gameObject); 
        }
    }
}

