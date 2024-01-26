using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
	private int _rank = 1;

	private void Start()
	{
		
	}

	void Initialization()
	{
		_rank = 1;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") ) 
		{
			
		}
	}
}
