using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrewSelect : MonoBehaviour, ISelectHandler, IPointerClickHandler, IDeselectHandler
{
	public static HashSet <CrewSelect> allCrew = new HashSet <CrewSelect> ();
	public static HashSet <CrewSelect> currentlySelected = new HashSet <CrewSelect> ();

	//probabs not needed
	//private Renderer crewRenderer;
	private CrewScript crewScript;
	private SpriteRenderer crewSR;

	[SerializeField]
	private Sprite unselectedCrew;
	[SerializeField]
	private Sprite selectedCrew;

	//public bool IsSelected { get; private set; }
	//private bool isLocalCrew;


	private void Awake ()
	{
		//crewRenderer = transform.parent.GetComponent <Renderer> ();
		crewScript = transform.parent.GetComponent <CrewScript> ();
		crewSR = transform.parent.GetComponent <SpriteRenderer> ();
	}

	public void AddToHash (bool _isLocalCrew)
	{
		//adds crew only to selectables if is part of local crew
		//if (transform.parent.GetComponent <CrewScript> ().IsLocalCrew) {
		if (_isLocalCrew) {
			allCrew.Add (this);
		}
	}

	public void OnPointerClick (PointerEventData _eventData)
	{
		if (!Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.RightShift)) {
			DeselectAll (_eventData);
		}

		OnSelect (_eventData);
		//Debug.Log ("click");
	}

	public void OnSelect (BaseEventData _eventData)
	{
		currentlySelected.Add (this);

		//IsSelected = true;
		//crewScript.SelectionState (IsSelected);

		crewSR.sprite = selectedCrew;
		//crewRenderer.
	}

	public void OnDeselect (BaseEventData _eventData)
	{
		//IsSelected = false;
		//crewScript.SelectionState (IsSelected);

		crewSR.sprite = unselectedCrew;
	}

	public static void DeselectAll (BaseEventData _eventData)
	{
		foreach (CrewSelect selectable in currentlySelected) {
			selectable.OnDeselect (_eventData);
		}

		currentlySelected.Clear ();
	}

	public void MovementOrders (Point _destination)
	{
		crewScript.GiveMovementOrders (_destination);
	}

	private void OnDestroy ()
	{
		RemoveFromHash ();
		crewScript.ResetDestination ();
	}

	private void RemoveFromHash ()
	{
		if (crewScript.IsLocalCrew) {
			allCrew.Remove (this);

			if (currentlySelected.Contains (this)) {
				currentlySelected.Remove (this);
			}
		}
	}
}