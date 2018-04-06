/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//can either be a room, sysrem, elevator, crew, or danger;
public enum TestObjType { Room, System, Elevator, Crew, Danger }

public class SaveLoadNotes : MonoBehaviour 
{
	[SerializeField]
	private GameObject singleRoom;
	[SerializeField]
	private GameObject doubleRoom;
	[SerializeField]
	private GameObject tripleRoom;
	[SerializeField]
	private GameObject systemOne;
	[SerializeField]
	private GameObject systemTwo;
	[SerializeField]
	private GameObject elevator;
	[SerializeField]
	private GameObject crewOne;
	[SerializeField]
	private GameObject crewTwo;
	[SerializeField]
	private GameObject fire;

	void Awake ()
	{
		List <DefineObjectType> objTypeList = new List <DefineObjectType> ();

		objTypeList.Add (new DefineObjectType (200, TestObjType.Room, doubleRoom));
		objTypeList.Add (new DefineObjectType (14707, TestObjType.System, systemTwo));
		objTypeList.Add (new DefineObjectType (0, TestObjType.Danger, fire));
		objTypeList.Add (new DefineObjectType (600, TestObjType.Room, tripleRoom));
		objTypeList.Add (new DefineObjectType (2000, TestObjType.Crew, crewOne));

		foreach (DefineObjectType objectType in objTypeList) 
		{
			Debug.Log (objectType.cost + " " + objectType.objType + " " + objectType.objTypePrefab);
		}
	}

	//public bool isLoading = false; //would tell the Placement functions if btn or load operation

	//dictionary or list to store object types, their values and propeties
}

public class DefineObjectType : MonoBehaviour
{
	public int cost;
	public TestObjType objType;
	public GameObject objTypePrefab;

	public DefineObjectType (int newCost, TestObjType newObjType, GameObject newObjTypePrefab)
	{
		cost = newCost;
		objType = newObjType;
		objTypePrefab = newObjTypePrefab;
	}
}
*/