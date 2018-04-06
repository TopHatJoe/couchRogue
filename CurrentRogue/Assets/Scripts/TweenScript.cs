using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenScript : MonoBehaviour 
{
	public bool HasDoor { get; set; }
	public Point GridPosition { get; private set; }

	public void Setup (Point gridPos, Vector3 worldPos, Transform parent)
	{
		HasDoor = true;

		this.GridPosition = gridPos;
		transform.position = worldPos;
		//sets gameObject grid as newTiles parent
		transform.SetParent (parent);

		//adds newTile to Tiles Dictionary
		//LevelManager.Instance.Tiles.Add(gridPos, this);
	}
}