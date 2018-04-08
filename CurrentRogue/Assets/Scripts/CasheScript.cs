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

	private bool couchMode;
	public bool CouchMode { get { return couchMode; } }

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
		LogCtrlDict ();
	}

	private void LogCtrlDict () {
		for (int i = 0; i < ctrlDict.Count; i++) {
			Debug.Log (i + ctrlDict [i]);
		}
	}
}