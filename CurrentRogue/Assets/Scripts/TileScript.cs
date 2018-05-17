using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour 
{
	private int playerID = 0;
	public int PlayerID { get { return playerID; } }

	//for dmg
	public Dictionary <int, HealthScript> LocalObjDict = new Dictionary <int, HealthScript> ();

	//public string RoomString;

	[SerializeField]
	private BoxCollider2D emptyCol;

	//if gridpos.x is even: its a tile, if not its a tween
	[SerializeField]
	public bool IsTile;
	[SerializeField]
	private bool IsWeaponSlot;
	//[SerializeField]
	//private int weaponSize;

	public Point GridPosition { get; private set; }

	//trying to fix BlockDoor
	//public int ForcedGCost = 0;
	//public bool DoorPassed { get; set; }
	public bool HasDoorSlot { get; set; }
	public bool HasDoor { get; set; }
	public bool DoorOpen { get; set; }

	public bool IsEmpty { get; set; }
	private bool walkable; //{ get; set; }
	public bool Walkable { get { return walkable; } set { walkable = value; emptyCol.enabled = !value; } }
	public bool SystemPlacable { get; set; }
	public bool SysIsPowered { get; set; }
	public bool HasSubSysSlot { get; set; }
	public bool HasSubSys { get; set; }
	public bool SubSysPlacable { get; set; }
	public bool HasAccessor { get; set; }
	//mayhap simply add small, medium, large prefix
	public bool WeaponPlacable { get; set; }

	public bool Manned { get; set; }
	public bool IsDestination { get; set; }

	public bool HasElevator { get; set; }
	private bool ElevatorHere { get; set; }
	//public bool ElevatorHere { get { return elevatorHere; } set { elevatorHere = value; } }
	public bool Passable { get; set; }
	public bool OnFire { get; set; }
	//public bool Targeted { get; set; }

	public DoorScript doorRef;

	/*
	private bool leftDoorOpen;
	public bool LeftDoorOpen 
	{ get { return leftDoorOpen; } set { leftDoorOpen = value; } }
	private bool rightDoorOpen;
	public bool RightDoorOpen 
	{ get { return rightDoorOpen; } set { rightDoorOpen = value; } }
	*/

	private bool tileIsFree;


	//saves the data passed into the parameterList locally, and seperately for every objType
	private string localRoom;
	private string localSystem;
	private string localCrew;
	//private string localDanger;


	//private int objType;
	//private float[] RoomID;
	private float[] SystemID;
	private float[] CrewID;
	private float[] DangerID;
	//public float leftDoorID;
	//public float rightDoorID;

	[SerializeField]
	private bool isEmptySpace;

	public bool Debugging { get; set; }

	private Color32 fullColor = Color.red;
	private Color32 emptyColor = Color.green;

	//private SpriteRenderer spriteRenderer;
	//public SpriteRenderer SpriteRenderer { get; set; }

	public Vector2 WorldPostion 
	{ get { return new Vector2 (transform.position.x + GetComponent<SpriteRenderer> ().bounds.size.x / 2, transform.position.y + GetComponent<SpriteRenderer> ().bounds.size.y / 2); } }
	//!the additional stuff sets the point to the middle of the tile!	
	//{ get { return new Vector2 (transform.position.x, transform.position.y); } }


	void Start () 
	{
		
	}

	public void Setup (Point gridPos, Vector3 worldPos, Transform parent, bool isTile)
	{
		IsTile = isTile;

		if (IsTile) {
			if (isEmptySpace) {
				//swapped
				walkable = false;
				Passable = true;
				IsEmpty = true;
				SystemPlacable = false;
				HasSubSys = false;

				WeaponPlacable = false;

				Manned = true;
				IsDestination = true;

				HasAccessor = false;
				HasElevator = false;
				OnFire = false;

				HasDoorSlot = false;
				HasDoor = false;
				DoorOpen = true;
			} else {
				walkable = false;
				Passable = false;
				IsEmpty = false;
				SystemPlacable = false;
				HasSubSys = false;

				WeaponPlacable = false;

				Manned = true;
				IsDestination = true;

				HasAccessor = false;
				HasElevator = false;
				OnFire = false;

				HasDoorSlot = false;
				HasDoor = false;
				DoorOpen = false;
			}

			if (IsWeaponSlot) {
				WeaponPlacable = true;
			}

			this.GridPosition = gridPos;
			transform.position = worldPos;
			//sets gameObject grid as newTiles parent
			transform.SetParent (parent);

			//adds newTile to Tiles Dictionary
			LevelManager.Instance.Tiles.Add(gridPos, this);
		} 

		if (!IsTile) {
			//could cause issues
			walkable = true;

			Passable = true;
			IsEmpty = true;
			SystemPlacable = false;
			HasSubSys = false;

			WeaponPlacable = false;

			Manned = true;
			IsDestination = true;

			HasAccessor = false;
			HasElevator = false;
			OnFire = false;

			HasDoorSlot = false;
			HasDoor = false;
			DoorOpen = true;

			this.GridPosition = gridPos;
			transform.position = worldPos;
			//sets gameObject grid as newTiles parent
			transform.SetParent (parent);

			//adds newTile to Tiles Dictionary
			LevelManager.Instance.Tiles.Add(gridPos, this);
		}

		ElevatorHere = false;
		SysIsPowered = false;
	}

	private void OnMouseOver ()
	{
		if (IsTile) {
			if (!IsWeaponSlot) {

				//if the pointer is over the btns or ClickedBtn equals null it won't place a room
				if (!EventSystem.current.IsPointerOverGameObject ()) {
					if (GameManager.Instance.ClickedBtn != null) {
						int objType = GameManager.Instance.ClickedBtn.objType;
						string _objStr = PlacementManager.Instance.objStr;


						//Room
						if (objType == 0) {
							if (Input.GetMouseButtonDown (0)) {
								string _objDimStr = GameManager.Instance.ClickedBtn.ObjDim;
								int[] _objDim = ReadDimensions (_objDimStr);

								if (TheresRoom (_objDim, 0, 0, 0)) {
									//PlaceRoom
									PlaceObj (_objStr);
								}
							}
						}


					//System
					else if (objType == 1) {

							if (Input.GetMouseButtonDown (0)) {
								string _objDimStr = GameManager.Instance.ClickedBtn.ObjDim;
								char[] _objDimArr = _objDimStr.ToCharArray ();
								int[] _objDim = new int[_objDimArr.Length];
								for (int i = 0; i < _objDimArr.Length; i++) {
									_objDim [i] = int.Parse (_objDimArr [i].ToString ());
								}

								if (TheresRoom (_objDim, 0, 0, 1)) {
									//PlaceSystem 
									PlaceObj (_objStr);
								}
							}
						} 

					//SubSys
					else if (objType == 2) {
							if (SubSysPlacable) {
								if (Input.GetMouseButtonDown (0)) {
									//PlaceSubSystem
									PlaceObj (_objStr);
								}
							}
						} 

					//Accessors
					else if (objType == 3) {
							if (SystemPlacable || SubSysPlacable) {
								if (Input.GetMouseButtonDown (0)) {
									//PlaceAccessor
									PlaceObj (_objStr);
								}
							}
						}

					//Crew
					else if (objType == 4) {
							if (!Manned && !Debugging) {
								ColorTile (emptyColor);
							}

							if (Manned && !Debugging) {
								ColorTile (fullColor);
							} else if (Input.GetMouseButtonDown (0)) {
								//PlaceCrew
								PlaceObj (_objStr);
							}
						} 

					//DEBUG

					//Dangers
					else if (objType == 5) {
							if (walkable && !Debugging && !OnFire) {
								ColorTile (emptyColor);
							}

							if (!walkable && !Debugging && OnFire) {
								ColorTile (fullColor);
							} else if (Input.GetMouseButtonDown (0) && !OnFire) {
								//PlaceDanger ();
								PlaceObj (_objStr);
							}
						}

					//END DEBUG

					//Targeting
					else if (objType == 6) {
							if (Input.GetMouseButtonDown (0))
					//set target
								Debug.LogError ("gun finder classic!");
								SetTarget (_objStr, 0);
						}  
					}


					if (Input.GetButtonUp ("Fire1") && CrewSelect.currentlySelected.Count > 0) {
						foreach (CrewSelect _selected in CrewSelect.currentlySelected) {
							_selected.MovementOrders (GridPosition);
						}

						LevelManager.Instance.DragSelectRef.gameObject.SetActive (true);
					}

				//Removing Objects  //if in Hangar should be added
				else if (OnFire && Input.GetMouseButtonDown (1)) {
						//Debug.Log ("REMOVE DANGER");
						//RemoveDanger ();
						RemoveObj (4);
						//} else if (Manned && Input.GetMouseButtonDown (1)) {
					} else if (Manned && walkable && Input.GetMouseButtonDown (1)) {
						//Debug.Log ("REMOVE CREW");
						//RemoveCrew ();
						RemoveObj (2);
						//} else if (!SystemPlacable && Input.GetMouseButtonDown (1)) {
					} else if (HasSubSysSlot && !SubSysPlacable && walkable && Input.GetMouseButtonDown (1)) {
						//Debug.Log ("REMOVE SUBSYS");
						//RemoveSubSystem ();
						RemoveObj (6);
					} else if (HasAccessor && Input.GetMouseButtonDown (1)) {
						//Debug.Log ("REMOVE ACCESSOR");
						//RemoveSubSystem ();
						RemoveObj (6);
					} else if (!SystemPlacable && walkable && Input.GetMouseButtonDown (1)) {
						//Debug.Log ("REMOVE SYSTEM");
						//RemoveSystem ();
						RemoveObj (1);
					} else if (walkable && Input.GetMouseButtonDown (1)) {
						//Debug.Log ("REMOVE ROOM");
						//RemoveRoom ();
						RemoveObj (0);
					}
				}
			} else {
				if (!EventSystem.current.IsPointerOverGameObject ()) {
					if (GameManager.Instance.ClickedBtn != null) {
						int objType = GameManager.Instance.ClickedBtn.objType;
						string _objStr = PlacementManager.Instance.objStr;

						//if (objType > 6) {
							if (objType == 7) {
								if (WeaponPlacable) {
									if (Input.GetMouseButtonDown (0)) {
										Debug.Log ("weaponPlaced");
										PlaceObj (_objStr);
									} 
								} 
							}
						//}
					} 

				 	if (!WeaponPlacable && Input.GetMouseButtonDown (1)) {
						Debug.Log ("weaponDisPlaced");
						RemoveObj (7);
					}
				}
			}
		}
	}


	private int[] ReadDimensions (string _objDimStr) {
		char[] _objDimArr = _objDimStr.ToCharArray ();
		int[] _objDim = new int[_objDimArr.Length];
		for (int i = 0; i < _objDimArr.Length; i++) {
			_objDim [i] = int.Parse (_objDimArr [i].ToString ());
		}

		return _objDim;
	}

	private bool TheresRoom (int[] _objDim, int _count, int _y, int _type)
	{
		//Debug.Log ("length: " + _objDim.Length);
		int _gap = 0;

		while (_count < _objDim.Length) {
			
			if (_objDim [_count] == 0) {
				_count++;
				_gap++;

			} else {
				//Debug.Log ("objDim" + _count + ": " + _objDim[_count]);
				int _int = _objDim [_count];
				for (int x = 0; x < _int; x++) {
					Point _point = new Point (GridPosition.X + (x * 2) + (_gap * 2), GridPosition.Y + _y, GridPosition.Z);
					TileScript _tile = LevelManager.Instance.Tiles [_point].GetComponent<TileScript> ();

					if (_type == 0) {
						if (!_tile.IsEmpty) {
							//Debug.Log ("no space!");
							return false;
						}
					} else if (_type == 1) {
						if (!_tile.SystemPlacable) {
							//Debug.Log ("no Sys space!");
							return false;
						}
					}

					if (x == _objDim [_count] - 1) {
						_y--;
						//reset gap
						_gap = 0;
						_count++;
					}
				}
			}
		}

		return true;
	}

	//?
	private void OnMouseExit ()
	{
		if (!Debugging) 
		{
			ColorTile (Color.white);
		}
	}

	//?
	private void ColorTile (Color newColor)
	{
		//spriteRenderer.color = newColor;
	}


	private void PlaceObj (string _objStr, int _type = 10) {
		GameObject _obj = (GameObject)Instantiate (LevelManager.Instance.ObjDict [_objStr], transform.position, Quaternion.identity);
		IPlacable _placable = _obj.GetComponent <IPlacable> ();
		_placable.PlaceObj (0, this.GridPosition, _obj);

		if (_type < 10) {
			LocalObjDict.Add (_type, _obj.GetComponent <HealthScript> ());
			//Debug.Log ("type: " + _type);
		}
	}

	/*
	//public void PlaceRoom (int tmpObjRef, int width)
	public void PlaceRoom (string _objStr)
	{
		GameObject _obj = (GameObject)Instantiate (LevelManager.Instance.ObjDict [_objStr], transform.position, Quaternion.identity);
		IPlacable _placable = _obj.GetComponent <IPlacable> ();
		_placable.PlaceObj (0, this.GridPosition, _obj);
	}

	


	private void PlaceSystem (string _objStr)
	{
		GameObject _obj = (GameObject)Instantiate (LevelManager.Instance.ObjDict [_objStr], transform.position, Quaternion.identity);
		IPlacable _placable = _obj.GetComponent <IPlacable> ();
		_placable.PlaceObj (0, this.GridPosition, _obj);
	}

	private void PlaceCrew (string _objStr)
	{
		GameObject _obj = (GameObject)Instantiate (LevelManager.Instance.ObjDict [_objStr], transform.position, Quaternion.identity);
		IPlacable _placable = _obj.GetComponent <IPlacable> ();
		_placable.PlaceObj (0, GridPosition, null);
	}
	*/

	//DEBUG
	/*
	private void PlaceDanger ()
	{
		//Debug.Log ("Danger Placed at " + GridPosition.X + ", " + GridPosition.Y);

		LevelManager.Instance.parameterList.Add (GridPosition.X.ToString() + "," + GridPosition.Y.ToString() + ",4,6,0");
		localDanger = (GridPosition.X.ToString() + "," + GridPosition.Y.ToString() + ",4,6,0");


		DangerID = new float[3];

		//DangerID [0] = PlacementManager.AssignDangerID ();
		DangerID [0] = PlacementManager.Instance.AssignDangerID ();
		DangerID [1] = GridPosition.X;
		DangerID [2] = GridPosition.Y;

		GameObject obj = (GameObject)Instantiate (PlacementManager.Instance.ObjectPrefab, transform.position, Quaternion.identity);

		//obj.transform.SetParent (transform);
		obj.transform.SetParent (transform.GetChild (3));

		OnFire = true;

		GameManager.Instance.Buy ();
	}
	*/

	//END DEBUG

	private void SetTarget (string _objStr, int _gunID)
	{
		Vector3 targetPos = new Vector3 (GridPosition.X, GridPosition.Y, GridPosition.Z);

		//GameObject _gun = PlacementManager.Instance.GunObj;
		//Point _gunPos = _gun.GetComponent <WeaponScript> ().GridPos;
		//Vector3 _gunVect = new Vector3 (_gunPos.X, _gunPos.Y, _gunPos.Z);

		Debug.LogError ("gun finder!");
		//Point _gunPoint = PlacementManager.Instance.GunPoint;
		ShipScript _ship = transform.parent.parent.GetComponent <ShipScript> ();
		Point _gunPoint = _ship.WeaponList [_gunID].GridPos;

		Vector3 _gunPos = new Vector3 (_gunPoint.X, _gunPoint.Y, _gunPoint.Z);


		if (NetManager.Instance != null) {
			//Debug.Log (targetPos);

			//generates a random angle for radialBombardement
			float _angle = Random.Range (0f, Mathf.PI * 2f);
			//generates hit probability 
			int _prob = Random.Range (1, 100);

			//if theres an error: check playerInfo.Targeting
			NetManager.Instance.Targeting (targetPos, _gunPos, _angle, _prob);
		} else {
			//PlaceTarget ();
			PlaceObj (_objStr);
			//Debug.Log ("gfuhdahfuldhjkl");
		}
	}

	//public void PlaceTarget (Point _gunPoint, float _angle, int _prob)
	public void PlaceTarget (Point _gunPoint, float _angle)
	{
		GameObject _obj = (GameObject)Instantiate (PlacementManager.Instance.TargetObj, transform.position, Quaternion.identity);

		TargetScript _targetScr = _obj.GetComponent <TargetScript> ();
		_targetScr.PlaceObj (0, this.GridPosition, _obj);
		//Debug.Log ("target Placer");

		Debug.LogError ("get gun is commented out");
		_targetScr.GetGun (_gunPoint, _angle);

		//_obj.transform.SetParent (transform);


		//_obj.GetComponent <TargetScript> ().GetGun (_gunPoint);

		//TakeDamage (false);
	}

	/*
	private void PlaceSubSystem ()
	{
		GameObject _obj = (GameObject)Instantiate (PlacementManager.Instance.ObjectPrefab, transform.position, Quaternion.identity);
		IPlacable _placable = _obj.GetComponent <IPlacable> ();
		_placable.PlaceObj (0, this.GridPosition, _obj);
	}

	private void PlaceAccessor ()
	{
		GameObject _obj = (GameObject)Instantiate (PlacementManager.Instance.ObjectPrefab, transform.position, Quaternion.identity);
		IPlacable _placable = _obj.GetComponent <IPlacable> ();
		_placable.PlaceObj (0, this.GridPosition, _obj);
	}
	*/

	/*
	//this entire section is utterly pointless!?

	//the bool merely indicates that the function was called by FindRoomOrigin ()
	//the entire function might be changed debending on the dmg design. (the damage system)
	private void TakeDamage (bool isRoomOrigin)
	{
		if (!isRoomOrigin) {
			//checking all objs on the tile itself
			for (int i = 0; i < transform.childCount; i++) {
				//if (transform.GetChild (i) != null) {
					//if (transform.GetChild (i).GetChild (0) != null) {
				if (transform.GetChild (i).childCount > 0) {
					if (transform.GetChild (i).GetChild (0).GetComponent <HealthScript> () != null) {
						//the amount of dmg taken is currently hard coded...
						transform.GetChild (i).GetChild (0).GetComponent <HealthScript> ().TakeDamage (10);
					}
				} else {
					Debug.Log ("obj was castrated");
				}
				//}
			}

			//checking room origin
			if (FindRoomOrirgin () != null) {
				FindRoomOrirgin ().TakeDamage (true);
			}

			GameManager.Instance.Buy ();
		} else {
			if (transform.GetChild (0).transform.GetChild (0).GetComponent <HealthScript> () != null) {
				//the amount of dmg taken is currently hard coded...
				//transform.GetChild (0).GetComponent <HealthScript> ().TakeDamage (10);
				transform.GetChild (0).transform.GetChild (0).GetComponent <HealthScript> ().TakeDamage (10);
			}
		}
	}

	//private void FindRoomOrirgin ()
	public TileScript FindRoomOrirgin ()
	{
		if (localRoom != null) {
			string[] _infoString = localRoom.Split (',');

			int _x = int.Parse (_infoString [0]);
			int _y = int.Parse (_infoString [1]);

			Point _point = new Point (_x, _y, GridPosition.Z);

			return LevelManager.Instance.Tiles [_point];
		} else {
			Debug.LogError ("localRoom is null!");
			return null;
		}
	}

	public int GetRoomLength ()
	{
		if (localRoom != null) {
			string[] _infoString = localRoom.Split (',');

			int _width = int.Parse (_infoString [4]);
			Debug.Log ("roomWidth: " + _width);

			return _width;
		} else {
			Debug.LogError ("roomWidth is null!");
			return 3;
		}
	}
	*/

	//Remove FUNctions

	//public void RemoteRemoval (bool _isRoom, bool _isSystem, bool _isCrew)
	public void RemoteRemoval (int _objType)
	{
		RemoveObj (_objType);

		/*
		if (_isRoom) {
			//RemoveRoom ();
		} else if (_isSystem) {
			//RemoveSystem ();
		} else if (_isCrew) {
			//RemoveCrew ();
		} else {
			Debug.Log ("huh");
		}
		*/
	}

	private void RemoveObj (int _objType) {
		//GameObject _obj = transform.GetChild (0).GetChild (0).gameObject;
		if (_objType != 7) {
			GameObject _obj = transform.GetChild (_objType).GetChild (0).gameObject;
			IPlacable _placable = _obj.GetComponent <IPlacable> ();
			_placable.RemoveObj ();
		} else if (_objType == 7) {
			GameObject _obj = transform.GetChild (0).gameObject;
			IPlacable _placable = _obj.GetComponent <IPlacable> ();
			_placable.RemoveObj ();
		}
	}

	/*
	private void RemoveRoom ()
	{
		GameObject _obj = transform.GetChild (0).GetChild (0).gameObject;
		IPlacable _placable = _obj.GetComponent <IPlacable> ();
		_placable.RemoveObj ();
	}


	private void RemoveSystem ()
	{
		GameObject _obj = transform.GetChild (1).GetChild (0).gameObject;
		IPlacable _placable = _obj.GetComponent <IPlacable> ();
		_placable.RemoveObj ();
	}


	private void RemoveCrew ()
	{
		GameObject _obj = transform.GetChild (2).GetChild (0).gameObject;
		IPlacable _placable = _obj.GetComponent <IPlacable> ();
		_placable.RemoveObj ();
	}
	*/

	/*
	private void RemoveDanger ()
	{
		//Debug.Log ("Danger Removed at " + GridPosition.X + ", " + GridPosition.Y);

		LevelManager.Instance.parameterList.Remove (localDanger);

		//meyhap need to redet DangerID...

		//GameObject obj = (GameObject)this.GetComponent<GameObject> ().activeInHierarchy;
		//GameObject obj = transform.Find("Fire(Clone)").gameObject;

		GameObject _obj = transform.GetChild (3).GetChild (0).gameObject;

		//obj.SetActive (false);
		Destroy (_obj);

		OnFire = false;

		//probably needs to be replaced
		PlacementManager.Instance.Price = 10;

		GameManager.Instance.Sell ();
	}
	*/


	private void RemoveSubSystem ()
	{
		GameObject _obj = transform.GetChild (6).GetChild (0).gameObject;
		IPlacable _placable = _obj.GetComponent <IPlacable> ();
		_placable.RemoveObj ();
	}

	
	public void SetPrefabs (int price, int objWidth, int objType, GameObject objectPrefab)
	{
		PlacementManager.Instance.Price = price;
		PlacementManager.Instance.objWidth = objWidth;
		PlacementManager.Instance.objType = objType;
		PlacementManager.Instance.ObjectPrefab = objectPrefab;
	}


	public void PlaceObject (int objType, string _objDimStr, string _objStr)
	{
		//Room
		if (objType == 0) {
			int[] _objDim = ReadDimensions (_objDimStr);

			if (TheresRoom (_objDim, 0, 0, 0)) {
				//PlaceRoom
				PlaceObj (_objStr, 0);
			}
		}


		//System
		else if (objType == 1) {
			int[] _objDim = ReadDimensions (_objDimStr);
			
			if (TheresRoom (_objDim, 0, 0, 1)) {
				//PlaceSystem
				PlaceObj (_objStr, 1);
			}
		} 

		//SubSys
		else if (objType == 2) {
			if (SubSysPlacable) {
				//PlaceSubSystem
				PlaceObj (_objStr, 2);
			}
		} 

		//Accessors
		else if (objType == 3) {
			if (SystemPlacable || SubSysPlacable) {
				//PlaceAccessor
				PlaceObj (_objStr, 2);
			}
		}

		//Crew
		else if (objType == 4) {

			if (Manned && !Debugging) {
				ColorTile (fullColor);
			} else {

				if (CasheScript.Instance.CouchMode) {
					if (GridPosition.Z == NetManager.Instance.localPlayerID) {
						//could quite possibly become a problem when implementing shipHopping...

						//Debug.LogError ("pos: " + GridPosition.Z + ", netID: " + NetManager.Instance.localPlayerID);
						if (CasheScript.Instance.CouchCrewFits ()) {
							//CC -> couch coop
							_objStr = _objStr + "CC";

							//PlaceObj (_objStr);

							//whats group!?
							//GameObject _obj = (GameObject)Instantiate (LevelManager.Instance.ObjDict [_objStr], transform.position, Quaternion.identity);

							//NetManager.Instance.SpawnCrew (LevelManager.Instance.ObjDict [_objStr]);

							/*
							GameObject _obj = (GameObject) Instantiate (LevelManager.Instance.ObjDict [_objStr], transform.position, Quaternion.identity);
							IPlacable _placable = _obj.GetComponent <IPlacable> ();
							_placable.PlaceObj (0, this.GridPosition, _obj);

							CasheScript.Instance.AssignController (_obj.GetComponent <CouchCrewScript> ());
							*/

							//maybe not the best place for this
							if (Camera.main != null) {
								Camera.main.gameObject.SetActive (false);
							}


							NetManager.Instance.SpawnCrewCmd (GridPosition, _objStr);
						}
					} else {
						Debug.Log ("not enough couch companions for full crew");
					}
				} else {

					//PlaceCrew ();
					PlaceObj (_objStr);

					//modal issue 00; fix? -> add postfix depending on mode.
				}
			}
		} 

		//DEBUG

		//Dangers
		else if (objType == 5) {
			if (!walkable && !Debugging && OnFire) {
				ColorTile (fullColor);
			} else if (!OnFire) {
				//PlaceDanger ();
				PlaceObj (_objStr);
			}
		}

		//END DEBUG

		//Targeting
		else if (objType == 6) {
			//SetTarget (_objStr);
			PlaceObj (_objStr);
		} 

		//Weapons
		else if (objType == 7) {
			PlaceObj (_objStr);
		}
			
	}

	/*
	public void PlaceTarget (string _objStr, Point _gunPoint) {
		

		PlaceObj (_objStr);
	}
	*/


	//DAMAGING //21.04.18: commented out
	/*
	public void TakeDamage (int _dmg) {
		//introduce damageType etc later...

		//if (Manned) {
		//	Debug.Log ("hussah!");
		//}


		if (!IsEmpty) {
			transform.GetChild (0).GetChild (0).GetComponent <RoomScript> ().DistributeDamage (false, _dmg);
		}

		/*
		//room //others -> if theres no room, theres nothing!
		if (!IsEmpty) {

			//subSys
			if (HasSubSys || HasAccessor) {
				Debug.Log ("submissive Systematics Detected");

				transform.GetChild (6).GetChild (0).GetComponent <HealthScript> ().TakeDamage (_dmg);
			}

			//system
			if (!SystemPlacable) {
				Debug.Log ("fatal sys err!");

				transform.GetChild (1).GetChild (0).GetComponent <HealthScript> ().TakeDamage (_dmg);
			}

			//crew
			for (int i = 0; i < transform.GetChild (2).transform.childCount; i++) {
				Debug.Log ("crew" + i);

				transform.GetChild (2).GetChild (i).GetComponent <HealthScript> ().TakeDamage (_dmg);
			}



			transform.GetChild (0).GetChild (0).GetComponent <HealthScript> ().TakeDamage (_dmg);
		}
		////


		//if (SubSysPlacable) //-> need bool for that
	}
	*/


	public HealthScript [] GetHScripts (int rmDmg, int sysDmg, int subDmg) {
		HealthScript[] _hScrArr = new HealthScript[3];

		if (rmDmg == 0) {
			Debug.LogError ("Damage is 0!");
		} else {
			if (LocalObjDict.ContainsKey (0)) {
				//LocalObjDict [0].TakeCrewDamage (rmDmg);
			} else {
				//not the nicest way of doin' it but...
				LocalObjDict.Add (0, transform.GetChild (0).GetChild (0).gameObject.GetComponent <HealthScript> ());
				Debug.Log ("dict entry made");
				//LocalObjDict [0].TakeCrewDamage (rmDmg);
			}

			_hScrArr [0] = LocalObjDict [0].GetOriginHScr ();
		}

		if (sysDmg == 0) {
			Debug.LogError ("Damage is 0!");
		} else {
			if (LocalObjDict.ContainsKey (1)) {
				//LocalObjDict [0].TakeCrewDamage (rmDmg);
				_hScrArr [1] = LocalObjDict [1].GetOriginHScr ();
			} else if (!SystemPlacable) {
				//not the nicest way of doin' it but...
				LocalObjDict.Add (1, transform.GetChild (1).GetChild (0).gameObject.GetComponent <HealthScript> ());
				Debug.Log ("dict entry made");
				//LocalObjDict [0].TakeCrewDamage (rmDmg);

				_hScrArr [1] = LocalObjDict [1].GetOriginHScr ();
			}
		}

		if (subDmg == 0) {
			Debug.LogError ("Damage is 0!");
		} else {
			if (LocalObjDict.ContainsKey (2)) {
				//LocalObjDict [0].TakeCrewDamage (rmDmg);
				_hScrArr [2] = LocalObjDict [2].GetOriginHScr ();
			} else if (transform.GetChild (6).childCount > 0) {
				//not the nicest way of doin' it but...
				LocalObjDict.Add (2, transform.GetChild (6).GetChild (0).gameObject.GetComponent <HealthScript> ());
				Debug.Log ("dict entry made");
				//LocalObjDict [0].TakeCrewDamage (rmDmg);

				_hScrArr [2] = LocalObjDict [2].GetOriginHScr ();
			}
		}

		return _hScrArr;

		/*
		HealthScript[] _hScrArr = new HealthScript[LocalObjDict.Count];
		for (int i = 0; i < LocalObjDict.Count; i++) {
			_hScrArr [i] = 
		}
		*/
	}

	public ISystem GetSystem () {
		ISystem _iSys = transform.GetChild (1).GetChild (0).GetComponent <ISystem> ();
		return _iSys;
	}

	public TerminalScr GetTerminal () {
		TerminalScr _terminalScr = transform.GetChild (6).GetChild (0).GetComponent <TerminalScr> ();
		return _terminalScr;
	} 

	public void PlaceTarget (int _gunID) {
		SetTarget ("Target", _gunID);
	}

	//only called on server
	public GameObject RemoteCouchCrew (string _objStr) {
		GameObject _obj = (GameObject) Instantiate (LevelManager.Instance.ObjDict [_objStr], transform.position, Quaternion.identity);
		IPlacable _placable = _obj.GetComponent <IPlacable> ();
		_placable.PlaceObj (0, this.GridPosition, _obj);

		//CasheScript.Instance.AssignController (_obj.GetComponent <CouchCrewScript> ());
		/*
		if (GridPosition.Z == NetManager.Instance.localPlayerID) {
			//isLocal = true;

			//couchCrewSetup is called by this as well...
			CasheScript.Instance.AssignController (_obj.GetComponent <CouchCrewScript> ());
		}

		Debug.LogError ("crewPos: " + GridPosition.Z + ", netID: " + NetManager.Instance.localPlayerID);
		*/

		return _obj;
	}
}