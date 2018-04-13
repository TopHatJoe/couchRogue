using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour 
{
	private Point gridPos;
	public Point GridPos { get { return gridPos; } }
	private bool couchMode;

	private List <int> accessIndexList = new List <int> ();
	public List <int> AccessIndexList { get { return accessIndexList; } }
	private Dictionary <int, TileScript> accessDict = new Dictionary <int, TileScript> ();



	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			for (int i = 0; i < accessIndexList.Count; i++) {
				Debug.Log ("Pos: " + gridPos.X + ", " + gridPos.Y + ";  port: " +  accessDict[accessIndexList [i]].GridPosition.Y);
			}
		}
	}

	public void ElevatorAccessIni (Point _point, bool _couchMode) {
		gridPos = _point;
		couchMode = _couchMode;

		FindOtherElevators ();
	}

	public void GetElevatorList () {
		
	}

	//finds the other elevator access points
	private void FindOtherElevators () {
		//gridPos.X
		//10 is subject to change

		TileScript _thisTile = LevelManager.Instance.Tiles [gridPos];

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
					ElevatorScript _elevator = _tile.transform.GetChild (6).GetChild (0).GetComponent <ElevatorScript> ();
					_elevator.accessDict.Add (_thisTile.GridPosition.Y, _thisTile);
					_elevator.accessIndexList.Add (_thisTile.GridPosition.Y);
					//Debug.Log (y);
				}
			}
		}
	}


	public void OnTriggerEnter2D (Collider2D _col) {
		//Debug.Log ("im walking here");

		if (_col.GetComponent <CouchCrewScript> () != null) {
			_col.GetComponent <CouchCrewScript> ().IsElevatorNear (true, this);
		}
	}

	public void OnTriggerExit2D (Collider2D _col) {
		//Debug.Log ("are you still there");

		//if (_col.GetComponent <CouchCrewScript> () != null) {
			_col.GetComponent <CouchCrewScript> ().IsElevatorNear (false, null);
		//}
	}
}