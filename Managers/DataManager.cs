using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GameData;

public class DataManager : MonoBehaviour
{
    [Header("Highscore Variables")]
    [SerializeField]
    private Text _highScoreText;
    [SerializeField] private Text _currentScoreText;
    [SerializeField] private Text _livesText;
    [SerializeField] private Text _winLoseText;
    [SerializeField] private Text _pauseText;

    [Header("Pause")]
    [SerializeField]
    private PlayerController _player;
    [SerializeField] private PlayerHealth _playerHealth;

    [Header("SFX")]
    [SerializeField]
    private SoundManager _soundMan;
    [SerializeField] private AudioClip _pauseSound;

    private string _currentScoreLocalized;
    private string _highScoreLocalized;
    private string _livesLocalized;

    private int _highScore;
    private int _currentScore;

    private bool _paused = false;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 10);
        Physics2D.IgnoreLayerCollision(9, 11);
        Physics2D.IgnoreLayerCollision(9, 12);
        Physics2D.IgnoreLayerCollision(10, 11);
        Physics2D.IgnoreLayerCollision(10, 12);
        Physics2D.IgnoreLayerCollision(11, 12);

        _currentScore = 0;
        _highScore = PlayerPrefs.GetInt(Constants.Get<string>("HighScoreKey"));
        _winLoseText.text = "";

        _currentScoreLocalized = _currentScoreText.GetComponent<TextLangSetter>().GetText();
        _highScoreLocalized = _highScoreText.GetComponent<TextLangSetter>().GetText();
        _livesLocalized = _livesText.GetComponent<TextLangSetter>().GetText();

        UpdateScore();
        UpdateLives(Constants.Get<int>("MaxLives"));
    }

    public void OnAddPoints(int pPoints)
    {
        _currentScore += pPoints;
        UpdateScore();
    }

    public void OnClearDate()
    {
        _currentScore = 0;
        _highScore = 0;
        PlayerPrefs.DeleteAll();
        UpdateScore();
    }

    public void OnPause()
    {
        if (_playerHealth.health <= 0)
            return;

        if (_paused)
        {
            _pauseText.text = "Pause";
            _player.enabled = true;
            Time.timeScale = 1.0f;
        }
        else
        {
            _pauseText.text = "Unpause";
            _player.enabled = false;
            Time.timeScale = 0.0f;
        }

        _paused = !_paused;

        _soundMan.PlaySound(_pauseSound);
    }

    public void OnQuit()
    {
        Time.timeScale = 1.0f;
        _soundMan.Save();
        OnSaveData();
        SceneManager.LoadScene(1);
    }

    public void OnSaveData()
    {
        if (_currentScore > _highScore)
        {
            _highScore = _currentScore;
            _winLoseText.text = "NEW HIGHSCORE!";
            UpdateScore();
            Save();
        }
        else
        {
            _winLoseText.text = "NO NEW HIGHSCORE!";
        }
    }

    public void OnSubtractPoints(int pPoints)
    {
        _currentScore = (_currentScore - pPoints < 0) ? 0 : _currentScore - pPoints;
        UpdateScore();
    }

    private void Save()
    {
        PlayerPrefs.SetInt(Constants.Get<string>("HighScoreKey"), _highScore);
    }

    private void UpdateScore()
    {
        _currentScoreText.text = string.Format("{0}: {1}", _currentScoreLocalized, _currentScore);
        _highScoreText.text = string.Format("{0}: {1}", _highScoreLocalized, _highScore);
    }

    public void UpdateLives(int pLives)
    {
        _livesText.text = string.Format("{0}: {1}", _livesLocalized, pLives);
    }
}
