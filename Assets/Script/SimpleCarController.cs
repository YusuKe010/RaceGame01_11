﻿using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;
using UnityEngine.Splines;

namespace Script
{
	[Serializable]
	public class AxleInfo
	{
		public WheelCollider leftWheel;
		public WheelCollider rightWheel;
		public bool motor; //このホイールがエンジンにアタッチされているかどうか
		public bool steering; // このホイールがハンドルの角度を反映しているかどうか
	}

	public enum CartStatus
	{
		Load,
		Grass
	}

	public class SimpleCarController : MonoBehaviour
	{
		public List<AxleInfo> axleInfos;
		public float maxMotorTorque;
		public float maxSteeringAngle;
		private Rigidbody _rb;
		[SerializeField] float _cartRotateSpeed = 0.0f;
		[SerializeField] private float _maxCartBrake = 10000.0f;
		[SerializeField] private Ease _ease;
		[SerializeField] private GameObject[] _muffler;
		[SerializeField] private GameObject _grass;
		[SerializeField] private CartStatus _cartStatus = CartStatus.Load;
		[SerializeField] private Transform _startPos;
		private GameManager _gameManager;
		float _cartRotate = 0.0f;
		float _rotationAccel = 360.0f;
		private bool _isresult = false;

		WheelFrictionCurve _wheelFrictionCurve = new WheelFrictionCurve();

		const float MAX_SPEED = 40.0f;
		const float MAX_ROTATION_SPEED = 360.0f / 5f;

		private Tween[] _tweens = new Tween[4];

		private void Start()
		{
			_rb = GetComponent<Rigidbody>();
			_gameManager = FindObjectOfType<GameManager>();
			Initialization();
		}

		void Initialization()
		{
			this.transform.position = _startPos.position;
			_muffler[0].SetActive(true);
			_muffler[1].SetActive(true);
			_grass.SetActive(false);
			foreach (AxleInfo axleInfo in axleInfos)
			{
				axleInfo.leftWheel.brakeTorque = .0f;
				axleInfo.rightWheel.brakeTorque = .0f;
				_wheelFrictionCurve = axleInfo.leftWheel.sidewaysFriction;
			}
		}

		private void Update()
		{
			float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

			switch (_gameManager._gameMode)
			{
				case GameMode.BeforeStart:
					break;
				case GameMode.InGame:
					Accel(steering);
					Carb(steering);
					switch (_cartStatus)
					{
						case CartStatus.Load:
							break;
						case CartStatus.Grass:
							break;
					}

					break;
			}
		}

		// 対応する視覚的なホイールを見つけます
		// Transform を正しく適用します
		public void ApplyLocalPositionToVisuals(WheelCollider collider)
		{
			if (collider.transform.childCount == 0)
			{
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
		}

		private void OnCollisionEnter(Collision other)
		{
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Load"))
			{
				Debug.Log("Load");
				_cartStatus = CartStatus.Load; //CartStatus.Load
				_muffler[0].SetActive(true);
				_muffler[1].SetActive(true);
				_grass.SetActive(false);
			}

			if (other.CompareTag("Grass"))
			{
				Debug.Log("Grass");
				_cartStatus = CartStatus.Grass; //CartStatus.Grass
				_grass.SetActive(true);
			}
		}

		void Accel(float steering)
		{
			foreach (AxleInfo axleInfo in axleInfos)
			{
				if (axleInfo.steering)
				{
					axleInfo.leftWheel.steerAngle = steering;
					axleInfo.rightWheel.steerAngle = steering;
				}

				if (_cartStatus == CartStatus.Grass)
				{
					axleInfo.leftWheel.sidewaysFriction = new WheelFrictionCurve(){stiffness = .5f};
					axleInfo.rightWheel.sidewaysFriction = new WheelFrictionCurve(){stiffness = .5f};
				}
				else
				{
					axleInfo.leftWheel.sidewaysFriction = _wheelFrictionCurve;
					axleInfo.rightWheel.sidewaysFriction = _wheelFrictionCurve;
				}


				if (axleInfo.motor)
				{
					if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
					{
						// _tweens[0].Kill();
						// _tweens[1].Kill();
						// _tweens[0] = DOTween.To(
						// 	() => axleInfo.leftWheel.motorTorque,
						// 	value => axleInfo.leftWheel.motorTorque = value,
						// 	maxMotorTorque, 1.0f).SetEase(Ease.OutQuad);
						// _tweens[1] = DOTween.To(
						// 	() => axleInfo.rightWheel.motorTorque,
						// 	value => axleInfo.rightWheel.motorTorque = value,
						// 	maxMotorTorque, 1.0f).SetEase(Ease.OutQuad);
						axleInfo.leftWheel.motorTorque = maxMotorTorque;
						axleInfo.rightWheel.motorTorque = maxMotorTorque;
					}

					if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
					{
						// 	_tweens[0].Kill();
						// 	_tweens[1].Kill();
						// 	_tweens[0] = DOTween.To(
						// 		() => axleInfo.leftWheel.motorTorque,
						// 		value => axleInfo.leftWheel.motorTorque = value,
						// 		0, 1.5f);
						// 	_tweens[1] = DOTween.To(
						// 		() => axleInfo.rightWheel.motorTorque,
						// 		value => axleInfo.rightWheel.motorTorque = value,
						// 		0, 1.5f);

						axleInfo.leftWheel.motorTorque = 0;
						axleInfo.rightWheel.motorTorque = 0;
					}

					if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
					{
						_tweens[2].Kill();
						_tweens[3].Kill();
						_tweens[2] = DOTween.To(
							() => axleInfo.leftWheel.brakeTorque,
							value => axleInfo.leftWheel.brakeTorque = value,
							_maxCartBrake, 0.0f);
						_tweens[3] = DOTween.To(
							() => axleInfo.rightWheel.brakeTorque,
							value => axleInfo.rightWheel.brakeTorque = value,
							_maxCartBrake, 0.0f);
					}

					if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
					{
						_tweens[2].Kill();
						_tweens[3].Kill();
						_tweens[2] = DOTween.To(
							() => axleInfo.leftWheel.brakeTorque,
							value => axleInfo.leftWheel.brakeTorque = value,
							0.0f, .0f);
						_tweens[3] = DOTween.To(
							() => axleInfo.rightWheel.brakeTorque,
							value => axleInfo.rightWheel.brakeTorque = value,
							0.0f, .0f).OnComplete(() => Debug.Log("aaa"));
					}
				}

				ApplyLocalPositionToVisuals(axleInfo.leftWheel);
				ApplyLocalPositionToVisuals(axleInfo.rightWheel);
			}
		}

		void Carb(float steering)
		{
			if (steering == 0)
			{
				_cartRotateSpeed *= 0.9f;
			}


			_cartRotateSpeed += _rotationAccel * steering * Time.deltaTime;
			_cartRotateSpeed = Mathf.Clamp(_cartRotateSpeed, -MAX_ROTATION_SPEED, MAX_ROTATION_SPEED);
			_cartRotate += _cartRotateSpeed * Time.deltaTime;
			transform.localEulerAngles = new Vector3(0.0f, _cartRotate, 0.0f);
		}
	}
}