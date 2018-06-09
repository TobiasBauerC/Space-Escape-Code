using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private DataManager _dataManager;
    [SerializeField] private RocketBooster _rocketBooster;

    private const float MIN_SWIPE_LENGTH = 50.0F;
    private const float MAX_SWIPE_TIME = 0.35f;

    private float num;
    private float _elapsedTime;
    private float _xPos;

    private bool _startTimer;

    void Start()
    {
        _elapsedTime = 0.0f;
        _xPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.x > _xPos)
        {
            Vector2 newPos = transform.position;
            newPos.x -= 0.1f;
            transform.position = newPos;
            return;
        }
        else if (transform.position.x < _xPos)
        {
            Vector2 newPos = transform.position;
            newPos.x = _xPos;
            transform.position = newPos;
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            JetMovment(1.0f);
            _rocketBooster.RechargeBooster();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            JetMovment(-1.0f);
            _rocketBooster.RechargeBooster();
        }
#endif
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _startTimer = true;
                _elapsedTime = 0f;

            }

            if (touch.phase == TouchPhase.Ended && _elapsedTime < MAX_SWIPE_TIME)
            {
                if (touch.deltaPosition.y > 0.0f)
                    JetMovment(1.0f);
                else if (touch.deltaPosition.y < 0.0f)
                    JetMovment(-1.0f);

                _rocketBooster.RechargeBooster();
            }
        }
#endif

        if (_startTimer)
        {
            if (_elapsedTime < MAX_SWIPE_TIME)
            {
                _elapsedTime += Time.deltaTime;
            }

            if (_elapsedTime >= MAX_SWIPE_TIME)
            {
                _startTimer = false;
            }
        }
    }

    private void JetMovment(float pDirection)
    {
        _rb.AddForce(new Vector2(0.0f, Random.Range(Constants.Get<float>("JetForceMin"), Constants.Get<float>("JetForceMax")) * pDirection));
    }

    public void AddPoints()
    {
        if (GetComponent<PlayerHealth>().health > 0)
            _dataManager.OnAddPoints((int)Random.Range(25.0f, 75.0f));
    }
}
