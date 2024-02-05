using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.Splines;

namespace Script
{
	public class LapManager : MonoBehaviour
	{
		[SerializeField] private int _lapCount = 3;
		[SerializeField] private Text _lapText;
		[SerializeField] private Text _nowLapText;
		[SerializeField] private Text _timerText;
		[SerializeField] private Ease _lapTimerFade;
		[SerializeField] private Button _button;
		private GameManager _gameManager;
		private SplineAnimate _splineAnimate;
		private float[] _lapTimer;
		private float _raceTimer = 0;
		private int _nowLapCount = 0;
		private bool _isResult = false;

		private void Start()
		{
			_splineAnimate = GetComponent<SplineAnimate>();
			_gameManager = FindObjectOfType<GameManager>();
			Initialization();
		}

		void Initialization()
		{
			_lapTimer = new float[_lapCount + 1];
			_timerText.enabled = true;
			_lapText.enabled = false;
			_isResult = false;
		}

		private void Update()
		{
			switch (_gameManager._gameMode)
			{
				case GameMode.InGame:
					_raceTimer += Time.deltaTime;
					_timerText.text = "Timer:" + _raceTimer.ToString("f3");
					if(_lapCount >= _nowLapCount)
					_lapTimer[_nowLapCount] += Time.deltaTime;
					break;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag("Lap"))
			{
				if (_nowLapCount >= _lapCount && !_isResult)
				{
					_isResult = true;
					_splineAnimate.Play();
					Result();
					_nowLapText.enabled = false;
					_gameManager._gameMode = GameMode.Ranking;
				}
				else if (_nowLapCount != 0 && _nowLapCount < _lapCount)
				{
					LapTimer();
				}
				_nowLapCount++;
				_nowLapText.text = $"{_nowLapCount}/3Lap";
			}
		}

		void LapTimer()
		{
			if (_lapText )
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

		void Result()
		{
			_button.onClick.AddListener(() => SceneChanger.Instance.LoadScene("Title"));
		}
	}
}