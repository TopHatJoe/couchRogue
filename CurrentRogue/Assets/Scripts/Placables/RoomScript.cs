using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour, IPlacable
{
	private int roomID;

	private bool isOrigin = false;
	public bool IsOrigin { get { return isOrigin; } }

	[SerializeField]
	private Sprite roomSprite;
	[SerializeField]
	private Sprite dmgSprite;

	private SpriteRenderer sprRenderer;
	private BoxCollider2D thisCol;

	//number of damaged objects in room (including room itself)
	private int damages = 0;


	[SerializeField]
	//is there a next component to this obj?
	private bool last;
	[SerializeField]
	//is the component a border component?
	private bool isLeftBorder;
	[SerializeField]
	//and where?
	private bool isRightBorder;
	[SerializeField]
	//can a door be placed?
	private bool HasDoorSlot;

	[SerializeField]
	//next component to be placed
	private GameObject nextObj;
	//the instance of the obj placed by this instance
	private GameObject thisNextObj;
	//the parent tile
	private TileScript tile;
	//the gridPos
	private Point gridPos;

	//the first obj to be placed, and removed
	private GameObject originObj;
	public GameObject OriginObj { get { return originObj; } }
	//only defined on originObj, the tile, that was clicked to place this room
	private Point _trueOriginPos;

	[SerializeField]
	//the dimensions of the obj
	private string objDimensions;

	[SerializeField]
	private string objStr;


	[SerializeField]
	//two ints defining relative gridPos for nextObj
	private Vector2 nextPos;

	private string saveStr;

	//progress of the repairs... //updated by crew
	private int repairProgress = 100;
	/*
	private int RepairProgress { 
		get { 
			return repairProgress; 
		} set { 
			repairProgress = value; 
			if (isOrigin) {
				UpdateRepairBar (repairProgress / 100);
			}
		} 
	}
	*/


	//list of all systems, subsystems and other placables except for everything that moves...
	private List <GameObject> roomContents = new List <GameObject> ();

	//private GameObject healthBarBase;
	[SerializeField]
	private GameObject healthBar;
	private HealthScript hScr;

	private bool isFullyDamaged = false;
	public bool IsFullyDamaged { get { return originObj.GetComponent <RoomScript> ().isFullyDamaged; } }
	private bool isFullyRepaired = true;
	public bool IsFullyRepaired { get { return originObj.GetComponent <RoomScript> ().isFullyRepaired; } }


	public void PlaceObj (int _index, Point _gridPos, GameObject _originObj) {
		thisCol = gameObject.GetComponent <BoxCollider2D> ();
		if (CasheScript.Instance.GameMode == 0) {
			thisCol.enabled = false;
		} else {
			thisCol.enabled = true;
		}

		originObj = _originObj;


		if (this.gameObject == originObj) {
			isOrigin = true;

			_trueOriginPos = _gridPos;

			saveStr = (objStr + ",0," + _gridPos.X.ToString () + "," + _gridPos.Y.ToString ());
			LevelManager.Instance.parameterList.Add (saveStr);

			//adds the origin to the roomList for ICR (InitialContentReferencing)
			LevelManager.Instance.roomList.Add (this);

			//healthbar!
			//healthBarBase = healthBar.transform.parent.gameObject;
		} 

		//gridPos = _gridPos;
		gridPos = new Point (_gridPos.X + Mathf.RoundToInt (nextPos.x * 2), _gridPos.Y + Mathf.RoundToInt (nextPos.y), _gridPos.Z);
		Debug.Log ("gridPos: " + gridPos.X + ", " + gridPos.Y + ", " + gridPos.Z);


		//set tweens walkable
		/*
		if (true) {
			Point _point = new Point (gridPos.X + 1, gridPos.Y, gridPos.Z);
			LevelManager.Instance.Tiles [_point].Walkable = true;
		}
		*/
	
		tile = LevelManager.Instance.Tiles [gridPos]; //-> ye fucktard! that's what's caused the issue shithead! //?
		hScr = gameObject.GetComponent <HealthScript> ();
		//transform.parent.parent.GetComponent <TileScript> ();

		tile.HScrDict.Add (0, hScr);


		/*
		//important for dmg
		if (this.gameObject != originObj) {
			tile.LocalObjDict.Add (0, gameObject.GetComponent <HealthScript> ());
		}
		*/

		transform.position = tile.transform.position;
		//Quaternion.identity = tile.Quaternion.identity;

		transform.SetParent (tile.transform.GetChild (0));

		if (HasDoorSlot) {
			if (isLeftBorder) {
				Point _point = new Point (gridPos.X - 2, gridPos.Y, gridPos.Z);
				TileScript _neighbourTile = LevelManager.Instance.Tiles [_point];
				if (!_neighbourTile.IsEmpty && _neighbourTile.HasDoorSlot) {
				//if (!tile.IsEmpty && tile.HasDoorSlot) {
					//place door
					//Debug.Log ("doorPLaced left");
					PlaceDoor (tile);
				}
			} 

			if (isRightBorder) {
				Point _point = new Point (gridPos.X + 2, gridPos.Y, gridPos.Z);
				TileScript _neighbourTile = LevelManager.Instance.Tiles [_point];
				if (!_neighbourTile.IsEmpty && _neighbourTile.HasDoorSlot) {
					//place door
					//Debug.Log ("doorPlaced Right");
					PlaceDoor (_neighbourTile);
				}
			}
		}

		//Debug.Log ("yaya");
		DoBool ();

		GameManager.Instance.Buy ();


		if (!last) {
			//creates next component of obj

			//combinig nextPos with point to get the true next gridPos
			//Point _point = new Point (_gridPos.X + Mathf.RoundToInt (nextPos.x * 2), _gridPos.Y + Mathf.RoundToInt (nextPos.y), _gridPos.Z);
			//TileScript _tile = LevelManager.Instance.Tiles [_point];

			thisNextObj = (GameObject)Instantiate (nextObj);

			IPlacable _placable = thisNextObj.GetComponent <IPlacable> ();
			_placable.PlaceObj (_index, gridPos, originObj);

			//Debug.Log ("nextHScr set!");
			hScr.NextHScr = thisNextObj.GetComponent <HealthScript> ();
		}

		sprRenderer = gameObject.GetComponent <SpriteRenderer> ();
	}

	private void DoBool () {
		//Debug.Log ("Doing Bools on: " + tile.GridPosition.X + ", " + tile.GridPosition.Y);
		tile.IsEmpty = false;
		tile.Walkable = true;
		tile.SystemPlacable = true;

		tile.Manned = false;
		tile.IsDestination = false;

		tile.HasElevator = false;

		tile.HasDoorSlot = HasDoorSlot;
	}

	private void UndoBool () {
		tile.IsEmpty = true;
		tile.Walkable = false;
		tile.SystemPlacable = false;
		tile.Manned = true;
		tile.IsDestination = false;
		tile.HasElevator = false;
		tile.HasDoorSlot = false;
	}


	private void PlaceDoor (TileScript _ts)
	{
		//GameObject door = (GameObject)Instantiate (LevelManager.Instance.ObjList [7], _ts.transform.position, Quaternion.identity);	
		GameObject door = (GameObject)Instantiate (LevelManager.Instance.ObjList [32], _ts.transform.position, Quaternion.identity);	
		door.transform.SetParent (_ts.transform.GetChild (3));
		//door.transform.SetParent (_ts.transform.GetChild(0).GetChild (0).transform);

		//gives the door script the point
		DoorScript _ds = door.GetComponent<DoorScript> ();
		_ds.doorPos = _ts.GridPosition;

	}

	public void RemoveObj () {
		originObj.GetComponent <RoomScript> ().RemoveRoom (false);
	}

	public void RemoveRoom (bool _wasChecked) {
		if (!_wasChecked) {
			if (IsFree ()) {
				if (!last) {
					thisNextObj.GetComponent <RoomScript> ().RemoveRoom (false);
				} else {
					originObj.GetComponent <RoomScript> ().RemoveRoom (true);
				}
			} else {
				Debug.LogError ("Room is Obstructed");
			}
		}

		if (_wasChecked) {
			if (this.gameObject == originObj) {
				//saveStr = (objStr + ",0," + _gridPos.X.ToString () + "," + _gridPos.Y.ToString ());
				LevelManager.Instance.parameterList.Remove (saveStr);
			}

			if (!last) {
				thisNextObj.GetComponent <RoomScript> ().RemoveRoom (true);
			}

			if (isLeftBorder) {
				RemoveDoor (gridPos, true);
			} 
			if (isRightBorder) {
				RemoveDoor (gridPos, false);
			}

			UndoBool ();

			Destroy (gameObject);
		} 
	}

	//is the room free of stuff it may be removed
	private bool IsFree ()
	{
		//if (tile.OnFire || !tile.SystemPlacable || tile.Manned || tile.HasSubSysSlot && !tile.SubSysPlacable) {
		if (tile.OnFire || !tile.SystemPlacable || tile.Manned || tile.HasAccessor) {
			return false;
		} else {
			return true;
		}

		/*
		for (int w = 0; w <= roomWidth; w++) {
			Point tmp = new Point (GridPosition.X + w, GridPosition.Y, GridPosition.Z);
			TileScript tmpCheck = LevelManager.Instance.Tiles [tmp].GetComponent<TileScript> ();

			if (tmpCheck.OnFire || !tmpCheck.SystemPlacable || tmpCheck.Manned) 
			{ tileIsFree = false; break; } 

			if (w == roomWidth) 
			{ tileIsFree = true; }
		}
		*/
	}


	private void RemoveDoor (Point _point, bool _left)
	{
		TileScript _dp;
		TileScript _ts = LevelManager.Instance.Tiles [_point].GetComponent <TileScript> ();

		//tile pos x - 1
		if (_left) {
			Point _doorPos = new Point (_point.X - 1, _point.Y, _point.Z);
			_dp = LevelManager.Instance.Tiles [_doorPos].GetComponent <TileScript> ();		
			_ts = LevelManager.Instance.Tiles [_point].GetComponent <TileScript> ();
		} else {
			Point _doorPos = new Point (_point.X + 1, _point.Y, _point.Z);
			_dp = LevelManager.Instance.Tiles [_doorPos].GetComponent <TileScript> ();

			Point _pos = new Point (_point.X + 2, _point.Y, _point.Z);
			_ts = LevelManager.Instance.Tiles [_pos].GetComponent <TileScript> ();
		}
			
		if (_dp.HasDoor) {
			//_dp.HasDoor = false;
			GameObject _door = _ts.transform.GetChild (3).GetChild (0).gameObject;
			//GameObject _door = _ts.transform.GetChild (0).GetChild (0).GetChild (0).gameObject;
			Destroy (_door);
		}
	}	

	/*
	public void Remove ()
	{
		transform.parent.GetComponent <TileScript> ().RemoteRemoval (true, false, false);
	}
	*/

	/*
	public void Damage ()
	{
		gameObject.GetComponent <SpriteRenderer> ().sprite = dmgSprite;
	}
	*/

	//give it another bool, that checks if the current component is on the same level as the crew
	public void RoomComponentFree (TileScript _desTile, CrewScript _crew, bool _yFull) {
		//were all options for y == clickedDestination.Y full?
		if (_yFull) {
			if (tile.Manned || tile.IsDestination) {
				if (!last) {
					//repeats the process with the next component
					thisNextObj.GetComponent <RoomScript> ().RoomComponentFree (_desTile, _crew, true);
				} else {
					Debug.Log ("room is outta space");
				}
			} else {
				//continues the crewMovement process
				_crew.GiveGoAhead (true, tile);
			}
		} else {
			//shouldnt take crewPos, but destinationPos instead //done
			if (tile.Manned || tile.IsDestination || gridPos.Y != _desTile.GridPosition.Y) {
				//as of now, the right border is the rightest there is, thus if it was checked, there is no available option
				if (!last) {
					//repeats the process
					thisNextObj.GetComponent <RoomScript> ().RoomComponentFree (_desTile, _crew, false);
				} else {
					//the rightmost was checked, there is no space at the initial y coordinate
					originObj.GetComponent <RoomScript> ().RoomComponentFree (_desTile, _crew, true);
					Debug.Log ("room is outta space at the same hight");
				}
			} else {
				//continues the crewMovement process
				_crew.GiveGoAhead (true, tile);
			}
		}
	}




	//gets Contents (except for crew and other movables) of all roomComponents
	public void GetChildrenIni (int _id) {
		roomID = _id;
		//Debug.Log (_id);

		GameObject _obj = transform.parent.parent.gameObject;

		//systems
		if (_obj.transform.GetChild (1).childCount != 0) {
			//if (_obj.transform.GetChild (1).GetChild (0) != null) {
			roomContents.Add (_obj.transform.GetChild (1).GetChild (0).gameObject);
			//Debug.Log ("has sys"); 
			//}
		}

		//subSystems
		if (_obj.transform.GetChild (6).childCount != 0) {
			//if (_obj.transform.GetChild (6).GetChild (0) != null) {
			roomContents.Add (_obj.transform.GetChild (6).GetChild (0).gameObject);
			//Debug.Log ("has subSys");
			//}
		}

		if (!last) {
			thisNextObj.GetComponent <RoomScript> ().GetChildrenIni (_id);
		}
	}

	/* 21.04.18
	//actually only the origin needs to have an hp script... //?
	public void DistributeDamage (bool _fromOrigin, int _amount) {
		if (isOrigin || _fromOrigin) {
			for (int i = 0; i < roomContents.Count; i++) {
				roomContents [i].GetComponent <HealthScript> ().TakeDamage (_amount);
			}

			//causes FixShotKillz
			/*
			//crew
			GameObject _obj = transform.parent.parent.gameObject;
			//if (_obj.transform.GetChild (2).childCount != 0) {
			for (int i = 0; i < _obj.transform.GetChild (2).childCount; i++) {
				//_obj.transform.GetChild (2).GetChild (i).GetComponent <HealthScript> ().TakeDamage (_amount);
				_obj.transform.GetChild (2).GetChild (i).GetComponent <CrewScript> ().RemoveObj ();
			}
			//}
			////


			//room
			gameObject.GetComponent <HealthScript> ().TakeDamage (_amount); 

			if (!last) {
				thisNextObj.GetComponent <RoomScript> ().DistributeDamage (true, _amount);
			}

			//} else if (_fromOrigin) {
			//	thisNextObj.GetComponent <RoomScript> ().DistributeDamage (true, _amount);
		} else {
			originObj.GetComponent <RoomScript> ().DistributeDamage (false, _amount);
		}
	}
	*/


	/*
	public void GetRepaired (int _amount) {
		if (isOrigin) {
			RepairProgress += _amount;
			if (RepairProgress >= 100) {
				//Debug.Log (repairProgress);

				//this way the excess repairProgress gets carried over...
				RepairProgress = RepairProgress - 100;
				//this way it doesnt
				//repairProgress = 0;

				Debug.Log ("100!");

				//repairs by 10
				DistributeDamage (false, 10);
			}
		} else {
			originObj.GetComponent <RoomScript> ().GetRepaired (_amount);
		}
	}
	*/

	public RoomScript GetRoomOrigin () {
		return originObj.GetComponent <RoomScript> ();
	}

	public int Damages (int _amount) {
		damages += _amount;
		Debug.Log ("damages: " + damages);

		if (damages > 0) {
			//start/restart all repairs
			ManageAllRepairs (true);
		} else if (damages == 0) {
			//stop all repairs
			ManageAllRepairs (false);
		} else if (damages < 0) {
			Debug.LogError ("Too many fixes");
		}

		return damages;
	}

	public void ManageAllRepairs (bool _isDamaged) {
		//Debug.Log ("theres repairs needed: " + _isDamaged);

		Transform _trans = transform.parent.parent.GetChild (2);
		for (int i = 0; i < _trans.childCount; i++) {
			//_trans.GetChild (i).GetComponent <CrewScript> ().RepairManager (_isDamaged);
		}

		if (!last) {
			thisNextObj.GetComponent <RoomScript> ().ManageAllRepairs (_isDamaged);
		}
	}

	public bool IsDamaged () {
		return gameObject.GetComponent <HealthScript> ().IsDamaged;
	}



	private void UpdateRepairBar () {
		//Debug.Log ("repairProgress: " + repairProgress);
		//Debug.Log ("trying to repair: " + gridPos.X + ", " + gridPos.Y);
		float _float = (repairProgress / 100f);
		//Debug.Log ("float: " + _float);
		Vector3 _vect = new Vector3 (_float, 1f);
		//Debug.Log ("bar: " + _vect.x);
		healthBar.transform.localScale = _vect;
	}


	//new dmg system //test
	public void TakeCrewDamage (int _amount) {
		//RepairProgress = repairProgress - _amount;
		//Debug.Log (RepairProgress);


		/*
		if (isOrigin) {
			RepairProgress (_amount);
		} else {
			originObj.GetComponent <RoomScript> ().TakeCrewDamage (_amount);	
		}
		*/
	}

	private void RepairProgress (int _value) {
		repairProgress -= _value;

		if (repairProgress <= 0) {
			repairProgress = 0;
			isFullyDamaged = true;
			ChangeSprite ();
		} else if (repairProgress >= 100) {
			//Debug.Log ("over 9000!!");
			repairProgress = 100;
			isFullyRepaired = true;
			ChangeSprite ();
		} else {
			isFullyDamaged = false;
			isFullyRepaired = false; 
		}

		UpdateRepairBar ();
	}

	private void ChangeSprite () {
		if (isFullyDamaged) {
			sprRenderer.sprite = dmgSprite;
		} else if (IsFullyRepaired) {
			sprRenderer.sprite = roomSprite;
		}
	} 

	public GameObject GetOriginObj () {
		return originObj;
	}

	public void UpdateHealth (int _amount) {
		//gameObject.GetComponent <HealthScript> ().ChangeSprite ();
	}

	public void UpdateHealthState (bool _isFullyDamaged, bool _isFullyRepaired) {
		//Debug.Log ("room: ");

		//Debug.Log ("isFullyDamaged = " + _isFullyDamaged);
		//Debug.Log ("isFullyRepaired = " + _isFullyRepaired);
	}

	/*
	private void OnTriggerEnter2D (Collider2D _col) {
		Debug.LogError ("hey youve collided with me. room: " + gridPos.X + ", " + gridPos.Y + ", " + gridPos.Z);
	}
	*/

	public void TargetingPing (int _gunID, int _shipID) {
		Debug.LogError ("hey youve collided with me. room: " + gridPos.X + ", " + gridPos.Y + ", " + gridPos.Z);
		tile.PlaceTarget (_gunID, _shipID);
	}
}