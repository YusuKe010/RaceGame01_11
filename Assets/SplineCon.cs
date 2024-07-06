using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineCon : MonoBehaviour
{
	private Spline _spline;

	private void Start()
	{
		_spline = GetComponent<Spline>();
		Debug.Log(_spline.GetLength());
	}
}
