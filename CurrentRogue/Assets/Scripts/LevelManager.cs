using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//inherits from singleton for ease of access
public class LevelManager : Singleton<LevelManager>
{
	//NETWORKING
	private int tmpPlayerID = 0;
	[SerializeField]
	private int numOfShips;
	public int NumOfShips { get { return numOfShips; } }

	[SerializeField]
	private GameObject[] ships;
	public GameObject[] Ships { get { return ships; } }

	[SerializeField]
	private GameObject[] tilePrefabs;

	[SerializeField]
	private GameObject gunBtnPanel;
	public GameObject GunBtnPanel { get { return gunBtnPanel; } }


	[SerializeField]
	public List <GameObject> ObjList;

	[SerializeField]
	private GameObject[] roomArray;
	public GameObject[] RoomArray { get { return roomArray; } }
	[SerializeField]
	private GameObject[] systemArray;
	public GameObject[] SystemArray { get { return systemArray; } }
	[SerializeField]
	private GameObject[] subSysArray;
	public GameObject[] SubSysArray { get { return subSysArray; } }
	[SerializeField]
	private GameObject[] accessorArray;
	public GameObject[] AccessorArray { get { return accessorArray; } }
	[SerializeField]
	private GameObject[] crewArray;
	public GameObject[] CrewArray { get { return crewArray; } }
	[SerializeField]
	private GameObject[] weaponArray;
	public GameObject[] WeaponArray { get { return weaponArray; } }
	[SerializeField]
	private GameObject[] dangerArray;
	public GameObject[] DangerArray { get { return dangerArray; } }
	[SerializeField]
	private GameObject[] uiArray;
	public GameObject[] UiArray { get { return uiArray; } }


	public List <string> parameterList;

	[SerializeField]
	private GameObject hover;

	[SerializeField]
	private GameObject gridField;
	public GameObject GridField { get { return gridField; } }
	[SerializeField]
	private GameObject[] gridFields;

	[SerializeField]
	private Transform grid;
	[SerializeField]
	private Transform squareGrid;
	[SerializeField]
	private Transform tweenGrid;

	//gets desired numOfTiles for x and y
	[SerializeField]
	private int numOfTilesX = 48;
	[SerializeField]
	private int numOfTilesY = 27;

	//stores screen.width in a float, so localScale doesnt round to int
	private float screenWidthFloat; // = Screen.width;

	//calculates tilesize by multiplying the sprite size with the transorm.localScale
	private float TileSizeX
	{ get{ return tilePrefabs[0].GetComponent<SpriteRenderer> ().sprite.bounds.size.x * tilePrefabs[0].transform.localScale.x; } }
	private float TileSizeY
	{ get{ return tilePrefabs[0].GetComponent<SpriteRenderer> ().sprite.bounds.size.y * tilePrefabs[0].transform.localScale.y; } }

	private float SquareTileSizeX
	{ get{ return tilePrefabs[3].GetComponent<SpriteRenderer> ().sprite.bounds.size.x * tilePrefabs[3].transform.localScale.x; } }
	private float SquareTileSizeY
	{ get{ return tilePrefabs[3].GetComponent<SpriteRenderer> ().sprite.bounds.size.y * tilePrefabs[3].transform.localScale.y; } }

	private Point mapSize;

	//instantiating TileDictionary containing all points in grid
	public Dictionary<Point, TileScript> Tiles { get; set; }

	[SerializeField]
	private DragSelect dragSelect;
	public DragSelect DragSelectRef { get { return dragSelect; } }

	//[SerializeField]
	private Dictionary <string, GameObject> objDict = new Dictionary <string, GameObject> ();
	public Dictionary <string, GameObject> ObjDict { get { return objDict; } }


	//list of all roomOrigins //used for allRoomDamaging
	public List <RoomScript> roomList = new List<RoomScript> ();

	//public List <ElevatorScript> elevatorList



	//public string RoomString;



	void Start ()
	{
		Setup ();
	}

	private void Setup () {
		//creates dictionary of all origObj
		CreateObjDictionary ();

		//activate proper number of ships
		SetShipsActive ();

		//Debug.Log ("Start start");
		ScreenSetup ();
		//Debug.Log ("Screen was Setup");

		CreateLevel ();
		//Debug.Log ("Level created");


		//should place ships and get references
		StartCoroutine (ArtificialDelayIni ());

        //Debug.Log ("Place");
        //PlaceAllShips ();

        //reference roomContent origins; //way too early!
        //GetRoomContents ();

        SyncShipPos();
	}

    private void SyncShipPos () {
        if (CasheScript.Instance.GameMode != 0) {
            for (int i = 0; i < ships.Length; i++) {
                NetManager.Instance.SyncShipPos(i, ships[i].transform.position);
            }
        }
    }

	private void CreateObjDictionary () {
		/*
		//ROOMS
		objDict.Add ("1_Room1", 0);
		objDict.Add ("2_Room1", 1);
		objDict.Add ("3_Room1", 2);
		objDict.Add ("302_Room1", 3);
		objDict.Add ("022_Room1", 4);

		//SYSTEMS
		objDict.Add ("1_System1", 0);
		objDict.Add ("2_System1", 1);
		objDict.Add ("3_System1", 2);
		objDict.Add ("302_System1", 3);

		//SUBSYSTEMS
		objDict.Add ("1_GoobleBox", 0);

		//ACCESSORS
		objDict.Add ("1_Elevator", 0);

		//CREW
		objDict.Add ("1_Crew", 0);

		//DANGERS
		objDict.Add ("Fire", 0);

		//UI
		objDict.Add ("Target", 0);
		*/


		//ROOMS
		//GameObject _obj = roomArray [0];
		objDict.Add ("1_Room1", roomArray [0]);
		objDict.Add ("2_Room1", roomArray [1]);
		objDict.Add ("3_Room1", roomArray [2]);
		//objDict.Add ("302_Room1", roomArray [3]);
		objDict.Add ("022_Room1", roomArray [3]);
		objDict.Add ("302_Room1", roomArray [4]);

		//SYSTEMS
		objDict.Add ("1_System1", systemArray [0]);
		objDict.Add ("2_System1", systemArray [1]);
		objDict.Add ("3_System1", systemArray [2]);
		objDict.Add ("302_System1", systemArray [3]);
		objDict.Add ("3_Reactor1", systemArray [4]);
		objDict.Add ("3_Shield1", systemArray [5]);
		objDict.Add ("3_Weapon1", systemArray [6]);
		objDict.Add ("2_Medbay1", systemArray [7]);
		objDict.Add ("3_Engine1", systemArray [8]);
        objDict.Add ("3_Teleporter1", systemArray[9]);

		//SUBSYSTEMS
		objDict.Add ("1_GoobleBox", subSysArray [0]);

		//ACCESSORS
		objDict.Add ("1_Elevator", accessorArray [0]);

		//CREW
		objDict.Add ("1_Crew", crewArray [0]);
		//couch crew
		objDict.Add ("1_CrewCC", crewArray [1]);

		//DANGERS
		objDict.Add ("1_Fire", dangerArray [0]);

		//WEAPONS
		objDict.Add ("2_Gun", weaponArray [0]);
		objDict.Add ("2_GunF", weaponArray [1]);
		objDict.Add ("4_Gun", weaponArray [2]);
		objDict.Add ("4_GunF", weaponArray [3]);
		objDict.Add ("6_Gun", weaponArray [4]);
		objDict.Add ("6_GunF", weaponArray [5]);

		//UI
		objDict.Add ("Target", uiArray [0]);

	}


	private void ScreenSetup ()
	{
		ScreenSetup _screen = gameObject.GetComponent <ScreenSetup> ();
		_screen.Setup ();
		screenWidthFloat = _screen.ScreenWidth;
	}

	private void CreateLevel ()
	{
		CreateRoomGrid ();
		Debug.Log ("roomdrid created");
		CreateSquareGrid ();
		Debug.Log ("squaregrid created");
		SetCams ();

		CorrectCam ();
	}

	private void SetShipsActive ()
	{
		//numOfShips = 1;

		//if (NetManager.Instance != null) {
        numOfShips = CasheScript.Instance.ShipList.Count; //NetManager.Instance.playerList.Count;
		//}

        if (CasheScript.Instance.GameMode == 0) {
            //if in hangar
            numOfShips = 1;
        }

		for (int i = 0; i < numOfShips; i++) {
			ships [i].SetActive (true);
		}
	}

	private void CreateRoomGrid ()
	{
		Tiles = new Dictionary<Point, TileScript>();


		//string tmpString = "standard";
        string _typeStr = "triton";



		//probably debug only //needs FIXIN! //nope

		if (CasheScript.Instance != null) {
			_typeStr = CasheScript.Instance.ShipType;
		}
        //Debug.Log("shipType: " + _typeStr);
	
		string[] mapData = ReadLevelText (_typeStr);
		//string[] mapData = ReadLevelText ("standard");



		mapSize = new Point (mapData [0].ToCharArray ().Length, mapData.Length, tmpPlayerID);

		int mapX = mapData [0].ToCharArray ().Length;
		int mapY = mapData.Length;

		//sets origin point for grid
		Vector3 gridStart = gridField.transform.position;

		//gets the required tile size, so the grid fits
		float newTileSizeX = (screenWidthFloat / numOfTilesX);
		float newTileSizeY = (((screenWidthFloat / 16) * 9) / numOfTilesY);

		tilePrefabs[0].transform.localScale = new Vector3 (newTileSizeX, newTileSizeY);
		tilePrefabs[1].transform.localScale = new Vector3 (newTileSizeX, newTileSizeY);
		tilePrefabs[2].transform.localScale = new Vector3 (newTileSizeX, newTileSizeY);

		tilePrefabs[7].transform.localScale = new Vector3 (newTileSizeX, newTileSizeY);

		AdjustObjectScale ();

        //Debug.Log("numOfShips: " + numOfShips);
		for (int z = 0; z < numOfShips; z++) {

			gridStart = gridFields [z].transform.position;

			if (NetManager.Instance != null) {
                //mapData = ReadLevelText(NetManager.Instance.playerList[z].ShipType);
                mapData = ReadLevelText (CasheScript.Instance.ShipList [z].Type);
			}
				
			for (int y = 0; y < mapY; y++) 
			{
				char[] newTiles = mapData [y].ToCharArray ();
				//place tiles on evens
				for (int x = 0; x < mapX; x += 2) 
				{
					//PlaceTile (newTiles[x].ToString(), x, y, tmpPlayerID, gridStart);
					PlaceTile (newTiles[x].ToString(), x, y, z, gridStart);
					//GRIDSTART
					//Debug.Log ("place loop");
				}

				if (y == mapY - 1) {
					//Debug.Log ("CreatedTiles");
				}
			}

			for (int y = 0; y < mapY; y++) {
				//place tween on odds
				for (int x = 1; x < mapX; x += 2) {
					//PlaceTween (x, y, tmpPlayerID, gridStart);
					PlaceTween (x, y, z, gridStart);
					//Debug.Log ("tween loop");
				}

				if (y == mapY - 1) {
					//Debug.Log ("CreatedTweens");
				}
			}	
		}
	}

	private void CreateSquareGrid ()
	{
		string[] mapData = ReadLevelText ("SquareGrid");

		int mapX = mapData [0].ToCharArray ().Length;
		int mapY = mapData.Length;

		//sets origin point for grid
		Vector3 gridStart = gridField.transform.position;

		//gets the required tile size, so the grid fits
		float newTileSizeX = (screenWidthFloat / 48);
		float newTileSizeY = (((screenWidthFloat / 16) * 9) / 27);

		tilePrefabs[3].transform.localScale = new Vector3 (newTileSizeX, newTileSizeY);
		tilePrefabs[4].transform.localScale = new Vector3 (newTileSizeX, newTileSizeY);
		tilePrefabs[5].transform.localScale = new Vector3 (newTileSizeX, newTileSizeY);


		for (int z = 0; z < numOfShips; z++) {
			gridStart = gridFields [z].transform.position;
			for (int y = 0; y < mapY; y++) 
			{
				char[] newTiles = mapData [y].ToCharArray ();

				for (int x = 0; x < mapX; x++) 
				{
					//PlaceSquareTile (newTiles[x].ToString(), x, y, tmpPlayerID, gridStart);
					PlaceSquareTile (newTiles[x].ToString(), x, y, z, gridStart);
				}
			}
		}
	}

	private void PlaceTile (string tileType, int x, int y, int z, Vector3 gridStart)
	{
		//parsing string to number "1" = 1 
		int tileIndex = int.Parse (tileType);

		TileScript newTile = Instantiate (tilePrefabs[tileIndex]).GetComponent<TileScript> ();

		newTile.Setup (new Point (x, y, z), new Vector3(gridStart.x + ((TileSizeX / 2) * x), gridStart.y + (TileSizeY) * y), ships[z].transform.GetChild(2), true);
	}

	private void PlaceSquareTile (string tileType, int x, int y, int z, Vector3 gridStart)
	{
		//parsing string to number "1" = 1 
		int tileIndex = int.Parse (tileType);

		SquareTileScript newSquareTile = Instantiate (tilePrefabs[tileIndex]).GetComponent<SquareTileScript> ();

		//also sets the parent
		newSquareTile.Setup (new Point (x, y, z), new Vector3(gridStart.x + (SquareTileSizeX * x), gridStart.y + (SquareTileSizeY * y)), ships[z].transform.GetChild(3));
	}

	private void PlaceTween (int x,  int y, int z, Vector3 gridStart)
	{
		int tileIndex = 6;
		TileScript newTween = Instantiate (tilePrefabs [tileIndex]).GetComponent<TileScript> ();

		//tell him that hes a tween!
		newTween.Setup (new Point (x, y, z), new Vector3(gridStart.x + ((TileSizeX / 2) * x), gridStart.y + (TileSizeY * y)), ships[z].transform.GetChild(4), false);
	}

	private void AdjustObjectScale ()
	{
		Vector2 roomSize = new Vector2 ((tilePrefabs[0].transform.localScale.x / 2), (tilePrefabs[0].transform.localScale.y / 3));

		for (int x = 0; x < ObjList.Count; x++) {
			ObjList [x].transform.localScale = roomSize;
		}

		hover.transform.localScale = roomSize;
	}

    private string[] ReadLevelText (string _shipType)
	{
		TextAsset bindData = Resources.Load (_shipType) as TextAsset;

		string data = bindData.text.Replace (Environment.NewLine, string.Empty);

		return data.Split ('-');
	}
		
	public bool InBounds (Point position)
	{
		return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
	}


	private void SetCams ()
	{
		CamSwitch.Instance.SetCams ();
	}

	private void SetShips ()
	{
		CamSwitch.Instance.PlaceAllShips ();
	}

	private void CorrectCam ()
	{
		if (NetManager.Instance != null) {
			CamSwitch.Instance.CorrectCams ();
		}
	}

	public void DebugTest ()
	{
		if (CrewSelect.currentlySelected.Count > 0) {
			Debug.Log ("you are the chosen one neo");
		} else {
			Debug.Log ("uh uh uh, you didnt say the magic word");
		}
	}





	public void PlaceAllShips ()
	{
        //NetManager.Instance.LoadShips ();

        if (CasheScript.Instance.GameMode != 0) {
            NetManager.Instance.playerList[0].PlaceShips();
        }

        //Debug.Log ("set");
		//NetManager.Instance.SetCrewIndex ();
	}






	private void GetRoomContents () {
		Debug.Log ("getting contents");

		for (int i = 0; i < roomList.Count; i++) {
			roomList [i].GetChildrenIni (i);
		}
	}


	private IEnumerator ArtificialDelayIni () {
		yield return new WaitForSeconds (5f);

		Debug.LogError ("placement ini");

		//this check is probably unnecessary since its perfomed within place all ships...
		if (NetManager.Instance != null) {
			if (NetManager.Instance.localPlayerID == 0) {
				//if (CasheScript.Instance.AutoPlace) {
					//Debug.LogError ("Placing all ships?");
				PlaceAllShips ();
				//}
            } else {
                Debug.LogError("localPlayerID != 0");
            }
        } else {
            //Debug.LogError("netMngr == null");
            PlaceAllShips();
        }

		/*
		else {
			if (CasheScript.Instance.AutoPlace) {
				PlaceAllShips ();
			}
		}
		*/

		yield return new WaitForSeconds (0.5f);

		GetRoomContents ();
	}


    public void AdjustShipPos (float _amount) {
        foreach (var _ship in ships)
        {
            Vector3 _vect = _ship.transform.position;
            _vect.x += _amount;
            _ship.transform.position = _vect;
        }
    }
}