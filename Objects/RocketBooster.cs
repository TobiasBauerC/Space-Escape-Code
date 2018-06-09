using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RocketBooster : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRB;
    [SerializeField] private Text _boosterText;

    private FrictionJoint2D _frictionJoint;

    private int _swipeCount = 0;

    private float _rocketFuel = 50.0f;

    private bool _rocketsDead = false;

    // Use this for initialization
    void Start()
    {
        _frictionJoint = GetComponent<FrictionJoint2D>();
        _boosterText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (_rocketFuel <= 0.0f && !_rocketsDead)
        {
            _boosterText.text = "Recharge Boosters!";
            _frictionJoint.connectedBody = _playerRB;
            _rocketsDead = true;
        }
        else if (_rocketsDead)
        {
            if (_swipeCount >= 25)
            {
                _boosterText.text = "";
                _swipeCount = 0;
                _rocketFuel = 50.0f;
                _rocketsDead = false;
                _frictionJoint.connectedBody = null;
            }
        }
    }

    public void RechargeBooster()
    {
        if (_rocketsDead)
            _swipeCount++;
        else
            _rocketFuel--;
    }
}
