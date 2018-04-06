using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelectBtn : MonoBehaviour 
{
	public void setShipType (string shipType)
	{
		CasheScript.Instance.ShipType = shipType;
	}
}