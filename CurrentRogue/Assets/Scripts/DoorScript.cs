using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoorScript : MonoBehaviour 
{
	[SerializeField]
	private Sprite doorOpen;
	[SerializeField]
	private Sprite doorClosed;
	[SerializeField]
	private GameObject door;
	private SpriteRenderer sprRenderer;

	public Point closestTile;
	public Point doorPos;

	public bool isOpen = false;

	private float doorID;

	private TileScript tween;


	void Start ()
	{
		sprRenderer = door.GetComponent<SpriteRenderer> ();

		ChangeDoorstate (!isOpen);

		//doorID = PlacementManager.AssignDoorID ();
		//doorID = PlacementManager.Instance.AssignDoorID ();

		closestTile = doorPos;

		doorPos = new Point (doorPos.X - 1, doorPos.Y, doorPos.Z);

		TileScript tweenScript = LevelManager.Instance.Tiles [doorPos].GetComponent<TileScript> ();
		tween = tweenScript;

		//mayhap not the best place
		tween.HasDoor = true;
		tween.doorRef = this;

		ChangeDoorstate (!isOpen);
		ChangeDoorstate (!isOpen);
	}


	void OnMouseOver ()
	{
		if (NetManager.Instance != null) {
			if (doorPos.Z == NetManager.Instance.localPlayerID) {
				if (Input.GetMouseButtonDown (0)) {
					//Test ();
					if (NetManager.Instance != null) {
						SyncDoorState ();
					} else {
						ChangeDoorstate (!isOpen);
					}
				}
			}
		}
	}


	private void SyncDoorState ()
	{
		//NetManager.Instance.playerList[]
		if (NetManager.Instance != null) {
			//should i change that?	
			//Vector3 _doorPos = new Vector3 (doorPos.X, doorPos.Y, doorPos.Z);
			Vector3 _doorPos = new Vector3 (closestTile.X, closestTile.Y, closestTile.Z);
			//Debug.Log (_doorPos);
			NetManager.Instance.SyncDoorstate (_doorPos, !isOpen);
		} else {
			ChangeDoorstate (!isOpen);
		}
	}

	public void ChangeDoorstate (bool _doorState)
	{
		//isOpen = !isOpen;
		isOpen = _doorState;

		TileScript tweenScript = LevelManager.Instance.Tiles [doorPos].GetComponent<TileScript> ();

		if (isOpen) {
			sprRenderer.sprite = doorOpen;
			tweenScript.DoorOpen = true;
			//Debug.Log ("door (" + doorPos.X + ", " + doorPos.Y + ") is Open");
		} else {
			sprRenderer.sprite = doorClosed;
			tweenScript.DoorOpen = false;
			//Debug.Log ("door (" + doorPos.X + ", " + doorPos.Y + ") is Closed");
		}
	}

	private void OnDestroy ()
	{
		tween.HasDoor = false;
	}


	public void LetCrewThrough ()
	{
		StopCoroutine (DoorCycle ());
		StartCoroutine (DoorCycle ());
	}

	/*
	public void OpenForCrew ()
	{
		if (NetManager.Instance != null) {
			SyncDoorState ();
		} else {
			ChangeDoorstate (true);
		}
	}

	public void CloseForCrew ()
	{
		if (NetManager.Instance != null) {
			SyncDoorState ();
		} else {
			ChangeDoorstate (false);
		}
	}
	*/

	private IEnumerator DoorCycle ()
	{
		if (NetManager.Instance != null) {
			SyncDoorState ();
		} else {
			//ChangeDoorstate (!isOpen);
			ChangeDoorstate (true);
		}

		yield return new WaitForSeconds (0.65f);

		if (NetManager.Instance != null) {
			SyncDoorState ();
		} else {
			//ChangeDoorstate (!isOpen);
			ChangeDoorstate (false);
		}
	}
}