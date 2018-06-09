using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class MenuObject : MonoBehaviour
{

    private Rigidbody2D _rb;

    private Vector3 _rotate = new Vector3(0.0f, 0.0f, -2.0f);

    [SerializeField] private float _speed = 2.5f;

    // Use this for initialization
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        Vector3 direction = new Vector3(1.0f, -1.0f, 0.0f);
        _rb.velocity = direction.normalized * _speed;
    }

    void Update()
    {
        if (transform.position.y <= -7.0f)
        {
            Vector3 newPos = transform.position;
            newPos.y *= -1.0f;
            newPos.x -= 4.0f;
            transform.position = newPos;
        }
        else if (transform.position.x >= 10.0f)
        {
            Vector3 newPos = transform.position;
            newPos.x *= -1.0f;
            transform.position = newPos;
        }

        transform.Rotate(_rotate);
    }
}
