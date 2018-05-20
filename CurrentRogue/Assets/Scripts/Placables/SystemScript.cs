using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemScript : MonoBehaviour, IPlacable
{
	[SerializeField]
	//is there a next component to this obj?
	private bool last;

	private bool isOrigin;
	public bool IsOrigin { get { return isOrigin; } }

	[SerializeField]
	//next component to be placed
	private GameObject nextObj;
	//the instance of the obj placed by this instance
	private GameObject thisNextObj;
	private SystemScript thisNextSys;
	//the parent tile
	private TileScript tile;
	//the gridPos
	private Point gridPos;
	public Point GridPos { get { return gridPos; } }

	//the first obj to be placed, and removed
	private GameObject originObj;
	private SystemScript originSys;
	[SerializeField]
	//the dimensions of the obj
	private string objDimensions;


	[SerializeField]
	//two ints defining relative gridPos for nextObj
	private Vector2 nextPos;

	[SerializeField]
	private GameObject subSysSlot;

	[SerializeField]
	private bool hasSubSysSlot;

	[SerializeField]
	private string objStr;

	private string saveStr;

	private HealthScript hScr;
	public HealthScript HScr { get { return hScr; } }

	private bool isPowered = false;
	public bool IsPowered { get { return isPowered; } set { isPowered = value; tile.SysIsPowered = value; } }

	[SerializeField]
	private GameObject powerIndicator;
	private SpriteRenderer pwrIndicatorSpr;



	public void PlaceObj (int _index, Point _gridPos, GameObject _originObj) {
		originObj = _originObj;
		originSys = originObj.GetComponent <SystemScript> ();

		if (originObj == gameObject) {
			isOrigin = true;
			pwrIndicatorSpr = powerIndicator.GetComponent <SpriteRenderer> ();
		}

		if (this.gameObject == originObj) {
			//string _string = (objStr + ",1," + gridPos.X.ToString () + "," + gridPos.Y.ToString ());
			saveStr = (objStr + ",1," + _gridPos.X.ToString () + "," + _gridPos.Y.ToString ());
			LevelManager.Instance.parameterList.Add (saveStr);
		}

		//gridPos = _gridPos;
		gridPos = new Point (_gridPos.X + Mathf.RoundToInt (nextPos.x * 2), _gridPos.Y + Mathf.RoundToInt (nextPos.y), _gridPos.Z);


		subSysSlot.SetActive (hasSubSysSlot);


		tile = LevelManager.Instance.Tiles [gridPos];
		hScr = gameObject.GetComponent <HealthScript> ();


		transform.position = tile.transform.position;
		//Quaternion.identity = tile.Quaternion.identity;

		transform.SetParent (tile.transform.GetChild (1));

		//Debug.Log ("yaya");

		DoBool ();

		GameManager.Instance.Buy ();

		//subSysSlot.SetActive (hasSubSysSlot);

		if (!last) {
			//creates next component of obj

			//combinig nextPos with point to get the true next gridPos
			//Point _point = new Point (_gridPos.X + Mathf.RoundToInt (nextPos.x * 2), _gridPos.Y + Mathf.RoundToInt (nextPos.y), _gridPos.Z);
			//TileScript _tile = LevelManager.Instance.Tiles [_point];

			thisNextObj = (GameObject)Instantiate (nextObj);
			thisNextSys = thisNextObj.GetComponent <SystemScript> ();

			IPlacable _placable = thisNextObj.GetComponent <IPlacable> ();
			_placable.PlaceObj (_index, gridPos, originObj);

			hScr.NextHScr = thisNextObj.GetComponent <HealthScript> ();
		}
	}



	public void RemoveObj () {
		originObj.GetComponent <SystemScript> ().RemoveSystem (false);
	}

	private void RemoveSystem (bool _wasChecked) {
		if (!_wasChecked) {
			if (IsFree ()) {
				if (!last) {
					//thisNextObj.GetComponent <SystemScript> ().RemoveSystem (false);
					thisNextSys.RemoveSystem (false);
				} else {
					//originObj.GetComponent <SystemScript> ().RemoveSystem (true);
					originSys.RemoveSystem (true);
				}
			} else {
				Debug.Log ("System is Obstructed");
			}
		}

		if (_wasChecked) {
			if (this.gameObject == originObj) {
				LevelManager.Instance.parameterList.Remove (saveStr);
			}


			if (!last) {
				//thisNextObj.GetComponent <SystemScript> ().RemoveSystem (true);
				thisNextSys.RemoveSystem (true);
			}

			UndoBool ();

			Destroy (gameObject);
		}
	}

	private bool IsFree ()
	{
		//if (tile.OnFire || !tile.SystemPlacable || tile.Manned) {
		if (tile.HasSubSysSlot && !tile.SubSysPlacable) {
			return false;
		} else {
			return true;
		}
	}

	private void DoBool () {
		//Debug.Log ("Doing Bools on: " + tile.GridPosition.X + ", " + tile.GridPosition.Y);
		//tile.IsEmpty = false;
		//tile.Walkable = true;
		tile.SystemPlacable = false;
		tile.HasSubSysSlot = hasSubSysSlot;
		tile.SubSysPlacable = hasSubSysSlot;

		//tile.Manned = false;
		//tile.IsDestination = false;

		//tile.HasElevator = false;

		//tile.HasDoorSlot = HasDoorSlot;
	}

	private void UndoBool () {
		//tile.IsEmpty = true;
		//tile.Walkable = false;

		tile.SystemPlacable = true;
		tile.HasSubSysSlot = false;
		tile.SubSysPlacable = false;


		//tile.Manned = true;
		//tile.IsDestination = false;
		//tile.HasElevator = false;
		//tile.HasDoorSlot = false;
	}


	public void Remove ()
	{
		//transform.parent.GetComponent <TileScript> ().RemoteRemoval (false, true, false);
	}

	/* 21.04.18
	public void UpdateHealth (bool _isFullyDamaged, bool _isFullyRepaired) {
		//mayhap implement ISystem interface for consequences...
		//Debug.Log ("sys took Damage");


		if (gameObject.GetComponent <ISystem> () != null) { 
			ISystem _sys = gameObject.GetComponent <ISystem> ();
			_sys.UpdateHealthState (_isFullyDamaged, _isFullyRepaired);
		}
	}
	*/

	public GameObject GetOriginObj () {
		return originObj;
	}

	public void UpdateHealthState (bool _isFullyDamaged, bool _isFullyRepaired) {
		//NetManager.Instance.SyncSysHealth (gridPos, _isFullyDamaged, _isFullyRepaired);
		ISystem _sys = gameObject.GetComponent <ISystem> ();
		_sys.UpdateHealthState (_isFullyDamaged, _isFullyRepaired);

		/*
		if (gameObject.GetComponent <ISystem> () != null) { 
			ISystem _sys = gameObject.GetComponent <ISystem> ();
			_sys.UpdateHealthState (_isFullyDamaged, _isFullyRepaired);
		}
		*/
	}

	/* syncd health
	public void ReceiveHealthUpdate (bool _isFullyDamaged, bool _isFullyRepaired) {
		ISystem _sys = gameObject.GetComponent <ISystem> ();
		_sys.UpdateHealthState (_isFullyDamaged, _isFullyRepaired);
	}
	*/


	public void UpdatePowerState (bool _isPowered) {
		//update indicator
		if (isOrigin) {
			if (_isPowered) {
				pwrIndicatorSpr.color = Color.green;
			} else {
				pwrIndicatorSpr.color = Color.grey;
			}
		}


		//update powerState
		if (gameObject.GetComponent <ISystem> () != null) { 
			ISystem _sys = gameObject.GetComponent <ISystem> ();
			_sys.UpdatePowerState (_isPowered);
		}

		if (!last) {
			thisNextSys.UpdatePowerState (_isPowered);
		}
	}



	public void SyncPowerUpdate (bool _isPowered) {
		NetManager.Instance.SyncPowerState (gridPos, _isPowered);
	}

	public void ReceivePowerUpdate (bool _isPowered) {
		Debug.LogError ("system powered up at: " + gridPos.X + ", " + gridPos.Y + ", " + gridPos.Z);


		//update systenPower
		ISystem _iSys = gameObject.GetComponent <ISystem> ();
		_iSys.ReceivePowerUpdate (_isPowered);
	}
}