using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragSelect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	[SerializeField]
	private Image selectionBoxImg;

	Vector2 startPos;
	Rect selectionRect;


	private void Update ()
	{
		if (CrewSelect.currentlySelected.Count > 0) {
			if (Input.GetButtonDown ("Fire1")) {
				gameObject.SetActive (false);
			}
		}
	}

	public void OnBeginDrag (PointerEventData _eventData)
	{
		if (!Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.RightShift)) {
			CrewSelect.DeselectAll (new BaseEventData (EventSystem.current));
		}

		//Debug.Log ("DRAAAG!");

		selectionBoxImg.gameObject.SetActive (true);
		//mouse pos in screenSpace
		startPos = _eventData.position;

		selectionRect = new Rect ();
	}

	public void OnDrag (PointerEventData _eventData)
	{
		if (_eventData.position.x < startPos.x) {
			selectionRect.xMin = _eventData.position.x;
			selectionRect.xMax = startPos.x;
		} else {
			selectionRect.xMin = startPos.x; 
			selectionRect.xMax = _eventData.position.x;
		}

		if (_eventData.position.y < startPos.y) {
			selectionRect.yMin = _eventData.position.y;
			selectionRect.yMax = startPos.y;
		} else {
			selectionRect.yMin = startPos.y; 
			selectionRect.yMax = _eventData.position.y;
		}

		//scaling the img to match the rect
		selectionBoxImg.rectTransform.offsetMin = selectionRect.min;
		selectionBoxImg.rectTransform.offsetMax = selectionRect.max;
	}

	public void OnEndDrag (PointerEventData _eventData)
	{
		selectionBoxImg.gameObject.SetActive (false);

		foreach (CrewSelect selectable in CrewSelect.allCrew) {
			//probabs need to change the camera.main part
			if (selectionRect.Contains (Camera.main.WorldToScreenPoint (selectable.transform.position))) {
				selectable.OnSelect (_eventData);
			}
		}
	}

	public void OnPointerClick (PointerEventData _eventData)
	{
		List <RaycastResult> results = new List <RaycastResult> ();

		EventSystem.current.RaycastAll (_eventData, results);

		float distance = 0;

		foreach (RaycastResult result in results) {
			distance = result.distance;
			break;
		}

		GameObject nextObj = null;
		float maxDistance = Mathf.Infinity;

		foreach (RaycastResult result in results) {
			if (result.distance > distance && result.distance < maxDistance) {
				nextObj = result.gameObject;
				maxDistance = result.distance;
			}
		}

		if (nextObj) {
			ExecuteEvents.Execute<IPointerClickHandler> (nextObj, _eventData, (x, y) => { x.OnPointerClick ((PointerEventData)y); });
		}
	}
}