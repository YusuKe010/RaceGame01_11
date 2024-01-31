using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Script
{
	public class LapManager : MonoBehaviour
	{
		[SerializeField] private int _lapCount = 3;
		[SerializeField] private Text _lapText;
		[SerializeField] private Text _timerText;
		[SerializeField] private Ease _lapTimerFade;
		private GameManager _gameManager;
		private float[] _lapTimer;
		private float _raceTimer = 0;
		private int _nowLapCount = 0;

		private void Start()
		{
			_gameManager = FindObjectOfType<GameManager>();
			Initialization();
		}

		void Initialization()
		{
			_lapTimer = new float[_lapCount];
			_timerText.enabled = true;
			_lapText.enabled = false;
		}

		private void Update()
		{
			switch (_gameManager._gameMode)
			{
				case GameMode.InGame:
					_raceTimer += Time.deltaTime;
					_timerText.text = "Timer:" + _raceTimer.ToString("f3");
					_lapTimer[_nowLapCount] += Time.deltaTime;
					break;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Lap"))
			{
				LapTimer();
				_nowLapCount++;
				if (_nowLapCount >= _lapCount)
				{
					GameManager.Instance._gameMode = GameMode.Ranking;
				}
			}
		}

		void LapTimer()
		{
			if (_lapText)
			{
				_lapText.enabled = true;
				_lapText.text = "Lap : " + _lapTimer[_nowLapCount].ToString("f3");
				Vector3 v = _lapText.rectTransform.position;
				_lapText.DOFade(1.0f, 0.0f);
				var sequence = DOTween.Sequence();
				sequence.Join(_lapText.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -100), 1.0f));
				sequence.Join(_lapText.DOFade(0.0f, 1.0f).SetEase(_lapTimerFade));
				sequence.OnComplete(() =>
				{
					_lapText.enabled = false;
					_lapText.rectTransform.position = v;
				});
			}
			else
			{
				Debug.Log("ラップテキストが割り当てられていない");
			}
		}
	}
}