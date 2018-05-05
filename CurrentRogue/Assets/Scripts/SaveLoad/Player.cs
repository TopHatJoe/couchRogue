using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Singleton <Player>
{
	//private int tmpPlayerID = 0;

	[SerializeField]
	private GameObject inputTxtS;
	[SerializeField]
	private GameObject inputTxtL;
	[SerializeField]
	private string saveFileName;
	[SerializeField]
	public string loadFileName;

	[SerializeField]
	private string shipType;

	[SerializeField]
	private string shipString;


	//[SerializeField]
	//private TestScript testScript;
	/*
	public int sX = 0;
	public int sY = 1;
	public int sT = 2;
	public int sR = 3;
	public int sW = 4;
	*/

	public void GetSaveFileName ()
	{
		saveFileName = inputTxtS.GetComponent<Text> ().text;

		Debug.Log (saveFileName);
	}

	public void GetLoadFileName ()
	{
		loadFileName = inputTxtL.GetComponent<Text> ().text;

		Debug.Log (loadFileName);
	}

	public void Save ()
	{
		SaveLoadManager.SavePlayer (this, saveFileName);
	}


	public string LoadShipType (string _shipName)
	{
		shipType = SaveLoadManager.LoadShipType (_shipName);

		return shipType;
	}

	//MADNESS

	public string LoadShip (string _shipname) {
		return SaveLoadManager.LoadShip (_shipname);
	}

	/*
	public int [,] LoadShip (string _shipName)
	{
		int[,] loadedStats = SaveLoadManager.LoadShip (_shipName);

		return loadedStats;
	}
	*/

	//actually places the ship
	//public void PlacementLoop (int[,] loadedStats, int playerID)
	public void PlaceShip (string _shipStr, int _playerID)
	{
		string[] _strArr = _shipStr.Split ('-');



		for (int i = 0; i < _strArr.Length; i++) {
			string [] _strArrTwo = _strArr [i].Split (',');

			string _name = _strArrTwo [0];
			int _type = int.Parse (_strArrTwo [1]);
			int _x = int.Parse (_strArrTwo [2]);
			int _y = int.Parse (_strArrTwo [3]);

			Point _point = new Point (_x, _y, _playerID);

			PlaceObject (_name, _type, _point);
		}

		//Debug.Log ("playerID: " + _playerID);

		//the placement loop should be seperated
		//for (int i = 0; i < loadedStats.GetLength(0); i++) {
		//	PlaceObjOnLocation (loadedStats [i, 0], loadedStats [i, 1], playerID, loadedStats [i, 2], loadedStats [i, 3], loadedStats [i, 4]);
		//}
	}

	/*
	private void SplitShipString (string _shipStr)
	{
		string[] _strArr = _shipStr.Split ('-');



		for (int i = 0; i < _strArr.Length; i++) {
			string [] _strArrTwo = _strArr [i].Split (',');

			string _name = _strArrTwo [0];
			int _type = _strArrTwo [1];
			int _x = _strArrTwo [2];
			int _y = _strArrTwo [3];
		}

	}
	*/

	/*
	public int [,] ListToArray (List<string> _list = null)
	{
		List<string> tmpList = LevelManager.Instance.parameterList;

		if (_list != null) {
			tmpList = _list;
		} 

		int[,] data2D = new int[tmpList.Count, 5];

		for (int pL = 0; pL < tmpList.Count; pL++) 
		{
			string tmpData = tmpList [pL];

			string[] tmpStringData = tmpData.Split (',');

			for (int sD = 0; sD < tmpStringData.Length; sD++) 
			{
				data2D [pL, sD] = int.Parse (tmpStringData [sD]);

				Debug.Log (data2D [pL, sD]);
			}
		}
	
		return data2D;
	}
	*/

	public void PlaceObject (string _name, int _type, Point _point) {
		//Point _point = new Point (_x, _y);

		TileScript _tile = LevelManager.Instance.Tiles [_point];

		string _nameCopy = _name;
		string[] _ncArr = _nameCopy.Split ('_');

		_tile.PlaceObject (_type, _ncArr[0], _name);
		//23Room
	}

	/*
	//removed the width oprionality
	public void PlaceObjOnLocation (int x, int y, int z, int type, int reference, int width)
	{
		Point tmp = new Point (x, y, z);
		TileScript tmpPlacement = LevelManager.Instance.Tiles [tmp].GetComponent<TileScript> ();

		if (type == 0) {

			for (int tmpX = 0; tmpX <= width; tmpX++) {
				//Debug.Log ("executed 4");
				Point otherTmp = new Point (x + tmpX, y, z);
				TileScript tmpTileScript = LevelManager.Instance.Tiles [otherTmp].GetComponent<TileScript> ();

				if (!tmpTileScript.IsEmpty) 
				{ break; }

				if (tmpX == width) { 
					//tmpPlacement.PlaceObjectFromScript (type, reference, (width + 1));

					for (int i = 0; i <= width; i++) {
						Point anotherTmp = new Point (((otherTmp.X + i) - width), otherTmp.Y, otherTmp.Z);
						TileScript tmpReaction = LevelManager.Instance.Tiles [anotherTmp].GetComponent<TileScript> ();
						//tmpReaction.tileReactionToRoomPlacement ();
					}
				}
			}
		} else {
			//tmpPlacement.PlaceObjectFromScript (type, reference);
		}
	}
	*/

	//takes int[,] and converts it to string[]
	public string IntArrayToString (int[,] _loadedStats)
	{
		for (int x = 0; x < _loadedStats.GetLength(0); x++) {
			for (int y = 0; y < _loadedStats.GetLength (1); y++) {

				shipString += _loadedStats [x, y].ToString ();
					
				if (y != 4) {
					//splitter y
					shipString += ","; 
				}
			}

			if (x != _loadedStats.GetLength (0) - 1) {
				//splitter x
				shipString += "-";
			}
		}

		Debug.Log (shipString);
		return shipString;
	}


	//Converts parameter list to a single string
	public string ListToString (List <string> _parameterList)
	{
		string _string = "";

		for (int x = 0; x < _parameterList.Count; x++) {
			_string += _parameterList[x];

			//so that there isnt a splitter behind the last entry
			if (x != _parameterList.Count - 1) {
				_string += "-";
			}
			/*
			for (int y = 0; y < _parameterList.Count; y++) {

				shipString += _parameterList [x, y].ToString ();

				if (y != 4) {
					//splitter y
					shipString += ","; 
				}
			}

			if (x != _parameterList.Count - 1) {
				//splitter x
				shipString += "-";
			}
			*/
		}

		Debug.Log (_string);
		return _string;
	}



	//delete later
	public int [,] StringToIntArray (string _loadedShip)
	{
		string [] tmpArray = _loadedShip.Split ('-');

		int [,] data2D = new int[tmpArray.Length, 5];

		for (int x = 0; x < tmpArray.Length; x++) {
			string[] otherArray = tmpArray [x].Split (',');
			//int[] anotherArray = 
			for (int y = 0; y < otherArray.Length; y++) {
				data2D [x, y] = int.Parse (otherArray [y]);
			}
		}

		return data2D;
	}


	public void LoadFromBtn () {
		string _shipString = LoadShip (loadFileName);

		Debug.LogError (_shipString);
		PlaceShip (_shipString, 0);
	}

	//MADNESS
}