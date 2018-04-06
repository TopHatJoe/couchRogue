using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Door
{
	public int DoorID { get; set; }

	public Door (int doorId)
	{
		this.DoorID = doorId;
	}

	//makes points comparable.
	public static bool operator == (Door first, Door second)
	{
		return first.DoorID == second.DoorID;
	}

	public static bool operator != (Door first, Door second)
	{
		return first.DoorID != second.DoorID;
	}
}