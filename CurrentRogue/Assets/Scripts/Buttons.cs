using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour 
{
	[SerializeField]
	private int price;
	public int Price { get { return price; } }

	[SerializeField]
	private Text priceTxt;

	public int objWidth;

	//0=room; 1=system; 2=elevator; 3=crew;
	public int objType;

	public int objRef;

	[SerializeField]
	private string objDimensions;
	public string ObjDim { get { return objDimensions; } }

	[SerializeField]
	private string objStr;
	public string ObjStr { get { return objStr; } }

	[SerializeField]
	private GameObject localObjectPrefab;
	public GameObject LocalObjectPrefab { get { return localObjectPrefab; } }

	[SerializeField]
	private Sprite sprite;
	public Sprite Sprite { get { return sprite; } }

	//[SerializeField]
	//private bool isGun;
	//private GameObject gunObj;
	private Point gunPoint;


	void Start ()
	{
		priceTxt.text = price + " MONEY";
	}

	public void SetPrefab ()
	{
		PlacementManager placementMngr = PlacementManager.Instance;

		placementMngr.Price = Price;
		placementMngr.objWidth = objWidth;
		placementMngr.objType = objType;
		placementMngr.ObjectPrefab = LocalObjectPrefab;
		placementMngr.Sprite = Sprite;

		placementMngr.objRef = objRef;

		placementMngr.objStr = objStr;

		//if (isGun) {
		//placementMngr.GunPoint = gunPoint;
		//}

		/*
		PlacementManager.Instance.Price = Price;
		PlacementManager.Instance.objWidth = objWidth;
		PlacementManager.Instance.objType = objType;
		PlacementManager.Instance.ObjectPrefab = LocalObjectPrefab;
		PlacementManager.Instance.Sprite = Sprite;

		PlacementManager.Instance.objRef = objRef;
		*/
	}
}