﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class CouchCursorScr : MonoBehaviour 
{
	private Rigidbody2D rb;
	[SerializeField]
	private float speed = 1024;
	private string controllerID = "J01";

	private Collider2D col;

	private CouchCrewScript couchCrew;

	[SerializeField]
	private bool isTargeter;

	private TerminalScr terminal;
	public TerminalScr Terminal { get { return terminal; } set { terminal = value; } }


	void Start () {
		rb = gameObject.GetComponent <Rigidbody2D> ();
		couchCrew = transform.parent.parent.GetComponent <CouchCrewScript> ();

		controllerID = couchCrew.ControllerID;
	}

	void Update () {
		Vector3 _vect = new Vector3 (Input.GetAxis (controllerID + "-H"), -Input.GetAxis (controllerID + "-V"));
		if (_vect.magnitude > 1) {
			_vect.Normalize ();
		}

		rb.MovePosition (transform.position + _vect * speed * Time.deltaTime);
		//Debug.Log (Input.GetAxis (controllerID + "-H") + ", " + Input.GetAxis (controllerID + "-V"));

		if (!isTargeter) {
			if (col != null) {
				if (Input.GetButtonDown (controllerID + "-s")) {
					//mayhap just make the cursor useless outside the panel...
					if (col.GetComponent <ElevatorBtnScr> () != null) {
						int _levelNum = col.GetComponent <ElevatorBtnScr> ().PressButton ();
						//Debug.Log ("please dont press that button again");
						couchCrew.UseElevator (_levelNum);
					}
				}
			}
		} 
		/*
		else {
			if (Input.GetButtonDown (controllerID + "-s")) {
				//raycast tile, trigger onMouse
				if (col != null) {
					TileScript _tile = col.GetComponent <TileScript> ();
					Debug.Log ("tile: " + _tile.GridPosition.X + ", " + _tile.GridPosition.Y + ", " + _tile.GridPosition.Z);
				}

			}
		}
		*/
	}


	public void OnTriggerStay2D (Collider2D _col) {
		if (isTargeter) {
			col = _col;

			if (col != null) {
                if (Input.GetButtonDown(controllerID + "-r2")) {
                    RoomScript _room = col.GetComponent<RoomScript>();

                    if (terminal.IsWeaponTerminal) { 
                        _room.TargetingPing(terminal.CurrentWeaponID, terminal.GridPos.Z);
                        //TileScript _tile = _room.transform.parent.parent.GetComponent <TileScript> ();
                        //Debug.LogError ("tile: " + _tile.GridPosition.X + ", " + _tile.GridPosition.Y + ", " + _tile.GridPosition.Z);
                    } else if (terminal.IsTeleporterTerminal) {
                        terminal.Teleport(_room.GridPos, true);
                    }
				}

                if (Input.GetButtonDown (controllerID + "-l2")) {
                    RoomScript _room = col.GetComponent<RoomScript>();

                    if (terminal.IsTeleporterTerminal) {
                        //no direct teleportation possible with the bool solution...
                        terminal.Teleport(_room.GridPos, false);
                    }
                }

			} else {
				Debug.LogError ("col is null!");
			}
		}
	}

	public void OnTriggerExit2D (Collider2D _col) {
		if (isTargeter) {
			col = null;
		} else {
			col = null;

			_col.GetComponent <SpriteRenderer> ().color = Color.grey;
			//_col.GetComponent <ElevatorBtnPanelScr> ().PressButton ();
		}
	}


	public void OnTriggerEnter2D (Collider2D _col) {
		if (!isTargeter) {
			col = _col;

			_col.GetComponent <SpriteRenderer> ().color = Color.green;
			//_col.GetComponent <ElevatorBtnPanelScr> ().PressButton ();
		} else {
            /* seems unnecessary 070218
			col = _col;

			if (col != null) {
				if (Input.GetButtonDown (controllerID + "-s")) {
					RoomScript _room = col.GetComponent <RoomScript> ();
					_room.TargetingPing (terminal.CurrentWeaponID, terminal.GridPos.Z);
					//TileScript _tile = _room.transform.parent.parent.GetComponent <TileScript> ();
					//Debug.LogError ("tile: " + _tile.GridPosition.X + ", " + _tile.GridPosition.Y + ", " + _tile.GridPosition.Z);
				}
			}
            */
		}

		//} else {
		//	_col.GetComponent <RoomScript> ().TargetingPing ();
		//}
	}

	/*
	public void OnTriggerExit2D (Collider2D _col) {
		//if (!isTargeter) {
			col = null;

			_col.GetComponent <SpriteRenderer> ().color = Color.grey;
			//_col.GetComponent <ElevatorBtnPanelScr> ().PressButton ();
		//}
	}
	*/
}