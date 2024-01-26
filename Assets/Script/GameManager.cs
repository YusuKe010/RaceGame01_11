using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
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

    [SerializeField] private Text _timerText = null;
    [SerializeField] Text _cuontDown = null;
    private float _worldTimer = 0.0f;
    private GameMode _gameMode = GameMode.BeforeStart;

    void Start()
    {
        Initialization();
    }

    void Initialization()
    {
        gameObject.tag = "Lap";
        _gameMode = GameMode.BeforeStart;
        CountDown();
    }

    void Update()
    {
        switch (_gameMode)
        {
            case GameMode.BeforeStart:
                break;
            case GameMode.InGame:
                _worldTimer += Time.deltaTime;
                break;
            case GameMode.Ranking:
                break;
        }
    }

    //スタート前のカウントダウン
    async void CountDown(float waiteTime = 3.0f)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(waiteTime));
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        Debug.Log("3");
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        Debug.Log("2");
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        Debug.Log("1");
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f));
        Debug.Log("START");
        _gameMode = GameMode.InGame;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Cart cart = other.GetComponent<Cart>();
            
        }
    }
}
