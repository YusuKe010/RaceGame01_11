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

    Vector3 _force = Vector3.zero; //�O������󂯂��

    void Start()
    {
    }


    void Update()
    {
        //���ݎԂ�������W�̑������擾
        CartSystem.Attribute attr = CartSystem.Instance.GetAttribute(transform.localPosition);
        Debug.Log($"{attr}");
        //�X�y�[�X�L�[�������ꂽ�����
        if (Input.GetKey(KeyCode.Space))
        {
            _cartSpeed += _cartAccel * Time.deltaTime;
            _cartSpeed = Mathf.Clamp(_cartSpeed, 0.0f, MAX_SPEED);
        }

        //��R�i�����j
        _cartSpeed *= (attr == CartSystem.Attribute.Dart) ? 0.98f : 0.99f;
        _force *= (attr == CartSystem.Attribute.Dart) ? 0.9f : 0.99f;

        //���E�̖��L�[�Ő���
        float handle = Input.GetAxis("Horizontal");

        if (handle == 0) _cartRotateSpeed *= 0.9f;
        _cartRotateSpeed += _rotationAccel * handle * Time.deltaTime;
        _cartRotateSpeed = Mathf.Clamp(_cartRotateSpeed, -MAX_ROTATION_SPEED, MAX_ROTATION_SPEED);
        _cartRotate += _cartRotateSpeed * Time.deltaTime;
        transform.localEulerAngles = new Vector3(0.0f, _cartRotate, 0.0f);

        //�ړ�
        Vector3 prevPosition = transform.localPosition;
        transform.localPosition += transform.forward * Time.deltaTime * _cartSpeed + _force * Time.deltaTime;

        //�ǂƂ̏Փ˔���
        attr = CartSystem.Instance.GetAttribute(transform.localPosition);
        if (attr == CartSystem.Attribute.Wall)
        {
            //�ǂƂ̐ڐG�_�����߂�
            Vector3 newPosition;
            Vector3 dir = CartSystem.Instance.GetAttributeDetail(transform.localPosition, prevPosition, out newPosition);
            //�ǂ���̔������󂯂�
            _force = dir * 8.0f;
            //�X�s�[�h�𗎂Ƃ�
            _cartSpeed *= 0.75f;

            //���ׂ���̂߂荞�݂��������ꂽ�ʒu�ɍX�V
            transform.localPosition = newPosition;
        }
    }
}
