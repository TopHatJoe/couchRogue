using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
//serializable attribute is in system
using System.IO;
//open and save files
using System.Runtime.Serialization.Formatters.Binary;
//contains binary formatter

public static class SaveLoadManager
{
	public static void SavePlayer (Player player, string saveFileName)
	{
		BinaryFormatter bf = new BinaryFormatter ();

		//creates file to save a class to

		//had to relocate the datapath due to standalone issues
		//FileStream stream = new FileStream (Application.dataPath + "/Resources/SaveData/CustomShips/" + saveFileName + ".sav", FileMode.Create);
		//Debug.Log ("Data saved to: " + Application.dataPath + "/Resources/SaveData/CustomShips/" + saveFileName + ".sav");
		FileStream stream = new FileStream (Application.persistentDataPath + "/SaveData/" + saveFileName + ".sav", FileMode.Create);
		Debug.Log ("Data saved to: " + Application.persistentDataPath + "/SaveData/" + saveFileName + ".sav");

		PlayerData data = new PlayerData (player);

		bf.Serialize (stream, data);

		stream.Close ();
	}


	public static string LoadShipType (string loadFileName)
	{
		//had to relocate datapath
		//if (File.Exists (Application.dataPath + "/Resources/SaveData/CustomShips/" + loadFileName + ".sav")) 
		if (File.Exists (Application.persistentDataPath + "/SaveData/" + loadFileName + ".sav")) 
		{
			BinaryFormatter bf = new BinaryFormatter ();

			//FileStream stream = new FileStream (Application.dataPath + "/Resources/SaveData/CustomShips/" + loadFileName + ".sav", FileMode.Open);
			FileStream stream = new FileStream (Application.persistentDataPath + "/SaveData/" + loadFileName + ".sav", FileMode.Open);

			PlayerData data = (PlayerData)bf.Deserialize (stream);

			stream.Close ();

			return data.shipType;
		} else {
			Debug.LogError ("FILE DOES NOT EXIST");
			return null;
		}
	}

	//public static int[,] LoadPlayer2DArray (string loadFileName)
	public static string LoadShip (string loadFileName)
	{
		//had to relocate datapath
		//if (File.Exists (Application.dataPath + "/Resources/SaveData/CustomShips/" + loadFileName + ".sav")) 
		if (File.Exists (Application.persistentDataPath + "/SaveData/" + loadFileName + ".sav")) 
		{
			BinaryFormatter bf = new BinaryFormatter ();

			//FileStream stream = new FileStream (Application.dataPath + "/Resources/SaveData/CustomShips/" + loadFileName + ".sav", FileMode.Open);
			FileStream stream = new FileStream (Application.persistentDataPath + "/SaveData/" + loadFileName + ".sav", FileMode.Open);

			PlayerData data = (PlayerData)bf.Deserialize (stream);

			stream.Close ();

			//return data.stats2D;
			return data.shipData;
		} else {
			Debug.LogError ("FILE DOES NOT EXIST");
			return null;
		}
	}
}

[Serializable]
public class PlayerData
{
	public string shipType;

	//public int[,] stats2D;
	public string shipData;

	public PlayerData(Player player)
	{
		int listLength = LevelManager.Instance.parameterList.Count;

		//stats2D = Player.Instance.ListToArray();
		//shipData = Player.Instance.ListToArray();
		shipData = Player.Instance.ListToString(LevelManager.Instance.parameterList);

		shipType = CasheScript.Instance.ShipType;
	}
}