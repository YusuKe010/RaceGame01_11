using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Cart : MonoBehaviour
{
    [SerializeField] private Text _goalText;
    [SerializeField] float _cartAccel = 10.0f;
    [SerializeField] float _cartRotateSpeed = 0.0f;
    float _cartSpeed = 0.0f;
     float _cartRotate = 0.0f;
     float _rotationAccel = 360.0f;
    
    const float MAX_SPEED = 40.0f;
    const float MAX_ROTATION_SPEED = 360.0f / 3f;

    private int _lapCount = 0;
    public int LapCount => _lapCount;
    private Rigidbody _rb;

    void Start()
    {
        Initialization();
        _rb = GetComponent<Rigidbody>();
    }

    void Initialization()
    {
        gameObject.tag = "Player";
        _lapCount = 0;
        _goalText.enabled = false;
    }


    void Update()
    {
        //スペースキーが押されたら加速
        if (Input.GetKey(KeyCode.Space))
        {
            _cartSpeed += _cartAccel;
            _cartSpeed = Mathf.Clamp(_cartSpeed, 0.0f, MAX_SPEED);
        }
        _cartSpeed *= 0.995f;

        //左右の矢印キーで旋回
        float handle = Input.GetAxis("Horizontal");

        if (handle == 0)
        {
            _cartRotateSpeed *= 0.9f;
        }
        _cartRotateSpeed += _rotationAccel * handle * Time.deltaTime;
        _cartRotateSpeed = Mathf.Clamp(_cartRotateSpeed, -MAX_ROTATION_SPEED, MAX_ROTATION_SPEED);
        _cartRotate += _cartRotateSpeed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0.0f, _cartRotate, 0.0f);

        
        //移動
        //Vector3 prevPosition = transform.localPosition;
        //transform.localPosition += transform.forward * Time.deltaTime * _cartSpeed + _force * Time.deltaTime;

    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.forward * _cartSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lap"))
        {
            _lapCount++;
            if (_lapCount >= 3 && _goalText)
            {
                _goalText.enabled = true;
                _goalText.text = "GOAL!!";
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Dart"))
        {
            _cartSpeed *= 0.9f;
        }
    }
}
