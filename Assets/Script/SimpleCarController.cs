using UnityEngine;
using System.Collections.Generic;
using System;

namespace Script
{
    [Serializable]
    public class AxleInfo {
        public WheelCollider leftWheel;
        public WheelCollider rightWheel;
        public bool motor; //このホイールがエンジンにアタッチされているかどうか
        public bool steering; // このホイールがハンドルの角度を反映しているかどうか
    }
    
    public class SimpleCarController : MonoBehaviour
    { 
        public List<AxleInfo> axleInfos; 
        public float maxMotorTorque;
        public float maxSteeringAngle;
        private Rigidbody _rb;
        [SerializeField] float _cartAccel = 10.0f;
        [SerializeField] float _cartRotateSpeed = 0.0f;
        float _cartSpeed = 0.0f;
        float _cartRotate = 0.0f;
        float _rotationAccel = 360.0f;
    
        const float MAX_SPEED = 40.0f;
        const float MAX_ROTATION_SPEED = 360.0f / 3f;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
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
        }

        // 対応する視覚的なホイールを見つけます
        // Transform を正しく適用します
        public void ApplyLocalPositionToVisuals(WheelCollider collider)
        {
            if (collider.transform.childCount == 0) {
                return;
            }
     
            Transform visualWheel = collider.transform.GetChild(0);
     
            Vector3 position;
            Quaternion rotation;
            collider.GetWorldPose(out position, out rotation);
     
            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
        }
     
        public void FixedUpdate()
        {
            float motor = maxMotorTorque * Input.GetAxis("Vertical");
            float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
     
            foreach (AxleInfo axleInfo in axleInfos) {
                if (axleInfo.steering) {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }
                if (axleInfo.motor) {
                    axleInfo.leftWheel.motorTorque = motor;
                    axleInfo.rightWheel.motorTorque = motor;
                }
                ApplyLocalPositionToVisuals(axleInfo.leftWheel);
                ApplyLocalPositionToVisuals(axleInfo.rightWheel);
            }
            _rb.velocity = transform.forward * _cartSpeed;
        }
    }
    
}