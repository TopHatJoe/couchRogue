﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Networking;

public class CrewScript : MonoBehaviour, IPlacable
{
	[SerializeField]
	private bool CouchMode;
	private Rigidbody2D rb;

	[SerializeField]
	private bool DoDoorThing;

	private float speed = 1024;

	[SerializeField]
	private int repairSpeed;

    private Point crewPos;
    public Point CrewPos { get { return crewPos; } private set { crewPos = value; } }
	public int CrewOrigin { get; private set; }

	public Stack <Node> path;
	private Vector2 destination;

	[SerializeField]
	private GameObject debugTilePrefab;

	public bool IsLocalCrew { get; private set; }
	public bool IsSelected { get; private set; }

	private TileScript DestinationTile;
	private TileScript DepartTile;

	private PlayerInfo player;
	private int crewIndex;

	private bool reachable;

	[SerializeField]
	private string objStr;

	private string saveStr;

	private IEnumerator movementLoop;
	private IEnumerator repairLoop;

    private IEnumerator dmgLoop;
    [SerializeField]
    private bool friend;


	//crew has manned a tile. this is only true when its not moving //or fighting
	private bool isStationed;



    private HealthScript hScr;
    private float tileDistance;
    private Vector3 previousPos;



	public void PlaceObj (int _index, Point _gridPos, GameObject _originObj) {
        //CouchMode = CasheScript.Instance.CouchMode;

		CrewPos = _gridPos;
        //enterRoom
        RoomScript _room = LevelManager.Instance.Tiles[_gridPos].transform.GetChild(0).GetChild(0).GetComponent<RoomScript>();
        hScr = gameObject.GetComponent<HealthScript>();
        _room.EnterRoom(hScr);
 
		DepartTile = LevelManager.Instance.Tiles [_gridPos].GetComponent <TileScript> ();

		//if (this.gameObject == originObj) {
		saveStr = (objStr + ",4," + CrewPos.X.ToString () + "," + CrewPos.Y.ToString ());
		LevelManager.Instance.parameterList.Add (saveStr);
        //}

        //if (!CouchMode) {
        transform.SetParent(DepartTile.transform.GetChild(2));
        //}

		DepartTile.Manned = true;

		UndoChangesTwo (); //
		//repairLoop = RepairLoop ();

		isStationed = true;

        //starts repairLoop
        //IsStationed (true);
        //hScr = gameObject.GetComponent<HealthScript>();
        previousPos = transform.position;
        GetTileDistances();
		//Initialize ();

		GameManager.Instance.Buy ();

		Debug.Log (crewIndex);
	}


	public void RemoveObj () {
		DepartTile.Manned = false;
		if (DestinationTile != null) {
			DestinationTile.IsDestination = false;
		}

		LevelManager.Instance.parameterList.Remove (saveStr);


		PlacementManager.Instance.Price = 20;

		GameManager.Instance.Sell ();

		Destroy (gameObject);
	}


	private void Start ()
	{
		CouchMode = CasheScript.Instance.CouchMode;

		//DepartTile = transform.parent.parent.GetComponent <TileScript> ();
		//crewPos = DepartTile.GridPosition;

		//sets crew alignment //whose crew 's dat?
		CrewOrigin = CrewPos.Z;

		//mayhap unnessesary //if obj in hangar and void differ.
		if (NetManager.Instance != null) {
            //if (NetManager.Instance.localPlayerID == CrewPos.Z) {
            ShipScript _ship = LevelManager.Instance.Ships[crewPos.Z].GetComponent<ShipScript>();

            if (NetManager.Instance.localPlayerID == _ship.OwnerID) {
				IsLocalCrew = true;
			} else {
				IsLocalCrew = false;
			}

			player = NetManager.Instance.playerList [CrewPos.Z];
			player.crewList.Add (this);
			player.SetCrewIndex ();

            if (!CouchMode)
            {
                transform.GetChild(0).GetComponent<CrewSelect>().AddToHash(IsLocalCrew);
                /*
                TileScript _tile = LevelManager.Instance.Tiles[CrewPos];
                RoomScript _room = _tile.transform.GetChild(0).GetChild(0).GetComponent<RoomScript>();
                if (hScr == null) {
                    Debug.LogError("hScr == null!");
                } else { 
                    _room.EnterRoom(hScr);
                }
                */
			}

			/*
			if (CouchMode) {
				rb = gameObject.GetComponent <Rigidbody2D> ();
			}
			*/
		}

        //to avoid null
        dmgLoop = DmgLoop(0);
	}

    void Update()
    {
        if (!CouchMode) { 
            //ReassignCrewPos();
        }

		/*
		if (CouchMode) {
			if (Input.GetButton ("J00-V")) {
				//Debug.Log ("axis");
				Vector3 _vect = new Vector3 (Input.GetAxis ("J00-V"), 0);
				rb.MovePosition (transform.position + _vect * speed * Time.deltaTime);
			}
		}
		*/
	}

    /*
    private void ReassignCrewPos()
    {
        //Debug.LogError ("crewtrans: " + transform.position.x);
        //Debug.LogError ("crewPrev: " + previousPos.x);
        //Debug.LogError ("tileDist: " + tileDistance);

        //reassigns crewPos
        if (transform.position.x - previousPos.x > tileDistance)
        {
            //moved to the right
            UpdateCrewPos(2, 0);

            //crewPos.X += 2;
            //previousPos = LevelManager.Instance.Tiles [crewPos].transform.position;
            //Debug.Log ("crewPos: " + crewPos.X);
        }
        else if (transform.position.x - previousPos.x < -tileDistance)
        {
            //moved to the left
            UpdateCrewPos(-2, 0);

            //crewPos.X -= 2;
            //previousPos = LevelManager.Instance.Tiles [crewPos].transform.position;
            //Debug.Log ("crewPos: " + crewPos.X);
        }
    }
    */

    //classic edit
    private void UpdateCrewPos(Point _newPos)
    {
        TileScript _newTile = LevelManager.Instance.Tiles[_newPos];
        Point _nextPos = crewPos;

        //if the point crew has moved to is a tween
        if (!_newTile.IsTile) {
            //add the difference to determine movement direction
            int _direction = (crewPos.X - _newPos.X);

            //Point _nextPos = crewPos;
            _nextPos.X -= (_direction * 2);
        } else {

            //for elevators //should really always be called if newPos is not horizontally adjacent to crewPos
            if (_newPos.Y != crewPos.Y) {
                _nextPos = _newPos;
            }

            /*
            //should really always be called if newPos is not horizontally adjacent to crewPos
            if (_newPos.X == (crewPos.X - 2) || _newPos.X == (crewPos.X + 2)) {
               
            } else {
                _nextPos = _newPos;
                Debug.LogError("biuo: " + crewPos.X + ", " + _newPos.X);
            }
            */
        }

        //get room references
        TileScript _prevTile = LevelManager.Instance.Tiles[crewPos];
        TileScript _nextTile = LevelManager.Instance.Tiles[_nextPos];

        RoomScript _prevRoom = _prevTile.transform.GetChild(0).GetChild(0).GetComponent<RoomScript>();
        RoomScript _nextRoom = _nextTile.transform.GetChild(0).GetChild(0).GetComponent<RoomScript>();

        _prevRoom.ChangeRoom(_nextRoom, hScr);
        crewPos = _nextPos;

        Debug.LogError("updated crewPos: " + crewPos.X + ", " + crewPos.Y);


        /*
        TileScript _prevTile = LevelManager.Instance.Tiles[CrewPos];
        RoomScript _room = _prevTile.transform.GetChild(0).GetChild(0).GetComponent<RoomScript>();
       

        //_room.ExitRoom();
        //Debug.LogError("crewPos: " + crewPos.X + ", " + crewPos.Y);

        //crewPos.X += _x;
        //crewPos.Y += _y;

        //Vector3 _vect = LevelManager.Instance.Tiles [crewPos].transform.position;
        //_vect.x -= (tileDistance / 2);



        //stuff here!
        //Debug.LogError ("crewPos: " + crewPos.X + ", " + crewPos.Y + ", " + crewPos.Z);


        TileScript _currTile = LevelManager.Instance.Tiles[_newPos];
        //previousPos = _tile.transform.position;


        //_tile = LevelManager.Instance.Tiles[crewPos];

        RoomScript _nextRoom = _currTile.transform.GetChild(0).GetChild(0).GetComponent<RoomScript>();
        //_room.EnterRoom();
        _room.ChangeRoom(_nextRoom, hScr);
        //previousPos = _vect;

        //Debug.LogError ("crewPos: " + crewPos.X + ", " + crewPos.Y);
        */
    }



	public void Remove ()
	{
		//transform.parent.GetComponent <TileScript> ().RemoteRemoval (false, false, true);
	}

	//Movement

	//
	public void GiveMovementOrders (Point _destination)
	{
		//Debug.Log ("myPos: " + crewPos.X + ", " + crewPos.Y + ", " + crewPos.Z);
		//Debug.Log ("destination: " + _destination.X + ", " + _destination.Y + ", " + _destination.Z);

		TileScript _tile = LevelManager.Instance.Tiles [_destination];

		if (_tile.Walkable) {

			//bool _foundDestination = false;

			/*
			if (_tile.Manned) {
				//Debug.Log ("manned");
			} 

			if (_tile.IsDestination) {
				//Debug.Log ("is destined");
			}
			*/


			if (!_tile.Manned && !_tile.IsDestination) {
				//removes previous destination
				reachable = ReachabilityCheck (_tile);

				//_foundDestination = true;
				GiveGoAhead (true, _tile);
			} else {

				//checks rest of the room
				//TileScript _roomOri = _tile.FindRoomOrirgin ();
				//TileScript _roomOri = _tile.transform.GetChild (0).GetChild(0).GetComponent<RoomScript> ().OriginObj.GetComponent<RoomScript> ();
			
				//gets the originObj of any room
				RoomScript _roomOri = _tile.transform.GetChild (0).GetChild (0).GetComponent<RoomScript> ().OriginObj.GetComponent<RoomScript> ();
				//checks for the best available alternative
				_roomOri.RoomComponentFree (_tile, this, false);


				/*
				Point _oriPoint = _roomOri.GridPosition;

				for (int i = 0; i < _roomOri.GetRoomLength (); i += 2) {
					Point _point = new Point (_oriPoint.X + i, _oriPoint.Y, _oriPoint.Z);
					TileScript _otherTile = LevelManager.Instance.Tiles [_point];

					if (!_otherTile.Manned && !_otherTile.IsDestination && _otherTile.Walkable) {
						//removes previous destination
						reachable = DoDestination (_otherTile);

						_foundDestination = true;
						break;
					}
				}
				*/
			}
		}
	}


	public void GiveGoAhead (bool _foundDestination, TileScript _tile)
	{
		reachable = ReachabilityCheck (_tile);

		if (_foundDestination == true) {
			//Movement 

			if (reachable) {
				Movement (CrewPos, DestinationTile.GridPosition);
			} else {
				Debug.Log ("unreachable");
			}
		} else {
			Debug.Log ("room is full!");
		}
	}


	private bool ReachabilityCheck (TileScript _tile)
	{
		//only if the new destination is reachable, the path will be altered
		Stack<Node> _path = AStar.GetPath (CrewPos, _tile.GridPosition);
		if (_path != null) {

			//!!!
			//StopCoroutine (Move ());

			if (movementLoop != null) {
				StopCoroutine (movementLoop);
			}

			ResetDestination ();

			DestinationTile = _tile;
			DestinationTile.IsDestination = true;

			return true;
		} else {
			return false;
		}
	}

	public void ResetDestination ()
	{
		if (DestinationTile != null) {
			if (DestinationTile.transform.GetChild (5).childCount > 0) {
				//Debug.Log ("eh");
				GameObject _obj = DestinationTile.transform.GetChild (5).GetChild (0).gameObject;
				Destroy (_obj);
			}
			//Debug.Log ("ah");


			DestinationTile.IsDestination = false;

			//DestinationTile = null;
		}
	}


	private void Movement (Point _start, Point _goal)
	{
		Vector3 _startVec = PointToVector (_start);
		Vector3 _goalVec = PointToVector (_goal);

		player.SyncMovement (_startVec, _goalVec, crewIndex);
	}

	public void SyncedMovement (Vector3 _startVec, Vector3 _goalVec) 
	{
		Point _start = VectorToPoint (_startVec);
		Point _goal = VectorToPoint (_goalVec);

        //CrewPos = _start;
        UpdateCrewPos(_start);

		//DestinationTile = LevelManager.Instance.Tiles [_goal];
		TileScript _destinationTile = LevelManager.Instance.Tiles [_goal];

		path = AStar.GetPath (CrewPos, _destinationTile.GridPosition);

		//checks if path was found
		if (path != null) {
			DestinationTile = _destinationTile;

			if (IsLocalCrew) {
				Color32 _color = Color.green;
				DisplayPoint (DestinationTile.WorldPostion, _color);
			}

			//un-man rooms on departure
			DepartTile = LevelManager.Instance.Tiles [CrewPos];
			DepartTile.Manned = false;

            Node _node = path.Pop();
            UpdateCrewPos(_node.GridPosition);
            destination = _node.TileTransform;

			//CrewPos = path.Peek ().GridPosition;
			//destination = path.Pop ().TileTransform;

			if (movementLoop != null) {
				StopCoroutine (movementLoop);
			}

			movementLoop = Move ();

			//StartCoroutine (Move ());
			StartCoroutine (movementLoop);
		} else {
			//ResetDestination ();

			//actually dont do reset
		}
	}
	//

	public void DisplayPoint (Vector3 worldPos, Color32 color)
	{
		GameObject debugTile = (GameObject)Instantiate (debugTilePrefab, worldPos, Quaternion.identity);

		debugTile.transform.SetParent(DestinationTile.transform.GetChild (5));

		debugTile.GetComponent<SpriteRenderer> ().color = color;
	}


	//DEBUG
	private void LogPath (Stack <Node> _path)
	{
		foreach (Node _node in _path) {
			Debug.Log ("PathLogger: " + _node.GridPosition.X + ", " + _node.GridPosition.Y);
		}
	}


	private IEnumerator Move () {
		//stops repairLoop
		IsStationed (false);

		//Debug.Log ("not null 0");

		while (path.Count > 0) {
			transform.position = Vector2.MoveTowards (transform.position, destination, speed * Time.deltaTime);

			yield return new WaitForSeconds (0.002f);

			Vector2 _pos = transform.position;

			if (Vector2.Distance (_pos, destination) < 0.005f && path.Count > 0) {
			//if (transform.position == destination && path.Count > 0) {
				
                Node _node = path.Pop ();
                //crewPos = path.Peek ().GridPosition;
                //CrewPos = _node.GridPosition;
                UpdateCrewPos(_node.GridPosition);
                //destination = path.Pop ().TileTransform;
				destination = _node.TileTransform;


				//delay movement at doors
				if (_node.HasDoor) {
					if (!LevelManager.Instance.Tiles [CrewPos].DoorOpen) {
						//later changed to while !doorOpen -> wait
						//door opens when destroyed

						//checks if crew has door permissions
						if (CrewOrigin == _node.GridPosition.Z && DoDoorThing) {
							_node.TileRef.doorRef.LetCrewThrough ();
							//_node.TileRef.doorRef.OpenForCrew ();
						} else {
							yield return new WaitForSeconds (2);
						}
					}
				}

				if (path.Count == 0) {
					Debug.Log ("not null 1");

					if (DestinationTile == null) {
						Debug.LogError ("destinationTile == NULL!!!");
					}

					//room reaction on crew arrival
					DestinationTile.Manned = true;

					Debug.Log ("not null 2");

					//starts repairLoop
					
                    IsStationed (true);

					Debug.Log ("not null 3");

					transform.SetParent (DestinationTile.transform.GetChild (2));

					Debug.Log ("not null 4");

					//remove ui destination
					ResetDestination ();
					//UndoChanges (); 

					//Debug.Log ("not null 2");

					CrewRebirth (CrewPos, true);
					//Debug.Log ("final destination");
					yield break;
					//if (!LevelManager.Instance.Tiles [crewPos].IsTile) {
				//} else if (path.Peek ().IsTile) {
				} else if (!_node.IsTile && path.Peek ().IsTile) {
                    //doesnt work for y movement though

                    //Debug.LogError("removed tween jump");
                    CrewPos = path.Peek ().GridPosition;
				} 

				/*
				else if (path.Peek ().HasDoor) {
					yield return new WaitForSeconds (3);
				}
				*/

				//reassign obj
				CrewRebirth (CrewPos, false);

				//Debug.Log ("crewPos: " + crewPos.X + ", " + crewPos.Y);
			}
		}
	}



    //dmg loops
    private void DoSomeDamage(int _amount)
    {
        Debug.Log ("gonna do some damage!");
        dmgLoop = DmgLoop(_amount);
        StartCoroutine(dmgLoop);
    }

    private void StopSomeDamage()
    {
        Debug.Log ("gonna stop doin some damage!");

        StopCoroutine(dmgLoop);

        //isOccupied = false;
    }


    private IEnumerator DmgLoop(int _amount)
    {
        //isOccupied = true;
        TileScript _tile = LevelManager.Instance.Tiles[CrewPos];

        //RoomScript _room = LevelManager.Instance.Tiles [crewPos].transform.GetChild (0).GetChild (0).GetComponent <RoomScript> ();

        HealthScript[] _hScrArr = _tile.GetHScripts(_amount, _amount, _amount);

        yield return new WaitForSeconds(1f);

        bool _hasSys = false;
        if (_hScrArr[1] != null)
        {
            _hasSys = true;
        }

        bool _hasSubSys = false;
        if (_hScrArr[2] != null)
        {
            _hasSubSys = true;
        }

        while (true)
        {
            //_tile.TakeCrewDamage (_amount, 0, 0);
            _hScrArr[0].TakeCrewDamage(_amount);
            if (_hasSys)
            {
                _hScrArr[1].TakeCrewDamage(_amount - 2);
            }
            if (_hasSubSys)
            {
                _hScrArr[2].TakeCrewDamage(_amount + 2);
            }

            //Debug.Log ("took " + _amount + " damage!");



            //this should happen if all obj on that tile are completely destroyed or repaired...
            if (_amount > 0)
            {
                if (_hScrArr[0].IsFullyDamaged)
                {
                    if (_hasSys)
                    {
                        if (_hScrArr[1].IsFullyDamaged)
                        {
                            if (_hasSubSys)
                            {
                                if (_hScrArr[2].IsFullyDamaged)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (_hasSubSys)
                    {
                        if (_hScrArr[2].IsFullyDamaged)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (_amount < 0)
            {
                if (_hScrArr[0].IsFullyRepaired)
                {
                    if (_hasSys)
                    {
                        if (_hScrArr[1].IsFullyRepaired)
                        {
                            if (_hasSubSys)
                            {
                                if (_hScrArr[2].IsFullyRepaired)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (_hasSubSys)
                    {
                        if (_hScrArr[2].IsFullyRepaired)
                        {
                            break;
                        }
                    }
                    else
                    {
                        //Debug.Log ("break!");
                        break;
                    }
                }
            }



            yield return new WaitForSeconds(0.2f);
        }

        //isOccupied = false;
    }








	private void CrewRebirth (Point _crewPos, bool _isFin)
	{
		//needs to later be changend so we can differenciate between stationed passing and foe crew -> bool
		TileScript _tile = LevelManager.Instance.Tiles [_crewPos];
		transform.SetParent (_tile.transform.GetChild (2));
	}


	private Vector3 PointToVector (Point _point)
	{
		Vector3 _vector = new Vector3 (_point.X, _point.Y, _point.Z);

		return _vector;
	}

	private Point VectorToPoint (Vector3 _vector)
	{
		Point _point = new Point (Mathf.RoundToInt (_vector.x), Mathf.RoundToInt (_vector.y), Mathf.RoundToInt (_vector.z));

		return _point;
	}

	public void SetCrewIndex (int _index)
	{
		crewIndex = _index;

		//Debug.Log ("crew reports for duty: " + crewPos.Z + ", " + crewIndex);
	}


    /* 21.04.18
	public void UpdateHealth (int _amount) {
		Debug.Log ("took Damage");
	}
	*/

    private void IsStationed(bool _value)
    {
        //isStationed = !isStationed;
        isStationed = _value;

        if (isStationed) {
            if (friend) {
                DoSomeDamage(-10);
            }
            else {
                DoSomeDamage(10);
            }
        } else {
            StopSomeDamage();
        }


        /*
		//Debug.Log ("stuff");
		RoomScript _room = transform.parent.parent.GetChild (0).GetChild (0).GetComponent <RoomScript> ().GetRoomOrigin ();


		if (isStationed) {
			if (_room.IsDamaged ()) {
				//Debug.Log ("start");

				//RoomScript _room = DepartTile.transform.GetChild (0).GetChild (0).GetComponent <RoomScript> ();

				//repairLoop = RepairLoop (); //-> is setup in setup
				StartCoroutine (repairLoop);
			}
		} else {
			if (repairLoop != null) {
				//Debug.Log ("stop");
				StopCoroutine (repairLoop);
			}
		}
        */
	}



	//Repair

	/*
	//private IEnumerator RepairLoop (RoomScript _room) {
	private IEnumerator RepairLoop () {
		if (DestinationTile == null) {
			Debug.LogError ("desyinationTile == NULL!");
		} else {
			Debug.Log ("dest: " + DestinationTile.GridPosition.X + ", " + DestinationTile.GridPosition.Y);
			RoomScript _room = DestinationTile.transform.GetChild (0).GetChild (0).GetComponent <RoomScript> ();
			//RoomScript _room = LevelManager.Instance.Tiles[crewPos].transform.GetChild (0).GetChild (0).GetComponent <RoomScript> ();

			//and obj is damaged //or all obj have a limit and crew just does its thing...
			while (true) {
				//while (isStationed) {
				_room.GetRepaired (repairSpeed);
				yield return new WaitForSeconds (0.2f);
			}
		}
	}

	//if something is damaged and the crew is stationed it starts repairing
	public void RepairManager (bool _isDamaged) {
		//Debug.Log ("crew here, what can i do for you sir?");

		if (repairLoop != null) {
			//Debug.Log ("stopped by others");
			StopCoroutine (repairLoop);
		}

		if (isStationed && _isDamaged) {
			//Debug.Log ("started by others");
			StartCoroutine (repairLoop);
		}
	}
	*/


	//comment out to get the points in the script that were altered in the great cooperation of april 6th
	//public void UndoChanges () {
		
	public void UndoChangesTwo () {
		
	}

	public GameObject GetOriginObj () {
		return gameObject;
	}

	public void UpdateHealthState (bool _isFullyDamaged, bool _isFullyRepaired) {

	}





    private void GetTileDistances() {
        //theres probably a more elegant soltion to this... but fuggid!
        Point _point0 = new Point(0, 0, 0);
        Point _point1 = new Point(1, 0, 0);

        Vector3 _vect0 = LevelManager.Instance.Tiles[_point0].transform.position;
        Vector3 _vect1 = LevelManager.Instance.Tiles[_point1].transform.position;

        float _distance = _vect1.x - _vect0.x;
        tileDistance = (_distance + 0.1f);
    }
}