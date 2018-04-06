using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessorScript : MonoBehaviour, IPlacable
{
	//the parent tile
	private TileScript tile;
	//the gridPos
	private Point gridPos;

	[SerializeField]
	private string objStr;

	private string saveStr;



	public void PlaceObj (int _index, Point _gridPos, GameObject _originObj) {
		gridPos = _gridPos;
		tile = LevelManager.Instance.Tiles [gridPos];

		//if (this.gameObject == originObj) {
		saveStr = (objStr + ",3," + gridPos.X.ToString () + "," + gridPos.Y.ToString ());
		LevelManager.Instance.parameterList.Add (saveStr);
		//}

		transform.SetParent (tile.transform.GetChild (6));

		tile.SubSysPlacable = false;
		tile.HasAccessor = true;

		//DEBUG
		tile.HasElevator = true;

		GameManager.Instance.Buy ();
	}

	public void RemoveObj () {
		tile.SubSysPlacable = true;
		tile.HasAccessor = false;

		LevelManager.Instance.parameterList.Remove (saveStr);


		//DEBUG
		tile.HasElevator = false;

		Destroy (gameObject);
	}



	public void UpdateHealth (int _amount) {
		Debug.Log (_amount);

		//needs to also be applied to nodes
		if (_amount <= 0) {
			tile.HasElevator = false;
		} else {
			tile.HasElevator = true;
		}

		Debug.Log ("hasElevator: " + tile.HasElevator);
	}
}