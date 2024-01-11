using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    const float MAX_SPEED = 40.0f;
    const float MAX_ROTATION_SPEED = 360.0f / 3f;

    float _cartSpeed = 0.0f;
    float _cartAccel = 10.0f;
    float _cartRotateSpeed = 0.0f;
    float _cartRotate = 0.0f;
    float _rotationAccel = 360.0f;

    Vector3 _force = Vector3.zero; //外部から受ける力

    void Start()
    {
    }


    void Update()
    {
        //現在車がいる座標の属性を取得
        CartSystem.Attribute attr = CartSystem.Instance.GetAttribute(transform.localPosition);
        Debug.Log($"{attr}");
        //スペースキーが押されたら加速
        if (Input.GetKey(KeyCode.Space))
        {
            _cartSpeed += _cartAccel * Time.deltaTime;
            _cartSpeed = Mathf.Clamp(_cartSpeed, 0.0f, MAX_SPEED);
        }

        //抵抗（減速）
        _cartSpeed *= (attr == CartSystem.Attribute.Dart) ? 0.98f : 0.99f;
        _force *= (attr == CartSystem.Attribute.Dart) ? 0.9f : 0.99f;

        //左右の矢印キーで旋回
        float handle = Input.GetAxis("Horizontal");

        if (handle == 0) _cartRotateSpeed *= 0.9f;
        _cartRotateSpeed += _rotationAccel * handle * Time.deltaTime;
        _cartRotateSpeed = Mathf.Clamp(_cartRotateSpeed, -MAX_ROTATION_SPEED, MAX_ROTATION_SPEED);
        _cartRotate += _cartRotateSpeed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0.0f, _cartRotate, 0.0f);

        //移動
        Vector3 prevPosition = transform.localPosition;
        transform.localPosition += transform.forward * Time.deltaTime * _cartSpeed + _force * Time.deltaTime;

        //壁との衝突判定
        attr = CartSystem.Instance.GetAttribute(transform.localPosition);
        if (attr == CartSystem.Attribute.Wall)
        {
            //壁との接触点を求める
            Vector3 newPosition;
            Vector3 dir = CartSystem.Instance.GetAttributeDetail(transform.localPosition, prevPosition, out newPosition);
            //壁からの反発を受ける
            _force = dir * 8.0f;
            //スピードを落とす
            _cartSpeed *= 0.75f;

            //かべからのめり込みが解消された位置に更新
            transform.localPosition = newPosition;
        }
    }
}
