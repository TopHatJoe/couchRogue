using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CouchCrewScript : NetworkBehaviour 
{
	private Rigidbody2D rb;
	private BoxCollider2D col;
	private NetworkIdentity netID;

	//[SerializeField]
	private float speed = 4096;

	private string controllerID;
	public string ControllerID { get { return controllerID; } }

	[SerializeField]
	private Camera cam;
	[SerializeField]
	private Camera crewShipCam;

	private int couchPlayerID;
	//the player is using an elevator, repairing stuff etc
	private bool isOccupied = false;
	private bool elevatorIsNear;
	private bool usingElevator = false;
	private bool usingTerminal = false;

	private ElevatorScript elevator;
	private TerminalScr terminalScr;

	[SerializeField]
	private GameObject elevatorMenu;
	private IEnumerator useElevator;
	private IEnumerator dmgLoop;

	private Point crewPos;

	//important for crew reassignment
	private float tileDistance;
	private Vector3 previousPos;

	//targeting
	private GameObject targetingCursor;

	private ShipScript targetedShip;
	private Color crewColor;
	public Color CrewColor { get { return crewColor; } } 

	private bool isLocal = false;
	private bool controlsSet = false;

	private int targetedShipID;

    private HealthScript hScr;



	void Start () {
		rb = gameObject.GetComponent <Rigidbody2D> ();
		col = gameObject.GetComponent <BoxCollider2D> ();
		netID = gameObject.GetComponent <NetworkIdentity> ();
        hScr = gameObject.GetComponent<HealthScript>();
	
		if (isServer) {
			//NetworkIdentity.
			crewPos = gameObject.GetComponent <CrewScript> ().crewPos;

            //necessary for ini, wouldnt otherwise be registrered properly //?
            //UpdateCrewPos(0, 0); //stupid ^^
            TileScript _tile = LevelManager.Instance.Tiles[crewPos];
            RoomScript _room = _tile.transform.GetChild(0).GetChild(0).GetComponent<RoomScript>();
            _room.EnterRoom(hScr);

			SyncCrewPos ();

			netID.AssignClientAuthority (NetManager.Instance.ConnDict [crewPos.Z]);
		}


		targetingCursor = transform.GetChild (3).GetChild (0).gameObject;

		GetTileDistances ();

		//Vector3 _vect = transform.position;
		//vect.x -= tileDistance;
		previousPos = transform.position;
		//previousPos = _vect;

		Debug.Log (tileDistance);

        //_cam = transform.GetChild (0).gameObject.GetComponent <Camera> ();

        //StartCoroutine (Test ());

        //CrewColorAssignment ();
        //Debug.Log ("crewColour: " + crewColor);
        //Debug.Log ("crewID: " + couchPlayerID);

        //Debug.LogError ("isLocalCrew: " + isLocal);
        StartCoroutine(CrewPosCheck());
	}

	void Update () {
		if (isLocal) {
			CrewControls ();
		}

        //might actually also be if (isLocal)...
		ReassignCrewPos ();

		/*
		if (true) {
			Vector3 _vect = Vector3.left;
			rb.MovePosition (transform.position + _vect * speed * Time.deltaTime);
		}
		*/
	}

	private void CrewControls () {
		//Debug.LogError ("ctrl!");
		if (!isOccupied) {
			Vector3 _vect = new Vector3 (Input.GetAxis (controllerID + "-H"), 0);
			rb.MovePosition (transform.position + _vect * speed * Time.deltaTime);

			/*
			if (elevatorIsNear) {
				if (Input.GetButtonDown (controllerID + "-s")) {
					DoElevatorMenu ();
				}
			} else {
			*/	
			if (Input.GetButtonDown (controllerID + "-s")) {
				//power system
				TileScript _tile = LevelManager.Instance.Tiles [crewPos];
				if (_tile.HasElevator) {
					DoElevatorMenu ();
				} else if (!_tile.SystemPlacable) {
					if (_tile.SysIsPowered && _tile.HasSubSys) {
						//use terminal
						//Debug.LogError ("im afraid i cant let you do that dave...");
						terminalScr = _tile.GetTerminal ();

						if (!terminalScr.IsUsed) {
							isOccupied = true;
							usingTerminal = true;
							terminalScr.UseTerminal (this);
							targetingCursor.gameObject.SetActive (true);
						}

					} else {
						//ISystem _iSys = _tile.GetSystem ();
						SystemScript _sysScr = _tile.GetSystem ();

						//currently just switches //the bool ain't needed
						_sysScr.SyncPowerUpdate (true);
						//_sysScr.ReceivePowerUpdate (true);

						//let's network that! //in iSys
					}
				}
			}
			//}

			if (Input.GetButtonDown (controllerID + "-a")) {
				//DoSomeDamage (10);
				CmdSyncLoops (true, 10);
			} else if (Input.GetButtonDown (controllerID + "-t")) {
				//DoSomeDamage (-10);
				CmdSyncLoops (true, -10);
			}
			//exits terminal
		} else if (usingTerminal) {
			
			//stop using target
			if (Input.GetButtonDown (controllerID + "-c")) {
				terminalScr.StopUsingTerminal ();
				SetCrewCamValues (null, false, 0);

				targetingCursor.gameObject.SetActive (false);
				usingTerminal = false;
				isOccupied = false;
			}
			
			//swap weapons //temp?
			if (Input.GetButtonDown (controllerID + "-a")) {
				terminalScr.SwapWeapon (1);
			} else if (Input.GetButtonDown (controllerID + "-t")) {
				terminalScr.SwapWeapon (-1);
			}

			//swap ships
			if (Input.GetButtonDown (controllerID + "-l1")) {
				terminalScr.SwapTargetedShip (-1);
			} else if (Input.GetButtonDown (controllerID + "-r1")) {
				terminalScr.SwapTargetedShip (1);
			}

			//power weapon up/down
			if (Input.GetButtonDown (controllerID + "-s")) {
				//bool doesn't matter currently...
				terminalScr.PowerWeapon (true);
			}

				//switch ship cameras

		} else if (!usingElevator) {
			if (Input.GetButtonDown (controllerID + "-c")) {
				//StopSomeDamage ();
				CmdSyncLoops (false, 0);
			}
		}
	}


	private void ReassignCrewPos () {
		//Debug.LogError ("crewtrans: " + transform.position.x);
		//Debug.LogError ("crewPrev: " + previousPos.x);
		//Debug.LogError ("tileDist: " + tileDistance);

		//reassigns crewPos
		if (transform.position.x - previousPos.x > tileDistance) {
			//moved to the right
			UpdateCrewPos (2, 0);

			//crewPos.X += 2;
			//previousPos = LevelManager.Instance.Tiles [crewPos].transform.position;
			//Debug.Log ("crewPos: " + crewPos.X);
		} else if (transform.position.x - previousPos.x < -tileDistance) {
			//moved to the left
			UpdateCrewPos (-2, 0);

			//crewPos.X -= 2;
			//previousPos = LevelManager.Instance.Tiles [crewPos].transform.position;
			//Debug.Log ("crewPos: " + crewPos.X);
		}
	}



	public void CouchCrewSetup (string _controllerID, int _couchPlayerID, int _couchCount) {
		Debug.LogError ("crewSetup");

		controllerID = _controllerID;
		isLocal = true;

		//Debug.Log ("playerID: " + _couchPlayerID);
		//Debug.Log ("couchCount: " + _couchCount);

		couchPlayerID = _couchPlayerID;
		CrewColorAssignment ();

		if (_couchCount == 2) {
			TwoPlayerSplit (_couchPlayerID);
		} else if (_couchCount == 3) {
			ThreePlayerSplit (_couchPlayerID);
		} else if (_couchCount == 4) {
			FourPlayerSplit (_couchPlayerID);
		}

		crewShipCam.rect = cam.rect;
		cam.gameObject.SetActive (true);

		CanvasManager.Instance.CouchCanvas (couchPlayerID, cam);


	}

	private void CrewColorAssignment () {
		if (couchPlayerID == 1) {
			crewColor = Color.white;
		} else if (couchPlayerID == 2) {
			crewColor = Color.red;
		} else if (couchPlayerID == 3) {
			crewColor = Color.yellow;
		} else if (couchPlayerID == 4) {
			crewColor = Color.cyan;
		} 
	}

	private void TwoPlayerSplit (int _couchPlayerID) {
		if (_couchPlayerID == 1) {
			Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
			cam.rect = _rect0;
		} else if (_couchPlayerID == 2) {
			Rect _rect1 = new Rect (0f, 0f, 1f, 0.5f);
			cam.rect = _rect1;
		}


		//Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
		//Rect _rect1 = new Rect (0f, 0f, 1f, 0.5f);

		//camList [0].rect = _rect0;
		//camList [1].rect = _rect1;
	}

	private void ThreePlayerSplit (int _couchPlayerID) {
		if (_couchPlayerID == 1) {
			Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
			cam.rect = _rect0;
		} else if (_couchPlayerID == 2) {
			Rect _rect1 = new Rect (0f, 0f, 0.5f, 0.5f);
			cam.rect = _rect1;
		} else if (_couchPlayerID == 3) {
			Rect _rect2 = new Rect (0.5f, 0f, 0.5f, 0.5f);
			cam.rect = _rect2;
		}
			

		//Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
		//Rect _rect1 = new Rect (0f, 0f, 0.5f, 0.5f);
		//Rect _rect2 = new Rect (0.5f, 0f, 0.5f, 0.5f);

		//camList [0].rect = _rect0;
		//camList [1].rect = _rect1;
		//camList [2].rect = _rect2;
	}

	private void FourPlayerSplit (int _couchPlayerID) {
		if (_couchPlayerID == 1) {
			Rect _rect0 = new Rect (0f, 0.5f, 0.5f, 0.5f);
			cam.rect = _rect0;
		} else if (_couchPlayerID == 2) {
			Rect _rect1 = new Rect (0.5f, 0f, 0.5f, 0.5f);
			cam.rect = _rect1;
		} else if (_couchPlayerID == 3) {
			Rect _rect2 = new Rect (0f, 0.5f, 0.5f, 0.5f);
			cam.rect = _rect2;
		} else if (_couchPlayerID == 4) {
			Rect _rect3 = new Rect (0.5f, 0f, 0.5f, 0.5f);
			cam.rect = _rect3;
		}


		//Rect _rect0 = new Rect (0f, 0.5f, 0.5f, 0.5f);
		//Rect _rect1 = new Rect (0.5f, 0f, 0.5f, 0.5f);
		//Rect _rect2 = new Rect (0f, 0.5f, 0.5f, 0.5f);
		//Rect _rect3 = new Rect (0.5f, 0f, 0.5f, 0.5f);

		//camList [0].rect = _rect0;
		//camList [1].rect = _rect1;
		//camList [2].rect = _rect2;
		//camList [3].rect = _rect3;
	}


	public void OnCollosionEnter2D (Collision2D _col) {
		//Debug.Log (_col.gameObject.layer);

		if (_col.gameObject.layer == 8) {
			Physics2D.IgnoreCollision (_col.collider, col);
		}


	}

	public void IsElevatorNear (bool _isNear, ElevatorScript _elevator) {
		elevatorIsNear = _isNear;
		elevator = _elevator;
	}

	private void DoElevatorMenu () {
		//Debug.Log ("ele- ele- elevator!");


		isOccupied = true;
		usingElevator = true;

		elevatorMenu.SetActive (true);
		//give the menu the elevator reference

		ElevatorScript _elevator = elevator.GetComponent <ElevatorScript> ();
		elevatorMenu.transform.GetChild (1).GetComponent <ElevatorBtnPanelScr> ().SetBtns (_elevator);
	}


	public void UseElevator (int _level) {
		elevatorMenu.SetActive (false);

		useElevator = UseElevatorCR (_level);
		StartCoroutine (useElevator);
	}

	private IEnumerator UseElevatorCR (int _level) {
		rb.simulated = false;

		//move here
		Point _point = elevator.GridPos;
		Vector3 _pos = LevelManager.Instance.Tiles [_point].transform.position;

        int _hDifference = (_level - _point.Y);

		//Vector3 _direction = _pos - transform.position;

		//Debug.Log (transform.position);
		//Debug.Log (_pos);

		while (Vector2.Distance (transform.position, _pos) > 5f) {
			//Vector3 _vect = Vector3.MoveTowards (transform.position, _pos, speed * Time.deltaTime);
			//_direction = _pos - transform.position;
			//_direction.Normalize ();
			transform.position = Vector2.MoveTowards (transform.position, _pos, speed * 0.05f);
			//rb.MovePosition (transform.position + _direction * speed * Time.deltaTime);
			yield return new WaitForSeconds (0.05f);
		}


		//Debug.Log ("im tryin, im tryin!");

		/*
		rb.MovePosition (Vector2.down);

		if (_point.Y < _level) {
			//up
		} else {
			//down
		}

		//move here
		//_point.Y = _level;
		*/

		_point.Y = _level;
		TileScript _tile = LevelManager.Instance.Tiles [_point];
		_pos = _tile.transform.position;

		//Debug.Log (transform.position);
		//Debug.Log (_pos);

		while (Vector2.Distance (transform.position, _pos) > 0.5f) {
			transform.position = Vector2.MoveTowards (transform.position, _pos, speed * 0.05f);


			//Vector3 _vect = Vector3.MoveTowards (transform.position, _pos, Time.deltaTime);
			//rb.MovePosition (transform.position + _vect * speed * Time.deltaTime);
			yield return new WaitForSeconds (0.05f);
		}

		//Debug.Log ("Done");
		rb.simulated = true;
		isOccupied = false;
		usingElevator = false;
		rb.MovePosition (transform.position + Vector3.left);

        //crewPos = _tile.GridPosition;
        UpdateCrewPos(0, _hDifference);

        //UpdateCrewPos(_tile.GridPosition);

		//Debug.LogError ("_tile: " + _tile.GridPosition.X + ", " + _tile.GridPosition.Y + ", " + _tile.GridPosition.Z);
		//Debug.LogError ("crewPos: " + crewPos.X + ", " + crewPos.Y + ", " + crewPos.Z);

		SyncCrewPos ();

		previousPos = _pos;
	}

    /*
    private void UpdateCrewPos (Point _newPos) {
        
        crewPos = _newPos;
    }
    */



	//crew reassignment //test

	//gets the distance between tiles for UpdateCrewPos ()
	private void GetTileDistances () {
		//theres probably a more elegant soltion to this... but fuggid!
		Point _point0 = new Point (0, 0, 0);
		Point _point1 = new Point (1, 0, 0);

		Vector3 _vect0 = LevelManager.Instance.Tiles [_point0].transform.position;
		Vector3 _vect1 = LevelManager.Instance.Tiles [_point1].transform.position;

		float _distance = _vect1.x - _vect0.x;
		tileDistance = (_distance + 0.1f);
	}




    private void UpdateCrewPos (int _x, int _y) {
        TileScript _tile = LevelManager.Instance.Tiles[crewPos];
        RoomScript _room = _tile.transform.GetChild(0).GetChild(0).GetComponent<RoomScript>();
        //_room.ExitRoom();
        //Debug.LogError("crewPos: " + crewPos.X + ", " + crewPos.Y);
    
		crewPos.X += _x;
        crewPos.Y += _y;
		//Vector3 _vect = LevelManager.Instance.Tiles [crewPos].transform.position;
		//_vect.x -= (tileDistance / 2);



		//stuff here!
		//Debug.LogError ("crewPos: " + crewPos.X + ", " + crewPos.Y + ", " + crewPos.Z);


        _tile = LevelManager.Instance.Tiles[crewPos];
        previousPos = _tile.transform.position;


        //_tile = LevelManager.Instance.Tiles[crewPos];

        RoomScript _nextRoom = _tile.transform.GetChild(0).GetChild(0).GetComponent<RoomScript>();
        //_room.EnterRoom();
        _room.ChangeRoom(_nextRoom, hScr);
        //previousPos = _vect;

		//Debug.LogError ("crewPos: " + crewPos.X + ", " + crewPos.Y);

	}



	//crew damage
	private void DoSomeDamage (int _amount) {
		//Debug.Log ("gonna do some damage!");
		dmgLoop = DmgLoop (_amount);
		StartCoroutine (dmgLoop); 
	}

	private void StopSomeDamage () {
		//Debug.Log ("gonna stop doin some damage!");

		StopCoroutine (dmgLoop);

		isOccupied = false;

		//Debug.Log ("stopped damaging!");
	}

	/*
	private IEnumerator RoomDmgLoop (int _amount) {
		isOccupied = true;

		RoomScript _room = LevelManager.Instance.Tiles [crewPos].transform.GetChild (0).GetChild (0).GetComponent <RoomScript> ();

		yield return new WaitForSeconds (1f);

		while (true) {
			_room.TakeCrewDamage (_amount);
			//Debug.Log ("took " + _amount + " damage!");

			if (_amount > 0) {
				if (_room.IsFullyDamaged) {
					//Debug.Log ("break!");
					break;
				}
			} else if (_amount < 0) {
				if (_room.IsFullyRepaired) {
					//Debug.Log ("break!");
					break;
				}
			}

			yield return new WaitForSeconds (1f);
		}

		isOccupied = false;
	}
	*/

	private IEnumerator DmgLoop (int _amount) {
		isOccupied = true;
		TileScript _tile = LevelManager.Instance.Tiles [crewPos];

		//RoomScript _room = LevelManager.Instance.Tiles [crewPos].transform.GetChild (0).GetChild (0).GetComponent <RoomScript> ();

		HealthScript[] _hScrArr = _tile.GetHScripts (_amount, _amount, _amount);

		yield return new WaitForSeconds (1f);

		bool _hasSys = false;
		if (_hScrArr [1] != null) {
			_hasSys = true;
		}

		bool _hasSubSys = false;
		if (_hScrArr [2] != null) {
			_hasSubSys = true;
		}

		while (true) {
			//_tile.TakeCrewDamage (_amount, 0, 0);
			_hScrArr [0].TakeCrewDamage (_amount);
			if (_hasSys) {
				_hScrArr [1].TakeCrewDamage (_amount - 2);
			}
			if (_hasSubSys) {
				_hScrArr [2].TakeCrewDamage (_amount + 2);
			}

			//Debug.Log ("took " + _amount + " damage!");



			//this should happen if all obj on that tile are completely destroyed or repaired...
			if (_amount > 0) {
				if (_hScrArr[0].IsFullyDamaged) {
					if (_hasSys) {
						if (_hScrArr [1].IsFullyDamaged) {
							if (_hasSubSys) {
								if (_hScrArr [2].IsFullyDamaged) {
									break;
								}
							} else {
								break;
							}
						}
					} else if (_hasSubSys) {
						if (_hScrArr [2].IsFullyDamaged) {
							break;
						}
					} else {
						break;
					}
				}
			} else if (_amount < 0) {
				if (_hScrArr [0].IsFullyRepaired) {
					if (_hasSys) {
						if (_hScrArr [1].IsFullyRepaired) {
							if (_hasSubSys) {
								if (_hScrArr [2].IsFullyRepaired) {
									break;
								}
							} else {
								break;
							}
						}
					} else if (_hasSubSys) {
						if (_hScrArr [2].IsFullyRepaired) {
							break;
						}
					} else {
						//Debug.Log ("break!");
						break;
					}
				}
			}



			yield return new WaitForSeconds (0.2f);
		}

		isOccupied = false;
	}

	/*
	private IEnumerator Test () {
		while (true) {
			Vector3 _vect = Vector3.left;
			rb.MovePosition (transform.position + _vect * speed / 0.05f);

			yield return new WaitForSeconds (0.05f);
		}
	}
	*/





	public void SetCrewCamValues (ShipScript _ship, bool _usingTerminal, int _shipID) {
		if (_usingTerminal) {
			targetedShip = _ship;
			crewShipCam.transform.SetParent (targetedShip.transform);
			//Debug.LogError ("cam size: " + crewShipCam.orthographicSize);
			crewShipCam.orthographicSize = targetedShip.ShipCam.orthographicSize;
			//Debug.LogError ("cam size: " + crewShipCam.orthographicSize);


			cam.gameObject.SetActive (false);
			crewShipCam.gameObject.SetActive (true);

			//crewShipCam.transform.position = new Vector3 (0, 0, -20);
			crewShipCam.transform.position = _ship.ShipCam.transform.position;

			targetingCursor.transform.position = _ship.transform.position;
			/* 
			Matrix4x4 _mat = crewShipCam.projectionMatrix;
			if (NetManager.Instance.localPlayerID == _shipID) {
				_mat *= Matrix4x4.Scale (new Vector3 (1, 1, 1));
			} else {
				_mat *= Matrix4x4.Scale (new Vector3 (-1, 1, 1));
			}
			crewShipCam.projectionMatrix = _mat;
			*/
		}

		cam.gameObject.SetActive (!_usingTerminal);
		crewShipCam.gameObject.SetActive (_usingTerminal);
	}





	public void GiveTerminalReference (TerminalScr _terminal) {
		targetingCursor.GetComponent <CouchCursorScr> ().Terminal = _terminal;
	}


	private void SyncCrewPos () {
		Vector3 _vect = new Vector3 (crewPos.X, crewPos.Y, crewPos.Z);
		CmdSyncCrewPos (_vect);
	}

	[Command]
	private void CmdSyncCrewPos (Vector3 _vect) {
		//the servers crewPos was set properly...

		//Vector3 _vect = new Vector3 (crewPos.X, crewPos.Y, crewPos.Z);

		//Debug.LogError ("crewPosOnServer");

		RpcSyncCrewPos (_vect);
	}

	[ClientRpc]
	private void RpcSyncCrewPos (Vector3 _vect) {
		Point _pos = new Point (Mathf.RoundToInt(_vect.x), Mathf.RoundToInt(_vect.y), Mathf.RoundToInt(_vect.z));

		crewPos = _pos;

		//Debug.LogError ("syncedCrewPos: " + crewPos.X + ", " + crewPos.Y + ", " + crewPos.Z); //"crewPosOnClient");

		SetControls ();
		/*
		if (crewPos.Z == NetManager.Instance.localPlayerID) {
			isLocal = true;

			//couchCrewSetup is called by this as well...
			CasheScript.Instance.AssignController (this);
		}
		*/

		//Debug.LogError ("RpcCrewPos: " + crewPos.Z + ", netID: " + NetManager.Instance.localPlayerID);
		//Debug.LogError ("crewPos: " + crewPos.X + ", " + crewPos.Y + ", " + crewPos.Z);

	}

	private void SetControls () {
		if (!controlsSet) {
			if (crewPos.Z == NetManager.Instance.localPlayerID) {
				isLocal = true;

				//couchCrewSetup is called by this as well...
				CasheScript.Instance.AssignController (this);
				controlsSet = true;
			}
		}
	}



	[Command]
	private void CmdSyncLoops (bool _doDamage, int _amount) {
		RpcSyncLoops (_doDamage, _amount);
	}

	[ClientRpc]
	private void RpcSyncLoops (bool _doDamage, int _amount) {
		//	Debug.LogError ("dmgLoop: " + _amount);

		if (_doDamage) {
			DoSomeDamage (_amount);
		} else {
			StopSomeDamage ();
		}
	}



    private IEnumerator CrewPosCheck () {
        while (true) {
            yield return new WaitForSeconds(1f);

            Debug.LogError("crewTransform" + crewPos.Z + ": " + transform.position.x + ", " + transform.position.y);
            TileScript _tile = LevelManager.Instance.Tiles[crewPos];
            Debug.LogError("crewTile[" + crewPos.X + "/" + crewPos.Y + "/" + crewPos.Z + "]: " + _tile.transform.position.x + ", " + _tile.transform.position.y);
            float _difference = (transform.position.x - _tile.transform.position.x);
            Debug.LogError("difference" + crewPos.Z + ": " + _difference);

            if (_difference < 1 && _difference > -1) {
                break;
            } else {
                LevelManager.Instance.AdjustShipPos(_difference);
            }
        }
    }
}