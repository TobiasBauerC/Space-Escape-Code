using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{

    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private GameObject _healthPackPrefab;
    [SerializeField] private Transform _playerPos;

    [SerializeField] private ShipController _mainShip;
    [SerializeField] private GameObject _blackhole;

    private enum ObstacleType { Asteroids, Ships, Blackhole };
    private ObstacleType _obstacleType = ObstacleType.Asteroids;

    private Coroutine _switcherCor;
    private Coroutine _asteroidSpawnerCor;
    private Coroutine _currentObstacleCor;

    private float _asteroidSpawnTimeMin = 5.0f;
    private float _asteroidSpawnTimeMax = 8.0f;

    public Transform playerPos
    {
        get { return _playerPos; }
    }

    // Use this for initialization
    void Start()
    {
        _currentObstacleCor = StartCoroutine(CurrentObstacle());
        _asteroidSpawnerCor = StartCoroutine(AsteroidSpawner());
        StartCoroutine(HealthSpawner());
    }

    public void TypeSwitch()
    {
        float type = Random.value;

        switch (_obstacleType)
        {
            case ObstacleType.Asteroids:
                StopCoroutine(_asteroidSpawnerCor);
                if (type > 0.5)
                    _obstacleType = ObstacleType.Ships;
                else
                    _obstacleType = ObstacleType.Blackhole;
                break;

            case ObstacleType.Ships:
                if (type > 0.5)
                    _obstacleType = ObstacleType.Asteroids;
                else
                    _obstacleType = ObstacleType.Blackhole;
                _mainShip.Exit();
                break;

            case ObstacleType.Blackhole:
                StopCoroutine(_currentObstacleCor);
                if (type > 0.5)
                    _obstacleType = ObstacleType.Ships;
                else
                    _obstacleType = ObstacleType.Asteroids;
                break;
        }

        _switcherCor = StartCoroutine(Switcher());
    }

    private IEnumerator AsteroidSpawner()
    {
        yield return new WaitForSeconds(Random.Range(_asteroidSpawnTimeMin, _asteroidSpawnTimeMax));

        var asteroid = (GameObject)Instantiate(_asteroidPrefab,
            new Vector2(12.0f,
            Random.Range(-4.0f, 4.0f)),
            _asteroidPrefab.transform.rotation);

        asteroid.GetComponent<AsteroidObject>().StartAsteroid(_playerPos);

        _asteroidSpawnerCor = StartCoroutine(AsteroidSpawner());
    }

    private IEnumerator HealthSpawner()
    {
        yield return new WaitForSeconds(Random.Range(7.5f, 30.0f));
        Instantiate(_healthPackPrefab, new Vector2(12.0f, Random.Range(-4.0f, 4.0f)), _healthPackPrefab.transform.rotation);
        StartCoroutine(HealthSpawner());
    }

    public IEnumerator Switcher()
    {
        yield return new WaitForSeconds(5.0f);

        switch (_obstacleType)
        {
            case ObstacleType.Asteroids:
                if (_asteroidSpawnTimeMin > 1.5f)
                {
                    _asteroidSpawnTimeMin -= 0.2f;
                    _asteroidSpawnTimeMax -= 0.2f;
                }
                _asteroidSpawnerCor = StartCoroutine(AsteroidSpawner());
                break;

            case ObstacleType.Ships:
                _mainShip.Enter();
                break;

            case ObstacleType.Blackhole:
                _blackhole.SetActive(true);
                break;
        }

        _currentObstacleCor = StartCoroutine(CurrentObstacle());
    }

    private IEnumerator CurrentObstacle()
    {
        float wait = Random.Range(15.0f, 25.0f);
        yield return new WaitForSeconds(wait);
        TypeSwitch();
    }
}
