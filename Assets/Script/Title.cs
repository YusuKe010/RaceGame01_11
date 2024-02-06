using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] private Button _button;
    void Start()
    {
        _button.onClick.AddListener(()=> button());
    }

    async void button()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        SceneChanger.Instance.LoadScene("Course");
    }
}
