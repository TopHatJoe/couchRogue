using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerScript : MonoBehaviour, IPlacable
{
	//the parent tile
	private TileScript tile;
	//the gridPos
	private Point gridPos;

	[SerializeField]
	private string objStr;



	public void PlaceObj (int _index, Point _gridPos, GameObject _originObj) {
		gridPos = _gridPos;
		tile = LevelManager.Instance.Tiles [gridPos];

		transform.SetParent (tile.transform.GetChild (4));

		//DEBUG
		tile.OnFire = true;

		GameManager.Instance.Buy ();
	}


	public void RemoveObj () {
		tile.OnFire = false;

		Destroy (gameObject);
	}


	public void UpdateHealth (int _amount) {
		Debug.Log ("took Damage");
	}


	public GameObject GetOriginObj () {
		return gameObject;
	}
}