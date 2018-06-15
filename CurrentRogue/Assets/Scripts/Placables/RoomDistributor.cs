using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomDistributor : MonoBehaviour 
{
	private void Start()
	{
        if (CasheScript.Instance.GameMode == 0)
        {
            gameObject.SetActive(false);
        }
	}

	/*
    private Point origGridPos;


	private void Start()
	{
        origGridPos = transform.parent.GetComponent<RoomScript>().GridPos;
	}

	private void OnMouseOver()
	{
        Debug.Log("oi");

        if (Input.GetButtonUp("Fire1") && CrewSelect.currentlySelected.Count > 0)
        {
            foreach (CrewSelect _selected in CrewSelect.currentlySelected)
            {
                _selected.MovementOrders(origGridPos);
            }

            LevelManager.Instance.DragSelectRef.gameObject.SetActive(true);
        }
 	}

	/*
    private List<HealthScript> inRoomHScr = new List<HealthScript>();



	private void Start()
	{
		 
	}

    private IEnumerator WaitRoutine () {
        yield return new WaitForSeconds(5f);

    }


	private void OnTriggerStay2D(Collider2D _collision) {
		
	}

	private void OnTriggerEnter2D(Collider2D _col)
	{
        Debug.Log("OI!!");

        //Debug.LogError("ahhhh collisions in the mornin!");
        //Debug.LogError("roomPos: " + gridPos.X + ", " + gridPos.Y);    

        //CouchCrewScript _couchCrew = _col.GetComponent<CouchCrewScript>();
        HealthScript _hScr = _col.GetComponent<HealthScript>();

        inRoomHScr.Add(_hScr);
	}


    private void OnTriggerExit2D(Collider2D _col)
    {
        Debug.Log("Bye!!");

        //Debug.LogError("getouttahear!");

        //Debug.LogError("ahhhh collisions in the mornin!");
        //CouchCrewScript _couchCrew = _col.GetComponent<CouchCrewScript>();
        HealthScript _hScr = _col.GetComponent<HealthScript>();

        inRoomHScr.Remove(_hScr);
    }

    */
}