using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum GameMode
{
    BeforeStart,
    InGame,
    Ranking
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
     [SerializeField]
     Text _cuontDownText = null;
    public GameMode _gameMode = GameMode.BeforeStart;
    private SceneChanger _sceneChanger = new SceneChanger();

    void Start()
    {
        Initialization();
    }

    void Initialization()
    {
        gameObject.tag = "Lap";
        _gameMode = GameMode.BeforeStart;
        _cuontDownText.enabled = false;
        CountDown();
    }

    void Update()
    {
        switch (_gameMode)
        {
            case GameMode.BeforeStart:
                break;
            case GameMode.InGame:
                break;
            case GameMode.Ranking:
                break;
        }
    }

    //スタート前のカウントダウン
    async void CountDown(float waiteTime = 3.0f)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(waiteTime));
        _cuontDownText.enabled = true;
        _cuontDownText.text = "3";
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        _cuontDownText.text = "2";
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        _cuontDownText.text = "1";
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        _cuontDownText.text = "START";
        _gameMode = GameMode.InGame;
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        _cuontDownText.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
    }
}
