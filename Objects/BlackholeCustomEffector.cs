using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeCustomEffector : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _backgroundManager;
    [SerializeField] private ObstacleSpawn _obstacleSpawn;
    [SerializeField] private float _moveAmount;
    [SerializeField] private float _minDistance;

    private int _tapCount;

    private float _rotAmount = -0.5f;

    private Transform _playerTran;
    private PlayerController _playerCont;
    private Rigidbody2D _playerRB;

    private WaitForSeconds _checkPlayerWait = new WaitForSeconds(0.1f);
    private Coroutine _playerDirCheck;

    private BackgroundMovement[] _backgroundMovements;

    // Use this for initialization
    void Awake()
    {
        if (!_player)
        {
            Debug.LogError("NULL REF: NO PLAYER GAMEOBJECT");
        }

        _backgroundMovements = _backgroundManager.GetComponentsInChildren<BackgroundMovement>();

        _playerTran = _player.GetComponent<Transform>();
        _playerCont = _player.GetComponent<PlayerController>();
        _playerRB = _player.GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        _tapCount = 0;
        _playerCont.enabled = false;
        _playerRB.constraints = RigidbodyConstraints2D.None;
        _playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        _playerDirCheck = StartCoroutine(PlayerDirCheck());
    }

    void OnDisable()
    {
        StopCoroutine(_playerDirCheck);
        if (_playerCont)
            _playerCont.enabled = true;
        if (_obstacleSpawn)
            _obstacleSpawn.TypeSwitch();
        _playerRB.velocity = Vector2.zero;
        _playerRB.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            _tapCount++;
            TapCheck();
        }

#elif UNITY_ANDROID || UNITY_IOS
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _tapCount++;
            TapCheck();
        }
#endif

        foreach (BackgroundMovement script in _backgroundMovements)
            script.BlackholeEnabled();

        transform.Rotate(0.0f, 0.0f, _rotAmount);

        if (Vector3.Distance(_playerTran.position, transform.position) < _minDistance)
        {
            _tapCount = 0;
            _playerRB.velocity = Vector2.zero;
            StopCoroutine(_playerDirCheck);

            if (_playerTran.localScale.x >= 0.01)
            {
                _playerTran.Rotate(0.0f, 0.0f, 10.0f);
                Vector3 scale = _playerTran.localScale;
                scale.x -= 0.01f;
                scale.y -= 0.01f;
                scale.z -= 0.01f;
                _playerTran.localScale = scale;
            }
            else
            {
                _player.GetComponent<PlayerHealth>().ChangePlayerHealth(-1000);
            }
        }
    }

    void TapCheck()
    {
        if (_tapCount >= 10)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator PlayerDirCheck()
    {
        _playerRB.velocity = Vector2.zero;
        _playerRB.AddForce((_playerTran.position - transform.position).normalized * _moveAmount * Time.smoothDeltaTime);

        yield return _checkPlayerWait;

        _playerDirCheck = StartCoroutine(PlayerDirCheck());
    }
}
