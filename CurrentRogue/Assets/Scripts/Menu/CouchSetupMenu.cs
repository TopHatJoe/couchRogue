using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouchSetupMenu : MonoBehaviour 
{
	private int localPlayerCount = 0;
	[SerializeField]
	private GameObject[] panelArr;

	//private List <GameObject> crewList = new List <GameObject> ();
	//private List <Camera> camList = new List <Camera> ();

	private bool j01active = false;
	private bool j02active = false;
	private bool j03active = false;
	private bool j04active = false;

	private List <bool> playerReady = new List<bool>();

	private Dictionary <int, string> ctrlDict = new Dictionary <int, string> ();


	void Start () {
		for (int i = 0; i < playerReady.Count; i++) {
			playerReady [i] = false;
		}
	}

	void Update () {
		if (!j01active) {
			if (Input.GetButtonDown ("J01-s")) {
				PlayerJoin ("J01");
				j01active = true;
			}	
		}
		if (!j02active) {
			if (Input.GetButtonDown ("J02-s")) {
				PlayerJoin ("J02");
				j02active = true;
			}
		}
		if (!j03active) {
			if (Input.GetButtonDown ("J03-s")) {
				PlayerJoin ("J03");				
				j03active = true;
			}
		}
		if (!j04active) {
			if (Input.GetButtonDown ("J04-s")) {
				PlayerJoin ("J04");
				j04active = true;
			}
		}
	}

	private void PlayerJoin (string _controllerID) {
		panelArr [localPlayerCount].SetActive (true);
		panelArr [localPlayerCount].GetComponent <ControllerMenu> ().GetControllerID (_controllerID);
		Debug.Log (_controllerID);

		ctrlDict.Add (localPlayerCount, _controllerID);
		playerReady.Add (false);

		localPlayerCount++;
	}

	public void SetReady (int _playerID, bool _isReady) {
		playerReady [_playerID] = _isReady;

		for (int i = 0; i < playerReady.Count; i++) {
			if (!playerReady [i]) {
				break;
			}

			if (i == playerReady.Count - 1) {
				Debug.Log ("all ready!");
				CasheScript.Instance.GetCtrlDict (ctrlDict);
			}
		}
	}
}