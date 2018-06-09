using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    private bool _spawned = false;
    private Rigidbody2D _rb2d;
    private GameObject _metalRef;
    private float _xSpeed = -0.0002f;

    [SerializeField] private float _originPosition;
    [SerializeField] private GameObject _metalPref;
    [SerializeField] bool useObstacle = true;

    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.bodyType = RigidbodyType2D.Kinematic;
        _rb2d.velocity = new Vector2(GameData.Constants.Get<float>("BackgroundSpeed"), 0.0f);
    }

    void Update()
    {

        if (_rb2d.velocity.x >= -4.0f)
            _rb2d.velocity = new Vector2(_rb2d.velocity.x + _xSpeed, 0.0f);

        if (transform.position.x <= -_originPosition)
        {
            transform.position = new Vector2(_originPosition, 0.0f);

            if (!useObstacle)
                return;

            Vector3 rot = new Vector3(0, 0, Random.Range(0.0f, 360.0f));

            if (!_metalRef)
            {
                float x = Random.Range(10.0f, 27.0f);
                float y = Random.Range(-1.5f, 1.5f);
                float z = gameObject.transform.position.z;
                _metalRef = (GameObject)Instantiate(_metalPref, new Vector3(x, y, z), Quaternion.Euler(rot));
                _metalRef.transform.parent = gameObject.transform;
                _spawned = true;
            }
            else
            {
                _metalRef.transform.rotation = Quaternion.Euler(rot);
            }
        }
    }

    public void BlackholeEnabled()
    {
        if (_metalRef)
            Destroy(_metalRef);
    }
}
