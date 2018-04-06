using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInfo : MonoBehaviour 
{
	[SerializeField]
	private bool isEmptySpace;
	public bool Walkable { get; set; }
	public bool Manned { get; set; }
	public bool HasElevator { get; set; }
	public bool OnFire { get; set; }
	private bool leftDoorOpen;
	public bool LeftDoorOpen;

	public bool HasDoor { get; set; }


	[SerializeField]
	private TileScript tile;

	[SerializeField]
	private TweenScript tween;

	public bool IsTile { get; set; }

	void Start ()
	{
		if (tile != null) {
			IsTile = true;

		} else if (tween != null) {
			IsTile = false;
		} else {
			Debug.LogError ("no reference error (NodeInfo has no reference)!");
		}
	}

	private void SetTileBooleans ()
	{
		Walkable = tile.Walkable;
		Manned = tile.Manned;
		HasElevator = tile.HasElevator;
		OnFire = tile.OnFire;
	}

	private void SetTweenBooleans ()
	{
		HasDoor = tween.HasDoor;
	}
}