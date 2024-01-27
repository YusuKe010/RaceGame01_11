using UnityEngine;
using System.Collections.Generic;
using System;
using DG.Tweening;

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

	public class SimpleCarController : MonoBehaviour
	{
		public List<AxleInfo> axleInfos;
		public float maxMotorTorque;
		public float maxSteeringAngle;
		private Rigidbody _rb;
		[SerializeField] float _cartRotateSpeed = 0.0f;
		[SerializeField] private float _maxCartBrake = 10000.0f;
		[SerializeField] private Ease _ease;
		float _cartRotate = 0.0f;
		float _rotationAccel = 360.0f;

		const float MAX_SPEED = 40.0f;
		const float MAX_ROTATION_SPEED = 360.0f / 3f;

		private Tween[] _tweens = new Tween[4];

		private void Start()
		{
			_rb = GetComponent<Rigidbody>();
			foreach (AxleInfo axleInfo in axleInfos)
			{
				axleInfo.leftWheel.brakeTorque = .0f;
				axleInfo.rightWheel.brakeTorque = .0f;
			}
		}

		private void Update()
		{
			float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
			foreach (AxleInfo axleInfo in axleInfos)
			{
				if (axleInfo.steering)
				{
					axleInfo.leftWheel.steerAngle = steering;
					axleInfo.rightWheel.steerAngle = steering;
				}

				if (axleInfo.motor)
				{
					if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
					{
						_tweens[0].Kill();
						_tweens[1].Kill();
						_tweens[0] = DOTween.To(
							() => axleInfo.leftWheel.motorTorque,
							value => axleInfo.leftWheel.motorTorque = value,
							maxMotorTorque, 3.0f).OnComplete(() => Debug.Log("加速終わり"));
						_tweens[1] = DOTween.To(
							() => axleInfo.rightWheel.motorTorque,
							value => axleInfo.rightWheel.motorTorque = value,
							maxMotorTorque, 3.0f);
					}

					if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
					{
						_tweens[0].Kill();
						_tweens[1].Kill();
						_tweens[0] = DOTween.To(
							() => axleInfo.leftWheel.motorTorque,
							value => axleInfo.leftWheel.motorTorque = value,
							0, 1.5f).OnComplete(() => Debug.Log("トルク０"));
						_tweens[1] = DOTween.To(
							() => axleInfo.rightWheel.motorTorque,
							value => axleInfo.rightWheel.motorTorque = value,
							0, 1.5f);
					}

					if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
					{
						_tweens[2].Kill();
						_tweens[3].Kill();
						_tweens[2] = DOTween.To(
							() => axleInfo.leftWheel.brakeTorque,
							value => axleInfo.leftWheel.brakeTorque = value,
							_maxCartBrake, .0f);
						_tweens[3] = DOTween.To(
							() => axleInfo.rightWheel.brakeTorque,
							value => axleInfo.rightWheel.brakeTorque = value,
							_maxCartBrake, .0f).OnComplete(() => Debug.Log("止まった"));
					}

					if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
					{
						_tweens[2].Kill();
						_tweens[3].Kill();
						_tweens[2] = DOTween.To(
							() => axleInfo.leftWheel.brakeTorque,
							value => axleInfo.leftWheel.brakeTorque = value,
							0.0f, 1.0f).OnComplete(() => Debug.Log("走れ"));
						_tweens[3] = DOTween.To(
							() => axleInfo.rightWheel.brakeTorque,
							value => axleInfo.rightWheel.brakeTorque = value,
							0.0f, 1.0f);
					}
				}

				ApplyLocalPositionToVisuals(axleInfo.leftWheel);
				ApplyLocalPositionToVisuals(axleInfo.rightWheel);
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
	}
}