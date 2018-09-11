using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//enum GameMode { hangar = 0, classic, couch }
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
    //hangar/classic/couch
	private int gameMode;
	public int GameMode { get { return gameMode; } }

	//divide the screen by this number, spawn that number of crew;
	private int couchPlayerCount;
	public int CouchPlayerCount { get { return couchPlayerCount; } }

	public int CasheCount { get; private set; }

	private Dictionary <int, string> ctrlDict = new Dictionary <int, string> ();
    public Dictionary <int, string> CtrlDict { get { return ctrlDict; } }

	//because reasons //net sync too slow...
	private int assignedCrewCount = 0;


    private int numOfLocalShips;


    private List<List<string>> crewAssignments;
    public List<List<string>> CrewAssignments { get { return CrewAssignments; } }

    private int lastShip = -1;
    private int crewOnThisShipCounter = 0;

    //mulitship stuff

    //used by server only (?) //List <ownerID, shipString> //both are passed /where to?/ by PlaceShip (int, string)
    //private Dictionary <int, string> shipList = new Dictionary<int, string>();
    //public Dictionary <int, string> ShipList { get { return shipList; } set { shipList = value; } }

    private List<ShipInfo> shipList = new List<ShipInfo>();
    public List<ShipInfo> ShipList { get { return shipList; } set { shipList = value; } }


	void Start ()
	{
		DontDestroyOnLoad (this);
		CasheCount++;
	}


	//debug reasons
    /*
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.T)) {
			Debug.Log (shipType);
		}
	}
	*/


	public void SwitchMode (bool _mode) {
		couchMode = _mode;
		if (couchMode == true) {
			gameMode = 2;
		} else {
			gameMode = 1;
		}
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
	public bool CouchCrewFits (int _shipID) {
        if (lastShip != _shipID) {
            crewOnThisShipCounter = 0;
            lastShip = _shipID;
        }

        if (crewAssignments[_shipID].Count > crewOnThisShipCounter) {
            crewOnThisShipCounter++;
            return true;
        } else {
            return false;
        }

        /*
		if (couchCrewCount < couchPlayerCount) {
			couchCrewCount++;
			Debug.Log ("couchCount: " + couchCrewCount);

			return true;
		} else {
			return false;
		}
        */
	}

	public void AssignController (CouchCrewScript _couchCrew, int _shipID) {
		//crew placement os too fast for network sync 
		//-> count needs to be increased twice, once for placement, once for controller 

		assignedCrewCount++;
        string _ctrlID = crewAssignments[_shipID][0];
        Debug.Log("ass-ignment: " + _ctrlID);

        _couchCrew.CouchCrewSetup(_ctrlID, assignedCrewCount, couchPlayerCount);
        //_couchCrew.CouchCrewSetup (ctrlDict [assignedCrewCount - 1], assignedCrewCount, couchPlayerCount);

        //Debug.LogError("pre");
        crewAssignments[_shipID].Remove(_ctrlID);
        //Debug.LogError("post");

		//_couchCrew.CouchCrewSetup (ctrlDict [couchCrewCount - 1], couchCrewCount, couchPlayerCount);
	}

	public void UpdateGameMode (int _mode) {
		gameMode = _mode;

		if (_mode == 0) {
			Debug.Log ("hangar");
		} else if (_mode == 1) {
			Debug.Log ("classic");
			SwitchMode (false);
		} else if (_mode == 2) {
			Debug.Log ("couch");
			SwitchMode (true);
		} else {
			Debug.LogError ("unknown mode!");
			gameMode = 0;
		}
	}	

    public void GetCrewAssignments (List <List <string>> _value) {
        crewAssignments = _value;

        /* DEBUG
        for (int i = 0; i < _value[0].Count; i++)
        {
            Debug.Log("het! ctrlID: " + _value[0][i]);
        }
        */
    }
}