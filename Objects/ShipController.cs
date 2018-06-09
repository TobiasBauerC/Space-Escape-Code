using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Ship Settings")]
    [SerializeField]
    private Transform _playerTransform;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;
    [SerializeField] private float _speed;

    [Header("Proj Settings")]
    [SerializeField]
    private GameObject _projectile;
    [SerializeField] private float _projSpeed;
    [SerializeField] private GameObject[] _ships;

    private Rigidbody2D _rb2d;

    private WaitForSeconds _openFireTime = new WaitForSeconds(0.5f);
    private WaitForSeconds _originTime = new WaitForSeconds(3.0f);

    private Vector2 _forwardVelocity = new Vector2(-5.0f, 0.0f);
    private Vector2 _backwardVelocity = new Vector2(15.0f, 0.0f);
    private Vector3 _origin;

    private int _runCount = 0;
    private string _state = "Out";
    private bool _isMoving = false;
    private Vector3 _newPos;

    // Use this for initialization
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.isKinematic = true;

        _origin = transform.position;
    }

    void Update()
    {
        if (_state == "Out")
            return;

        if (Vector3.Distance(transform.position, _newPos) < 0.5f && _isMoving)
        {
            _rb2d.velocity = Vector3.zero;
            _isMoving = false;
            StartCoroutine(OpenFire());
        }
        else if (Vector3.Distance(transform.position, _newPos) > 0.5f && _isMoving)
        {
            Vector3 direction = _newPos - transform.position;
            _rb2d.velocity = Vector2.zero;
            _rb2d.velocity = direction.normalized * _speed;
        }
        else
        {
            _rb2d.velocity = Vector2.zero;
        }
    }

    public void NewDestination()
    {
        _newPos = new Vector3(Random.Range(_minX, _maxX), Random.Range(_minY, _maxY), _playerTransform.position.z);
        Vector3 direction = _newPos - transform.position;
        _rb2d.velocity = Vector2.zero;
        _rb2d.velocity = direction.normalized * _speed;
        _isMoving = true;
    }

    public void Enter()
    {
        _state = "In";
        _runCount = 0;
        _rb2d.isKinematic = false;
        _rb2d.velocity = _forwardVelocity;
        NewDestination();
    }

    public void Exit()
    {
        _state = "Out";
        StopAllCoroutines();

        _rb2d.velocity = Vector2.zero;
        _rb2d.velocity = _backwardVelocity;
        StartCoroutine(Reposition());
    }

    private void Shoot(GameObject pShip)
    {
        var proj = (GameObject)Instantiate(_projectile, pShip.transform.position, _projectile.transform.rotation);
        proj.GetComponent<Rigidbody2D>().velocity = new Vector2(_projSpeed, 0.0f);
    }

    private IEnumerator OpenFire()
    {
        _rb2d.velocity = Vector2.zero;

        foreach (GameObject ship in _ships)
        {
            Shoot(ship);
        }

        yield return _openFireTime;

        if (_runCount < 5)
        {
            _runCount++;
            StartCoroutine(OpenFire());
        }
        else
        {
            _runCount = 0;
            NewDestination();
        }
    }

    private IEnumerator Reposition()
    {
        yield return _originTime;
        _rb2d.velocity = Vector2.zero;
        _rb2d.isKinematic = true;
        transform.position = _origin;
    }
}
