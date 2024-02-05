using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField] private Button _button;
    void Start()
    {
        _button.onClick.AddListener(()=> SceneChanger.Instance.LoadScene("Course"));
    }
}
