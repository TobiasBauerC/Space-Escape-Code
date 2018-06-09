using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private bool _godMode = false;
    [SerializeField] private DataManager _dm;
    [SerializeField] private Text _gameOver;
    [SerializeField] private SoundManager _soundMan;
    [SerializeField] private AudioClip _deathSFX;

    private int _playerHealth;
    private int _maxHealth;

    public int health
    {
        get { return _playerHealth; }
    }

    void Start()
    {
        _maxHealth = _playerHealth = GameData.Constants.Get<int>("MaxLives");
        _gameOver.text = "";
    }

    public void ChangePlayerHealth(int pValue)
    {
        if (_godMode)
            return;

        if (pValue < 0 && _playerHealth > 0 && _playerHealth <= _maxHealth)
        {
            _playerHealth += pValue;
            _soundMan.PlaySound(_deathSFX);
        }
        else if (pValue > 0 && _playerHealth > 0 && _playerHealth < _maxHealth)
            _playerHealth += pValue;

        if (_playerHealth > _maxHealth)
            _playerHealth = _maxHealth;

        if (_playerHealth > 0)
        {
            _dm.UpdateLives(_playerHealth);
        }
        else
        {
            _playerHealth = 0;
            _dm.UpdateLives(_playerHealth);
            _dm.OnSaveData();

            GetComponent<PlayerController>().enabled = false;
            _gameOver.text = "GAME OVER!";
            StartCoroutine(MainMenu(3));
        }
    }

    private IEnumerator MainMenu(float time = 0)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(1);
    }
}
