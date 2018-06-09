using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private Canvas _mainMenuC;
    [SerializeField] private Canvas _howToPlayC;
    [SerializeField] private Canvas _creditsC;

    [SerializeField] private SoundManager _soundMan;
    [SerializeField] private AudioClip _startSound;

    // Use this for initialization
    void Awake()
    {
        //GameData.Downloader.Init();

        _mainMenuC.enabled = true;
        _howToPlayC.enabled = false;
        _creditsC.enabled = false;
    }

    public void OnStart()
    {
        _soundMan.PlaySound(_startSound, 2.0f);
        StartCoroutine(NextLevel());
    }

    public void OnBack()
    {
        _mainMenuC.enabled = true;
        _howToPlayC.enabled = false;
        _creditsC.enabled = false;
    }

    public void OnHowToPlay()
    {
        _mainMenuC.enabled = false;
        _howToPlayC.enabled = true;
        _creditsC.enabled = false;
    }

    public void OnCredits()
    {
        _mainMenuC.enabled = false;
        _howToPlayC.enabled = false;
        _creditsC.enabled = true;
    }

    public void OnAudio()
    {
        _mainMenuC.enabled = false;
        _howToPlayC.enabled = false;
        _creditsC.enabled = false;
    }

    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(2);
    }
}
