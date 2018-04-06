using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSystemScript : MonoBehaviour, IPlacable
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
		saveStr = (objStr + ",2," + gridPos.X.ToString () + "," + gridPos.Y.ToString ());
		LevelManager.Instance.parameterList.Add (saveStr);
		//}

		transform.SetParent (tile.transform.GetChild (6));

		tile.SubSysPlacable = false;
		tile.HasSubSys = true;

		GameManager.Instance.Buy ();
	}

	public void RemoveObj () {
		tile.SubSysPlacable = true;
		tile.HasSubSys = false;

		LevelManager.Instance.parameterList.Remove (saveStr);

		Destroy (gameObject);
	}


	public void UpdateHealth (int _amount) {
		Debug.Log ("subSys took Damage");
	}
}