using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public struct UniData
{
	public string unitName;
	public string jobName;
	public int level;
}

[Serializable]
public struct PartyData
{
	public List<UniData> partyMenber;
}

public class JsonTesting : MonoBehaviour
{
	private void Start()
	{
		UniData uniData = new();
		uniData.unitName = "たろう";
		uniData.jobName = "ゆうしゃ";
		uniData.level = 1;
		
		UniData uniData2 = new UniData();
		uniData2.unitName = "じろう";
		uniData2.jobName = "せんし";
		uniData2.level = 3;

		PartyData partyData = new();
		partyData.partyMenber = new();
		partyData.partyMenber.Add(uniData);
		partyData.partyMenber.Add(uniData2);

		string jsonData = JsonUtility.ToJson(partyData.partyMenber[1]);
		//UniData unitData = JsonUtility.FromJson<UniData>(jsonData);
		//Debug.Log(unitData.unitName);

		StreamWriter writer = new StreamWriter(Application.dataPath + "/data.json");
		writer.WriteLine(jsonData);
		writer.Flush();
		writer.Close();
		Debug.Log(Application.dataPath + "/data.json");
	}
}
