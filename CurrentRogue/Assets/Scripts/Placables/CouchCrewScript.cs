using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouchCrewScript : MonoBehaviour 
{
	private Rigidbody2D rb;
	private BoxCollider2D col;
	private float speed = 1024f;
	private string controllerID;
	[SerializeField]
	private Camera cam;
	private int couchPlayerID;
	//the player is using an elevator, repairing stuff etc
	private bool isOccupied;
	private bool elevatorIsNear;
	private ElevatorScript elevator;

	[SerializeField]
	private GameObject elevatorMenu;
	private IEnumerator useElevator;


	void Start () {
		rb = gameObject.GetComponent <Rigidbody2D> ();
		col = gameObject.GetComponent <BoxCollider2D> ();
		//_cam = transform.GetChild (0).gameObject.GetComponent <Camera> ();

		//StartCoroutine (Test ());
	}

	void Update () {
		if (!isOccupied) {
			Vector3 _vect = new Vector3 (Input.GetAxis (controllerID + "-H"), 0);
			rb.MovePosition (transform.position + _vect * speed * Time.deltaTime);
		}

		if (elevatorIsNear) {
			if (Input.GetButtonDown (controllerID + "-s")) {
				DoElevatorMenu ();
			}
		}

		/*
		if (true) {
			Vector3 _vect = Vector3.left;
			rb.MovePosition (transform.position + _vect * speed * Time.deltaTime);
		}
		*/
	}

	public void CouchCrewSetup (string _controllerID, int _couchPlayerID, int _couchCount) {
		controllerID = _controllerID;

		Debug.Log ("playerID: " + _couchPlayerID);
		Debug.Log ("couchCount: " + _couchCount);

		couchPlayerID = _couchPlayerID;

		if (_couchCount == 2) {
			TwoPlayerSplit (_couchPlayerID);
		} else if (_couchCount == 3) {
			ThreePlayerSplit (_couchPlayerID);
		} else if (_couchCount == 4) {
			FourPlayerSplit (_couchPlayerID);
		}

		CanvasManager.Instance.CouchCanvas (couchPlayerID, cam);
	}

	private void TwoPlayerSplit (int _couchPlayerID) {
		if (_couchPlayerID == 1) {
			Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
			cam.rect = _rect0;
		} else if (_couchPlayerID == 2) {
			Rect _rect1 = new Rect (0f, 0f, 1f, 0.5f);
			cam.rect = _rect1;
		}


		//Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
		//Rect _rect1 = new Rect (0f, 0f, 1f, 0.5f);

		//camList [0].rect = _rect0;
		//camList [1].rect = _rect1;
	}

	private void ThreePlayerSplit (int _couchPlayerID) {
		if (_couchPlayerID == 1) {
			Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
			cam.rect = _rect0;
		} else if (_couchPlayerID == 2) {
			Rect _rect1 = new Rect (0f, 0f, 0.5f, 0.5f);
			cam.rect = _rect1;
		} else if (_couchPlayerID == 3) {
			Rect _rect2 = new Rect (0.5f, 0f, 0.5f, 0.5f);
			cam.rect = _rect2;
		}


		//Rect _rect0 = new Rect (0f, 0.5f, 1f, 0.5f);
		//Rect _rect1 = new Rect (0f, 0f, 0.5f, 0.5f);
		//Rect _rect2 = new Rect (0.5f, 0f, 0.5f, 0.5f);

		//camList [0].rect = _rect0;
		//camList [1].rect = _rect1;
		//camList [2].rect = _rect2;
	}

	private void FourPlayerSplit (int _couchPlayerID) {
		if (_couchPlayerID == 1) {
			Rect _rect0 = new Rect (0f, 0.5f, 0.5f, 0.5f);
			cam.rect = _rect0;
		} else if (_couchPlayerID == 2) {
			Rect _rect1 = new Rect (0.5f, 0f, 0.5f, 0.5f);
			cam.rect = _rect1;
		} else if (_couchPlayerID == 3) {
			Rect _rect2 = new Rect (0f, 0.5f, 0.5f, 0.5f);
			cam.rect = _rect2;
		} else if (_couchPlayerID == 4) {
			Rect _rect3 = new Rect (0.5f, 0f, 0.5f, 0.5f);
			cam.rect = _rect3;
		}


		//Rect _rect0 = new Rect (0f, 0.5f, 0.5f, 0.5f);
		//Rect _rect1 = new Rect (0.5f, 0f, 0.5f, 0.5f);
		//Rect _rect2 = new Rect (0f, 0.5f, 0.5f, 0.5f);
		//Rect _rect3 = new Rect (0.5f, 0f, 0.5f, 0.5f);

		//camList [0].rect = _rect0;
		//camList [1].rect = _rect1;
		//camList [2].rect = _rect2;
		//camList [3].rect = _rect3;
	}


	public void OnCollosionEnter2D (Collision2D _col) {
		Debug.Log (_col.gameObject.layer);

		if (_col.gameObject.layer == 8) {
			Physics2D.IgnoreCollision (_col.collider, col);
		}


	}

	public void IsElevatorNear (bool _isNear, ElevatorScript _elevator) {
		elevatorIsNear = _isNear;
		elevator = _elevator;
	}

	private void DoElevatorMenu () {
		//Debug.Log ("ele- ele- elevator!");


		isOccupied = true;
		elevatorMenu.SetActive (true);
	}


	public void UseElevator (int _level) {
		elevatorMenu.SetActive (false);

		useElevator = UseElevatorCR (_level);
		StartCoroutine (useElevator);
	}

	private IEnumerator UseElevatorCR (int _level) {
		

		//move here
		Point _point = elevator.GridPos;
		Vector3 _pos = LevelManager.Instance.Tiles [_point].transform.position;

		Vector3 _direction = _pos - transform.position;

		Debug.Log (transform.position);
		Debug.Log (_pos);

		while (Vector2.Distance (transform.position, _pos) > 5f) {
			//Vector3 _vect = Vector3.MoveTowards (transform.position, _pos, speed * Time.deltaTime);
			_direction = _pos - transform.position;
			_direction.Normalize ();
			rb.MovePosition (transform.position + _direction * speed * Time.deltaTime);
			yield return new WaitForSeconds (0.005f);
		}


		//Debug.Log ("im tryin, im tryin!");

		/*
		rb.MovePosition (Vector2.down);

		if (_point.Y < _level) {
			//up
		} else {
			//down
		}

		//move here
		//_point.Y = _level;
		*/

		_point.Y = _level;
		_pos = LevelManager.Instance.Tiles [_point].transform.position;

		Debug.Log (transform.position);
		Debug.Log (_pos);

		while (Vector2.Distance (transform.position, _pos) > 0.5f) {
			Vector3 _vect = Vector3.MoveTowards (transform.position, _pos, Time.deltaTime);
			rb.MovePosition (transform.position + _vect * speed * Time.deltaTime);
			yield return new WaitForSeconds (0.05f);
		}
	}

	private IEnumerator Test () {
		while (true) {
			Vector3 _vect = Vector3.left;
			rb.MovePosition (transform.position + _vect * speed / 0.05f);

			yield return new WaitForSeconds (0.05f);
		}
	}
}