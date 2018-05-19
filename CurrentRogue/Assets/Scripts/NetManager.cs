using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetManager : Singleton <NetManager> 
{
	//[SerializeField]
	//private GameObject[] shipObjs;

	[SerializeField]
	private NetworkManager networkMngr;

	private int numOfPlayers;
	public int NumOfPlayers { get { return numOfPlayers; } }

	[SerializeField]
	private int tmpPlayerID = 0;

	[SerializeField]
	public int localPlayerID;
	//public int LocalPlayerID { get { return localPlayerID; } private set { localPlayerID = value; } }

	[SerializeField]
	public List <PlayerInfo> playerList;
	//public Dictionary <NetworkHash128, CrewScript> crewDict = new Dictionary<NetworkHash128, CrewScript> ();

	//a diectionary of connections and objects
	//[SerializeField]
	//public Dictionary <NetworkConnection, GameObject> objByConn;
	private Dictionary <int, NetworkConnection> connDict = new Dictionary <int, NetworkConnection> ();
	public Dictionary <int, NetworkConnection> ConnDict { get { return connDict; } }


	[SerializeField]
	public string[] shipStrings;

	//private int[] [,] ships;
	//private int [,] tmpShip;
	//private List <int[,]> ships;


	//private Dictionary <string, int[,]> shipDict;

	//stupid
	/*
	private string[] ship0;
	private string[] ship1;
	private string[] ship2;
	private string[] ship3;
	private string[] ship4;
	private string[] ship5;
	private string[] ship6;
	private string[] ship7;
	*/

	/*
	private int [,] ship0;
	private int [,] ship1;
	private int [,] ship2;
	private int [,] ship3;
	private int [,] ship4;
	private int [,] ship5;
	private int [,] ship6;
	private int [,] ship7;
	*/


	private int counter = 0;

	[SerializeField]
	private NetLieutenant netLieutenant;

	//public int counter = 0;
	//public int Counter { get { return Counter; } set { Counter = value; } }
	//[SyncVar]
	//public int Counter { get { return syncManager.counter; } set { syncManager.counter = value; Debug.Log ("Counter: " + syncManager.counter); } }
	//[SerializeField]
	//[SyncVar]
	//public int [][,] ShipArray { get { return syncManager.shipArray; } set { syncManager.shipArray = value; } }
	//public int [][,] ShipArray { get { return ShipArray; } set { ShipArray = value; } }
	//public string[] ShipArray { get { return syncManager.shipArray; } set { syncManager.shipArray = value; } }

	//public int [][,] shipArray;
	//[SerializeField]
	//public SyncManager syncManager;


	public void LogConnectionsByBtn ()
	{
		for (int i = 0; i < playerList.Count; i++) {
			//Debug.Log ("PlayerID: " + playerList [i].PlayerID);
		}
			
		//Debug.Log ("localPlayer: " + localPlayerID);
	}


	public void UpdateID ()
	{
		for (int i = 0; i < playerList.Count; i++) {
			//Debug.Log ("updated playerID from: " + playerList [i].PlayerID);
			playerList [i].AssignPlayerID (i);
			//Debug.Log ("to: " + playerList [i].PlayerID);

			//probably the cause of the shipNull bug...
			if (playerList [i].isLocalPlayer) {
				localPlayerID = playerList [i].PlayerID;
			}
		}
	}


	public void GameIni ()
	{
		netLieutenant.RpcIni ();
	}

	public void InitializeGame ()
	{
		Debug.Log ("initializing...");
		AssignPlayerID ();
		GetLocalPLayerID ();
		Declarations ();

		//get conns
		GetConns ();
		//CheckForMissingStrings ();
		//DeclareShipArray ();
		//LevelManager.Instance.Setup ();
	}

	private void AssignPlayerID ()
	{
		for (int i = 0; i < playerList.Count; i++) {
			//could probably just use i
			playerList [i].AssignPlayerID (tmpPlayerID);
			tmpPlayerID++;
		}
	}

	private void GetLocalPLayerID ()
	{
		for (int i = 0; i < playerList.Count; i++) {
			playerList [i].AssignLocalPlayerID ();
		}
	}

	public void LoadShips ()
	{
		netLieutenant.LoadShipsLoop ();
	}

	private void Declarations ()
	{
		numOfPlayers = playerList.Count;

		shipStrings = new string [playerList.Count];

		/*
		for (int i = 0; i < 8; i++) {

			if (i == 0) {
				shipDict.Add (("ship" + i), ship0);
			} else if (i == 1) {
				shipDict.Add (("ship" + i), ship1);
			} else if (i == 2) {
				shipDict.Add (("ship" + i), ship2);
			} else if (i == 3) {
				shipDict.Add (("ship" + i), ship3);
			} else if (i == 4) {
				shipDict.Add (("ship" + i), ship4);
			} else if (i == 5) {
				shipDict.Add (("ship" + i), ship5);
			} else if (i == 6) {
				shipDict.Add (("ship" + i), ship6);
			} else if (i == 7) {
				shipDict.Add (("ship" + i), ship7);
			}
		}
		*/
	}




	private void GetConns () {
		for (int i = 0; i < playerList.Count; i++) {
			connDict.Add (i, playerList [i].connectionToClient);
			//Debug.LogError ("Conn: " + i + ", " + playerList [i].connectionToClient);
		}
	}




	/*
	private void CheckForMissingStrings ()
	{
		for (int i = 0; i < playerList.Count; i++) {
			if (playerList [i].ShipName == null) {
				Debug.LogError ("Not All Ships Received!");
			}
		}
	}
	*/


	//in this context id means the id of the submitter
	public void AddToShipStringArray (int _playerID , string _shipString)
	{
		shipStrings [_playerID] = _shipString;

		counter++;

		//once all strings have been submitted, they are converted
		//if (counter == playerList.Count) {
		if (counter == NumOfPlayers) {
			//resets counter
			counter = 0;

			Debug.Log ("all strings received!");

			//convert strings to [,]
			//ConverterLoop ();

			PlacementLoop ();

		} else if (counter > NumOfPlayers) {
			Debug.LogError ("TOO MANY SHIP STRINGS!");
		} else if (counter < NumOfPlayers) {
			Debug.Log ("still missing ships");
		}
	}

	/*
	private void ConverterLoop ()
	{
		for (int i = 0; i < NumOfPlayers; i++) {
			//tmpShip = ConvertToDArray (shipStrings [i]);
			//ships.Add (tmpShip);

			//ship0 = ConvertToDArray (shipStrings [i]);

			//more stupid bruteforce
			if (i == 0) {
				//ship0 = ConvertToDArray (shipStrings [i]);
				ship0 = ConvertToDArray (shipStrings [i]);
			} else if (i == 1) {
				ship1 = ConvertToDArray (shipStrings [i]);
			} else if (i == 2) {
				ship2 = ConvertToDArray (shipStrings [i]);
			} else if (i == 3) {
				ship3 = ConvertToDArray (shipStrings [i]);
			} else if (i == 4) {
				ship4 = ConvertToDArray (shipStrings [i]);
			} else if (i == 5) {
				ship5 = ConvertToDArray (shipStrings [i]);
			} else if (i == 6) {
				ship6 = ConvertToDArray (shipStrings [i]);
			} else if (i == 7) {
				ship7 = ConvertToDArray (shipStrings [i]);
			}

		}
	}
	*/

	//MADNESS

	/*
	private void SplitShipString (string _shipStr)
	{
		string[] _strArr = _shipStr.Split ('-');



		for (int i = 0; i < _strArr.Length; i++) {
			string [] _strArrTwo = _strArr [i].Split (',');

			string _name = _strArrTwo [0];
			//int _type = _strArrTwo [1];
			//int _x = _strArrTwo [2];
			//int _y = _strArrTwo [3];
		}

	}
	*/

	/*
	private int [,] ConvertToDArray (string _shipString)
	{
		string[] tmpArray = _shipString.Split ('-');

		int[,] data2D = new int[tmpArray.Length, 5];

		for (int x = 0; x < tmpArray.Length; x++) {
			//tmpString = tmpArray [x]
			//string [] otherTmpArray = tmpArray[x].Split ('-');
			string [] otherTmpArray = tmpArray[x].Split (',');

			for (int y = 0; y < otherTmpArray.Length; y++) {
				data2D [x, y] = int.Parse (otherTmpArray [y]);

				//Debug.Log (data2D [x, y]);
			}
		}

		return data2D;

		/*
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
		//*
	}
	*/

	private void PlacementLoop ()
	{
		for (int i = 0; i < NumOfPlayers; i++) {
			//Player.Instance.PlacementLoop (ship0 , i);
			//Player.Instance.PlacementLoop (shipDict["ship" + i] , i);
			Player.Instance.PlaceShip (shipStrings [i], i);

			/*
			//EVEN MOR STUPID BRUTE SHIT
			if (i == 0) {
				Player.Instance.PlacementLoop (ship0 , i);
				//Player.Instance.PlacementLoop (ship0 , i);
			} else if (i == 1) {
				Player.Instance.PlacementLoop (ship1 , i);
			} else if (i == 2) {
				Player.Instance.PlacementLoop (ship2 , i);
			} else if (i == 3) {
				Player.Instance.PlacementLoop (ship3 , i);
			} else if (i == 4) {
				Player.Instance.PlacementLoop (ship4 , i);
			} else if (i == 5) {
				Player.Instance.PlacementLoop (ship5 , i);
			} else if (i == 6) {
				Player.Instance.PlacementLoop (ship6 , i);
			} else if (i == 7) {
				Player.Instance.PlacementLoop (ship7 , i);
			}
			*/
		}
	}


	//Gets the ship name from player and loads the type
	public void GetShipType (Text _inputText)
	{
		//playerList [localPlayerID].ShipName = _shipName;
		//playerList [localPlayerID].GetShipType = _inputText.text;
		//Debug.Log (localPlayerID);
		playerList [localPlayerID].LoadShipType (_inputText.text);

		//Debug.Log (playerList [localPlayerID].ShipName);

		//should be synced here? //nah is synced within loadShipType ()
	}


	public void Targeting (Vector3 _pos, Vector3 _gunPos, float _angle, int _prob)
	{
			playerList [localPlayerID].Targeting (_pos, _gunPos, _angle, _prob);
	}

	public void SyncDoorstate (Vector3 _pos, bool _doorstate)
	{
		playerList [localPlayerID].SyncDoorstate (_pos, _doorstate);
	}



	
	
	//public void SyncPowerState (Point _point, int _type, bool _isPowered)
	public void SyncPowerState (Point _point, bool _isPowered)
	{
		Vector3 _pos = PointToVector (_point);
		playerList [localPlayerID].SyncPowerstate (_pos, _isPowered);
	}

	//ill just use the first bool as _isDamaged for now.
	public void SyncSysHealth (Point _point, bool _isFullyDamaged, bool _isFullyRepaired, int _type)
	{
		Vector3 _pos = PointToVector (_point);
			playerList [localPlayerID].SyncSysHealthState (_pos, _isFullyDamaged, _isFullyRepaired, _type);
	}



	
	private Vector3 PointToVector (Point _point) {
		return new Vector3 (_point.X, _point.Y, _point.Z);
	}
			
	
	/*
	public void SyncShield (int _power, int _ID) {
		playerList [localPlayerID].SyncShieldPower (_power, _ID);
	}
	*/
	
	public void SyncProbabilityString (Vector3 _pos, string _probStr) {
		playerList [localPlayerID].SyncProbStr (_pos, _probStr);
	}

		public void SpawnCrewCmd (Point _pos, string _objStr) {
		Vector3 _posVect = PointToVector (_pos);

		playerList [localPlayerID].SpawnCouchCrew (_posVect, _objStr);
	}

	
	public void OnPlayerConnected (NetworkPlayer _player) {
		Debug.Log ("playerIP: " + _player.ipAddress);
	}




	/*
	public void AddToConnDict (int _playerID, NetworkConnection _conn) {
		connDict.Add (_playerID, _conn);
		Debug.LogError ("Conn: " + _playerID + ", " + _conn);
	}
	*/
	
	/*
	public void SetCrewIndex () {
		Debug.Log ("crew");

		foreach (PlayerInfo _player in playerList) {
			_player.SetCrewIndex ();
		}
	}
	*/

	//public void SyncMovement ()
}