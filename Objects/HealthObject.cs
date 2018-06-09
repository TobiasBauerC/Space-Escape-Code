using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class HealthObject : MonoBehaviour
{

    private Rigidbody2D _rb;
    private PlayerHealth _playerHealth;

    // Use this for initialization
    void Start()
    {
        _playerHealth = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerHealth>();

        _rb = GetComponent<Rigidbody2D>();

        Vector3 dest = new Vector3(-13.0f, Random.Range(-4.0f, 4.0f), 0.0f);
        _rb.AddForce((dest - transform.position).normalized * GameData.Constants.Get<float>("HealthForce") * Time.smoothDeltaTime);

        Destroy(gameObject, 60.0f); // incase object misses killzone
    }

    void Update()
    {
#if UNITY_EDITOR
        for (int i = 0; i < 3; i++)
        {
            if (Input.GetMouseButton(i))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider == GetComponent<Collider2D>())
                {
                    if (_playerHealth.health <= 0)
                        Destroy(gameObject);

                    _playerHealth.ChangePlayerHealth(2);
                    Destroy(gameObject);
                }
            }
        }
#endif
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount == 1)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);

            if (hit.collider == GetComponent<Collider2D>())
            {
                Debug.Log("Target Position: " + hit.collider.gameObject.transform.position);

                if (_playerHealth.health <= 0)
                    Destroy(gameObject);

                _playerHealth.ChangePlayerHealth(2);
                Destroy(gameObject);
            }
        }
#endif
    }
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "AsteroidKillZone")
        {
            Destroy(gameObject);
        }
    }
}
