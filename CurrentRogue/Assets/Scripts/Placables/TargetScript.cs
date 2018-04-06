using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour, IPlacable
{
	//the parent tile
	private TileScript tile;
	//the gridPos
	private Point gridPos;
	public Point GridPos { get { return gridPos; } }

	[SerializeField]
	private string objStr;

	private Point gunPoint;
	private WeaponScript weapon;



	public void PlaceObj (int _index, Point _gridPos, GameObject _originObj) {
		gridPos = _gridPos;
		tile = LevelManager.Instance.Tiles [gridPos];

		transform.SetParent (tile.transform.GetChild (7));

		/*
		Debug.Log ("wazzup");
		transform.SetParent (tile.transform.GetChild (5));
		Debug.Log ("dazzup");
		transform.SetParent (tile.transform.GetChild (5));
		Debug.Log ("lalala");
		*/

		//DEBUG
		//tile.OnFire = true;

		GameManager.Instance.Buy ();


	}

	public void GetGun (Point _gunPoint, float _angle) {
		gunPoint = _gunPoint;
		weapon = LevelManager.Instance.Tiles [_gunPoint].transform.GetChild (0).GetComponent <WeaponScript> ();


		//weapon.targetObj = tile.gameObject;
	
		//centers the target
		//weapon.targetObj = transform.GetChild (0).gameObject;
		weapon.DestroyPreviousTarget (transform.GetChild (0).gameObject);

		//weapon.Ammo.GetComponent <AmmoScript> ().SetTarget (tile);

		//weapon.Fire (_angle);
		weapon.TryFire (_angle);

		//weapon.TryFire (_angle);
		//Debug.Log ("test");
	}


	public void RemoveObj () {
		//tile.OnFire = false;

		Destroy (gameObject);
	}




	public void UpdateHealth (int _amount) {
		Debug.Log ("took Damage");
	}
}