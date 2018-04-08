using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CasheScript : Singleton<CasheScript>
{
	[SerializeField]
	private string shipType = "standard";
	public string ShipType { get { return shipType; } set { shipType = value; } }

	[SerializeField]
	private bool autoPlaceShip;
	public bool AutoPlace { get { return autoPlaceShip; } }

	//counts number of spawned players
	private int couchCrewCount = 0;
	//public int CouchCrewCount { {  } } 

	private bool couchMode;
	public bool CouchMode { get { return couchMode; } }

	//divide the screen by this number, spawn that number of crew;
	private int couchPlayerCount;
	public int CouchPlayerCount { get { return couchPlayerCount; } }

	public int CasheCount { get; private set; }

	private Dictionary <int, string> ctrlDict = new Dictionary <int, string> ();


	void Start ()
	{
		DontDestroyOnLoad (this);
		CasheCount++;
	}


	//debug reasons
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.T)) {
			Debug.Log (shipType);
		}
	}


	public void SwitchMode (bool _mode) {
		couchMode = _mode;
	}

	public void GetCtrlDict (Dictionary <int, string> _ctrlDict) {
		ctrlDict = _ctrlDict;
		couchPlayerCount = ctrlDict.Count;
		LogCtrlDict ();
	}

	private void LogCtrlDict () {
		for (int i = 0; i < ctrlDict.Count; i++) {
			Debug.Log (i + ctrlDict [i]);
		}
	}


	//assign crew controls here?
	public bool CouchCrewFits () {
		if (couchCrewCount < couchPlayerCount) {
			couchCrewCount++;
			Debug.Log ("couchCount: " + couchCrewCount);

			return true;
		} else {
			return false;
		}
	}

	public void AssignController (CouchCrewScript _couchCrew) {
		_couchCrew.CouchCrewSetup (ctrlDict [couchCrewCount - 1], couchCrewCount, couchPlayerCount);
	}
}