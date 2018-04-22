using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessorScript : MonoBehaviour, IPlacable
{
	//the parent tile
	private TileScript tile;
	//the gridPos
	private Point gridPos;

	[SerializeField]
	private string objStr;

	private string saveStr;

	[SerializeField]
	private ElevatorScript elevator;

	//private List <int> accessIndexList = new List <int> ();
	//private Dictionary <int, TileScript> accessDict = new Dictionary <int, TileScript> ();

	private bool couchMode = false;

	/*
	//DEBUG
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			for (int i = 0; i < accessIndexList.Count; i++) {
				Debug.Log ("Pos: " + gridPos.X + ", " + gridPos.Y + ";  port: " +  accessDict[accessIndexList [i]].GridPosition.Y);
			}
		}
	}
	*/

	public void PlaceObj (int _index, Point _gridPos, GameObject _originObj) {
		gridPos = _gridPos;
		tile = LevelManager.Instance.Tiles [gridPos];

		//if (this.gameObject == originObj) {
		saveStr = (objStr + ",3," + gridPos.X.ToString () + "," + gridPos.Y.ToString ());
		LevelManager.Instance.parameterList.Add (saveStr);
		//}

		transform.SetParent (tile.transform.GetChild (6));

		tile.SubSysPlacable = false;
		tile.HasAccessor = true;

		if (CasheScript.Instance.CouchMode) {
			couchMode = true;

			//FindOtherElevators ();
		}

		if (elevator != null) {
			//DEBUG
			tile.HasElevator = true;
			elevator.ElevatorAccessIni (gridPos, couchMode);
		}

		GameManager.Instance.Buy ();
	}

	public void RemoveObj () {
		tile.SubSysPlacable = true;
		tile.HasAccessor = false;

		LevelManager.Instance.parameterList.Remove (saveStr);

		if (elevator != null) {
			//DEBUG
			tile.HasElevator = false;
		}

		Destroy (gameObject);
	}


	/*
	public void UpdateHealth (int _amount) {
		Debug.Log (_amount);

		if (elevator != null) {
			//needs to also be applied to nodes
			if (_amount <= 0) {
				tile.HasElevator = false;
			} else {
				tile.HasElevator = true;
			}

			Debug.Log ("hasElevator: " + tile.HasElevator);
		}
	}
	*/

	public void UpdateHealthState (bool _isFullyDamaged, bool _isFullyRepaired) {
	
	}


	public GameObject GetOriginObj () {
		return gameObject;
	}

	/*
	//finds the other elevator access points
	private void FindOtherElevators () {
		//gridPos.X
		//10 is subject to change
		for (int _y = 0; _y < 10; _y++) {
			Point _point = new Point (gridPos.X, _y, gridPos.Z);

			if (!LevelManager.Instance.InBounds(_point)) { 
				break; 
			}

			TileScript _tile = LevelManager.Instance.Tiles [_point];
			if (_tile.HasElevator) {
				accessDict.Add (_y, _tile);
				accessIndexList.Add (_y);

				if (_y != gridPos.Y) {
					AccessorScript _accessor = _tile.transform.GetChild (6).GetChild (0).GetComponent <AccessorScript> ();
					_accessor.accessDict.Add (_y, _tile);
					_accessor.accessIndexList.Add (_y);
					//Debug.Log (y);
				}
			}
		}
	}

	public void RemoteAddElevator (int _level, Point _point) {
		
	}
	*/
}