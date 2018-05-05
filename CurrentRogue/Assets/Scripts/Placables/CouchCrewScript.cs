using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouchCrewScript : MonoBehaviour 
{
	private Rigidbody2D rb;
	private BoxCollider2D col;
	[SerializeField]
	private float speed;
	private string controllerID;
	public string ControllerID { get { return controllerID; } }
	[SerializeField]
	private Camera cam;
	private int couchPlayerID;
	//the player is using an elevator, repairing stuff etc
	private bool isOccupied;
	private bool elevatorIsNear;
	private bool usingElevator;

	private ElevatorScript elevator;

	[SerializeField]
	private GameObject elevatorMenu;
	private IEnumerator useElevator;
	private IEnumerator dmgLoop;

	private Point crewPos;

	//important for crew reassignment
	private float tileDistance;
	private Vector3 previousPos;


	void Start () {
		rb = gameObject.GetComponent <Rigidbody2D> ();
		col = gameObject.GetComponent <BoxCollider2D> ();
		crewPos = gameObject.GetComponent <CrewScript> ().crewPos;

		GetTileDistances ();

		//Vector3 _vect = transform.position;
		//vect.x -= tileDistance;
		previousPos = transform.position;
		//previousPos = _vect;

		Debug.Log (tileDistance);

		//_cam = transform.GetChild (0).gameObject.GetComponent <Camera> ();

		//StartCoroutine (Test ());
	}

	void Update () {
		if (!isOccupied) {
			Vector3 _vect = new Vector3 (Input.GetAxis (controllerID + "-H"), 0);
			rb.MovePosition (transform.position + _vect * speed * Time.deltaTime);
		

			if (elevatorIsNear) {
				if (Input.GetButtonDown (controllerID + "-s")) {
					DoElevatorMenu ();
				}
			} else {
				if (Input.GetButtonDown (controllerID + "-s")) {
					//power system
					TileScript _tile = LevelManager.Instance.Tiles [crewPos];
					if (!_tile.SystemPlacable) {
						ISystem _iSys = _tile.GetSystem ();
						//currently just switches
						_iSys.ReceivePowerUpdate (true);
					}
				}
			}

			if (Input.GetButtonDown (controllerID + "-a")) {
				DoSomeDamage (10);
			} else if (Input.GetButtonDown (controllerID + "-t")) {
				DoSomeDamage (-10);
			}
		} else if (!usingElevator) {
			if (Input.GetButtonDown (controllerID + "-c")) {
				StopSomeDamage ();
			}
		}


		//reassigns crewPos
		if (transform.position.x - previousPos.x > tileDistance) {
			//moved to the right
			UpdateCrewPos (2);

			//crewPos.X += 2;
			//previousPos = LevelManager.Instance.Tiles [crewPos].transform.position;
			//Debug.Log ("crewPos: " + crewPos.X);
		} else if (transform.position.x - previousPos.x < -tileDistance) {
			//moved to the left
			UpdateCrewPos (-2);

			//crewPos.X -= 2;
			//previousPos = LevelManager.Instance.Tiles [crewPos].transform.position;
			//Debug.Log ("crewPos: " + crewPos.X);
		}


		/*
		if (true) {
			Vector3 _vect = Vector3.left;
			rb.MovePosition (transform.position + _vect * speed * Time.deltaTime);
		}
		*/
	}

	public void CouchCrewSetup (string _controllerID, int _couchPlayerID, int _couchCount) {
		controllerID = _controllerID;

		//Debug.Log ("playerID: " + _couchPlayerID);
		//Debug.Log ("couchCount: " + _couchCount);

		couchPlayerID = _couchPlayerID;

		if (_couchCount == 2) {
			TwoPlayerSplit (_couchPlayerID);
		} else if (_couchCount == 3) {
			ThreePlayerSplit (_couchPlayerID);
		} else if (_couchCount == 4) {
			FourPlayerSplit (_couchPlayerID);
		}

		CanvasManager.Instance.CouchCanvas (couchPlayerID, cam);
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

		crewPos = _tile.GridPosition;
		previousPos = _pos;
	}





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

	private void UpdateCrewPos (int _amount) {
		crewPos.X += _amount;
		//Vector3 _vect = LevelManager.Instance.Tiles [crewPos].transform.position;
		//_vect.x -= (tileDistance / 2);
		previousPos = LevelManager.Instance.Tiles [crewPos].transform.position;
		//previousPos = _vect;

		//Debug.Log ("crewPos: " + crewPos.X + ", " + crewPos.Y);
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
}