using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameData : MonoBehaviour
{
    void Awake()
    {
        GameData.Downloader.Init();
        SceneManager.LoadScene(1);
    }
}
