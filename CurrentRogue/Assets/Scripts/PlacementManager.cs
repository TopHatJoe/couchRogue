using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : Singleton <PlacementManager>
{
	[SerializeField]
	private int price;
	public int Price { get { return price; } set { price = value; } }

	//0=room; 1=system; 2=elevator; 3=crew;
	public int objType;
	public int objRef;
	public int objWidth;

	private static float RoomID = 0.0f;
	private static float SystemID = 0.1f;
	private static float CrewID = 0.3f;
	private static float DangerID = 0.4f;
	private static float DoorID = 0.5f;

	[SerializeField]
	private GameObject objectPrefab;
	public GameObject ObjectPrefab { get { return objectPrefab; } set { objectPrefab = value; } }

	[SerializeField]
	private Sprite sprite;
	public Sprite Sprite { get { return sprite; } set { sprite = value; } }

	[SerializeField]
	private GameObject targetObj;
	public GameObject TargetObj { get { return targetObj; } }

	public string objStr;

	//public GameObject GunObj;
	public Point GunPoint;

    public int GunID;
    public int ShipID;

	/*
	public void TestID ()
	{
		AssignRoomID ();
		AssignSystemID ();
		AssignCrewID ();
		AssignDangerID ();
	}
	*/


	//public static float AssignRoomID ()
	public float AssignRoomID ()
	{
		RoomID++;
		//Debug.Log ("room assigned");
		//Debug.Log ("RoomID: " + RoomID);
		return RoomID;
	}

	//public static float AssignSystemID ()
	public float AssignSystemID ()
	{
		SystemID++;
		//Debug.Log ("system assigned");
		//Debug.Log (SystemID);
		return SystemID;
	}

	//public static float AssignCrewID ()
	public float AssignCrewID ()
	{
		CrewID++;
		//Debug.Log ("crew assigned");
		//Debug.Log (CrewID);
		return CrewID;
	}

	//public static float AssignDoorID ()
	public float AssignDoorID ()
	{
		DoorID++;
		//Debug.Log (DangerID);
		return DoorID;
	}

	//Debug... I think?

	//public static float AssignDangerID ()
	public float AssignDangerID ()
	{
		DangerID++;
		//Debug.Log (DangerID);
		return DangerID;
	}

}