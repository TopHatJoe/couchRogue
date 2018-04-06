using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInfo : NetworkBehaviour 
{
	[SerializeField]
	public int PlayerID { get; private set; }

	[SerializeField]
	private string shipName;
	public string ShipName { get { return shipName; } }

	[SerializeField]
	private string shipType = "standard";
	public string ShipType { get { return shipType; } }


	//private int [,] loadedShip;
	//public int [,] LoadedShip { get { return loadedShip; } }
	private string loadedShip;
	public string LoadedShip { get { return loadedShip; } }

	//list of all members of this players' crew
	public List <CrewScript> crewList;

	//private string shipString;



	void Start ()
	{
		NetManager.Instance.playerList.Add (this);

		NetManager.Instance.UpdateID ();

		//if (!isServer) {
		CatchUp ();	


		DontDestroyOnLoad (this);
	}
		

	public void AssignPlayerID (int _ID)
	{
		PlayerID = _ID;
	}

	private void OnDestroy ()
	{
		NetManager.Instance.playerList.Remove (this);

		NetManager.Instance.UpdateID ();
	}


	private void LogMyID ()
	{
		Debug.Log ("My ID: " + PlayerID);
	}


	public void AssignLocalPlayerID ()
	{
		if (isLocalPlayer) {
			NetManager.Instance.localPlayerID = PlayerID;
		}
	}
		

	[ClientRpc]
	public void RpcLoadLocalShip ()
	{
		if (isLocalPlayer) {
			//loads the ship in form of a 2D array

			//tested and safe

			//Disababled for Debug
			loadedShip = Player.Instance.LoadShip (shipName);

			//converts [,] to string  //already is
			//shipString = Player.Instance.IntArrayToString (loadedShip);

			//sends string to server instance of obj
			CmdSendShip (loadedShip);
		}
	}

	[Command]
	private void CmdSendShip (string _shipString)
	{
		//sends string to every instance of obj in network
		RpcDistributeShip (_shipString);
	}

	[ClientRpc]
	private void RpcDistributeShip (string _shipString)
	{
		//shipString = _shipString;
		loadedShip = _shipString;

		//adds the 
		NetManager.Instance.AddToShipStringArray (PlayerID, _shipString);
	}


	public void LoadShipType (string _shipName)
	{
		if (isLocalPlayer) {
			shipName = _shipName;
			//Debug.Log (shipName);

			//disabled for Debug
			shipType = Player.Instance.LoadShipType (shipName);
			//shipType = "standard";

			//Debug.Log (shipType);

			CmdSyncShipType (shipType);
		}
	}


	[Command]
	private void CmdSyncShipType (string _shipType)
	{
		RpcSyncShipType (_shipType);
		//Debug.Log ("CmdTypeSync");
	}

	[ClientRpc]
	private void RpcSyncShipType (string _shipType)
	{
		shipType = _shipType;
		//Debug.Log ("RpcTypeSync");
	}


	//if a player joins when another already chose a ship.
	private void CatchUp ()
	{
		if (isServer) {
			List <PlayerInfo> _playerList = NetManager.Instance.playerList;

			for (int i = 0; i < _playerList.Count; i++) {
				_playerList [i].RpcCatchUp (_playerList [i].shipType);
			}
		}
	}

	[ClientRpc]
	private void RpcCatchUp (string _shipType)
	{
		shipType = _shipType;
	}


	public void Targeting (Vector3 _pos, Vector3 _gunPos, float _angle, int _prob)
	{
		if (isLocalPlayer) {
			if (!isServer) {
				CmdTargeting (_pos, _gunPos, _angle, _prob);
			} else {
				RpcTargeting (_pos, _gunPos, _angle, _prob);
			}
		} else {
			Debug.LogError ("Authority Err: !localPlayer");
		}
	}

	[Command]
	private void CmdTargeting (Vector3 _pos, Vector3 _gunPos, float _angle, int _prob)
	{
		RpcTargeting (_pos, _gunPos, _angle, _prob);
	}

	[ClientRpc]
	private void RpcTargeting (Vector3 _pos, Vector3 _gunPos, float _angle, int _prob)
	{
		//Point _point = new Point (_pos.x, _pos.y, _pos.z);
		//Point _point = new Point (_pos[0], _pos[1], _pos[2]);

		/*
		int[] _intPos = new int[3];
		for (int i = 0; i < _intPos.Length; i++) {
			_intPos [i] = Mathf.RoundToInt (_pos [i]);
		}

		Debug.Log ("RPC: " + _intPos [0] + ", " + _intPos [0] + ", " + _intPos [0]);

		Point _point = new Point (_intPos [0], _intPos [1], _intPos [2]);
		*/

		//TARGETING NEEDS FIXIN

		//LevelManager.Instance.Tiles [_point].PlaceTarget ();

		//LevelManager.Instance.Tiles [_point].PlaceObject (6, "1", "Target");

		Point _point = VectorToPoint (_pos);
		Point _gunPoint = VectorToPoint (_gunPos);

		LevelManager.Instance.Tiles [_point].PlaceTarget (_gunPoint, _angle);
	}

	private Point VectorToPoint (Vector3 _vect) {
		int[] _intPos = new int[3];
		for (int i = 0; i < _intPos.Length; i++) {
			_intPos [i] = Mathf.RoundToInt (_vect [i]);
		}

		Point _point = new Point (_intPos [0], _intPos [1], _intPos [2]);
		//Debug.LogError ("RPC: " + _intPos [0] + ", " + _intPos [1] + ", " + _intPos [2]);

		return _point;
	}


	public void SyncDoorstate (Vector3 _pos, bool _doorState)
	{
		if (isLocalPlayer) {
			if (!isServer) {
				CmdSyncDoorstate (_pos, _doorState);
			} else {
				RpcSyncDoorstate (_pos, _doorState);
			}
		} else {
			Debug.Log ("Authority Err: !localPlayer");
		}
	}

	[Command]
	private void CmdSyncDoorstate (Vector3 _pos, bool _doorState)
	{
		RpcSyncDoorstate (_pos, _doorState);
	}

	[ClientRpc]
	private void RpcSyncDoorstate (Vector3 _pos, bool _doorState)
	{
		//Point _point = new Point (_pos.x, _pos.y, _pos.z);
		//Point _point = new Point (_pos[0], _pos[1], _pos[2]);
		/*
		int[] _intPos = new int[3];
		for (int i = 0; i < _intPos.Length; i++) {
			_intPos [i] = Mathf.RoundToInt (_pos [i]);
		}
		*/

		//Point _point = new Point (_intPos [0], _intPos [1], _intPos [2]);
		Point _point = VectorToPoint (_pos);

		GameObject _door = LevelManager.Instance.Tiles [_point].transform.GetChild (3).GetChild (0).gameObject;
		_door.GetComponent <DoorScript> ().ChangeDoorstate (_doorState);
	}


	public void SetCrewIndex () {
		//Debug.Log ("index");

		for (int i = 0; i < crewList.Count; i++) {
			crewList [i].SetCrewIndex (i);
		}
	}


	public void SyncMovement (Vector3 _start, Vector3 _goal, int _crewIndex)
	{
		if (isServer) {
			RpcSyncMovement (_start, _goal, _crewIndex);
		} else {
			CmdSyncMovement (_start, _goal, _crewIndex);
		}
	}

	[Command]
	private void CmdSyncMovement (Vector3 _start, Vector3 _goal, int _crewIndex)
	{
		RpcSyncMovement (_start, _goal, _crewIndex);
	}

	[ClientRpc]
	private void RpcSyncMovement (Vector3 _start, Vector3 _goal, int _crewIndex) 
	{
		CrewScript _crew = crewList [_crewIndex];
		_crew.UndoChanges (); //SyncedMovement (_start, _goal);
	}




	public void SyncPowerstate (Vector3 _pos, int _type, bool _isPowered) {
		if (isServer) {
			RpcSyncPowerstate (_pos, _type, _isPowered);
		} else {
			CmdSyncPowerstate (_pos, _type, _isPowered);
		}
	}

	[Command]
	private void CmdSyncPowerstate (Vector3 _pos, int _type, bool _isPowered) {
		RpcSyncPowerstate (_pos, _type, _isPowered);
	}

	[ClientRpc]
	private void RpcSyncPowerstate (Vector3 _pos, int _type, bool _isPowered) {
		Point _point = VectorToPoint (_pos);
		TileScript _tile = LevelManager.Instance.Tiles [_point];


		//weapons
		if (_type == 1) {
			//WeaponScript _weapon = LevelManager.Instance.Tiles [_point].transform.GetChild (0).GetComponent <WeaponScript> ();
			WeaponScript _weapon = _tile.transform.GetChild (0).GetComponent <WeaponScript> ();
			//should perhaps be done by function
			//_weapon.IsPowered = _isPowered;
			_weapon.FirePower (_isPowered);
		
		//engines and other systems? //-> interface!
		} else if (_type == 2) {
			EngineScript _engine = _tile.transform.GetChild (1).GetChild (0).GetComponent <EngineScript> ();
			_engine.SyncedPower (_isPowered);
		}
	}


	/*
	private Point VectToPoint (Vector3 _pos) {
		int[] _intPos = new int[3];
		for (int i = 0; i < _intPos.Length; i++) {
			_intPos [i] = Mathf.RoundToInt (_pos [i]);
		}

		return new Point (_intPos [0], _intPos [1], _intPos [2]);
	}
	*/

	public void DebugTest ()
	{
		
	}

	public void SyncShieldPower (int _power, int _ID) {
		if (isServer) {
			RpcSyncShieldPower (_power, _ID);
		} else {
			CmdSyncShieldPower (_power, _ID);
		}
	}

	[Command]
	private void CmdSyncShieldPower (int _power, int _ID) {
		RpcSyncShieldPower (_power, _ID);
	}

	[ClientRpc]
	private void RpcSyncShieldPower (int _power, int _ID) {
		//Debug.LogError ("syncing Shield! " + _power + ", " + _ID);

		if (LevelManager.Instance.Ships [_ID].transform.GetChild (6).gameObject != null) {
			GameObject _shieldObj = LevelManager.Instance.Ships [_ID].transform.GetChild (6).gameObject;
			//_shieldObj.GetComponent <ShieldScript> ().Power = _power;
			_shieldObj.GetComponent <ShieldScript> ().RemoteShield (_power);
		}
	}


	//probability Strings
	public void SyncProbStr (Vector3 _pos, string _probStr) {
		if (isServer) {
			RpcSyncProbStr (_pos, _probStr);
		} else {
			CmdSyncProbStr (_pos, _probStr);
		}
	}

	[Command]
	private void CmdSyncProbStr (Vector3 _pos, string _probStr) {
		RpcSyncProbStr (_pos, _probStr);
	}

	[ClientRpc]
	private void RpcSyncProbStr (Vector3 _pos, string _probStr) {
		Point _point = VectorToPoint (_pos);
		TileScript _tile = LevelManager.Instance.Tiles [_point];
		WeaponScript _weapon = _tile.transform.GetChild (0).GetComponent <WeaponScript> ();
		_weapon.ReceiveProbStr (_probStr);
	}
}