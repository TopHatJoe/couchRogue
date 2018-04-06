using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamSwitch : Singleton <CamSwitch> 
{
	[SerializeField]
	private string otherLoadName;

	[SerializeField]
	private Camera[] cams;
	[SerializeField]
	private Camera currentCam;

	//debug probably?
	public int currentCamID { get; private set; }


	/*
	void Start ()
	{
		currentCam = Camera.main;
		currentCamID = 0;

		//inverts y axis of alt cams (hopefully)
		//for (int i = 1; i < 4; i++) {
		for (int i = 1; i < cams.Length; i++) {
			Matrix4x4 mat = cams [i].GetComponent<Camera> ().projectionMatrix;
			mat *= Matrix4x4.Scale (new Vector3 (-1, 1, 1));
			cams [i].GetComponent<Camera> ().projectionMatrix = mat;
		}
	}
	*/


	void Start ()
	{
		//PlaceAllShips ();
	}

	void Update ()
	{
		if (Input.GetButtonDown ("Cam0"))
		{
			if (currentCam != null) {
				currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
			}
			cams [0].GetComponent<Camera> ().gameObject.SetActive (true);
			currentCam = cams [0];
			currentCamID = 0;
		}

		if (LevelManager.Instance.NumOfShips > 1) {
			if (Input.GetButtonDown ("Cam1")) {
				if (currentCam != null) {
					currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
				}
				cams [1].GetComponent<Camera> ().gameObject.SetActive (true);
				currentCam = cams [1];
				currentCamID = 1;
			}

			if (LevelManager.Instance.NumOfShips > 2) {
				if (Input.GetButtonDown ("Cam2")) {
					if (currentCam != null) {
						currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
					}
					cams [2].GetComponent<Camera> ().gameObject.SetActive (true);
					currentCam = cams [2];
					currentCamID = 2;
				}

				if (LevelManager.Instance.NumOfShips > 3) {
					if (Input.GetButtonDown ("Cam3")) {
						if (currentCam != null) {
							currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
						}
						cams [3].GetComponent<Camera> ().gameObject.SetActive (true);
						currentCam = cams [3];
						currentCamID = 3;
					}

					if (LevelManager.Instance.NumOfShips > 4) {
						if (Input.GetButtonDown ("Cam3")) {
							if (currentCam != null) {
								currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
							}
							cams [4].GetComponent<Camera> ().gameObject.SetActive (true);
							currentCam = cams [4];
							currentCamID = 4;
						}

						if (LevelManager.Instance.NumOfShips > 5) {
							if (Input.GetButtonDown ("Cam3")) {
								if (currentCam != null) {
									currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
								}
								cams [5].GetComponent<Camera> ().gameObject.SetActive (true);
								currentCam = cams [5];
								currentCamID = 5;
							}

							if (LevelManager.Instance.NumOfShips > 6) {
								if (Input.GetButtonDown ("Cam3")) {
									if (currentCam != null) {
										currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
									}
									cams [6].GetComponent<Camera> ().gameObject.SetActive (true);
									currentCam = cams [6];
									currentCamID = 6;
								}

								if (LevelManager.Instance.NumOfShips > 7) {
									if (Input.GetButtonDown ("Cam3")) {
										if (currentCam != null) {
											currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
										}
										cams [7].GetComponent<Camera> ().gameObject.SetActive (true);
										currentCam = cams [7];
										currentCamID = 7;
									}
								}
							}
						}
					}
				}
			}
		}

		/*
		if (Input.GetButtonDown ("Cam3"))
		{
			if (currentCam != null) {
				currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
			}
			cams [3].GetComponent<Camera> ().gameObject.SetActive (true);
			currentCam = cams [3];
			currentCamID = 3;
		}

		if (Input.GetButtonDown ("Cam4"))
		{
			if (currentCam != null) {
				currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
			}
			cams [4].GetComponent<Camera> ().gameObject.SetActive (true);
			currentCam = cams [4];
			currentCamID = 4;
		}

		if (Input.GetButtonDown ("Cam5"))
		{
			if (currentCam != null) {
				currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
			}
			cams [5].GetComponent<Camera> ().gameObject.SetActive (true);
			currentCam = cams [5];
			currentCamID = 5;
		}

		if (Input.GetButtonDown ("Cam6"))
		{
			if (currentCam != null) {
				currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
			}
			cams [6].GetComponent<Camera> ().gameObject.SetActive (true);
			currentCam = cams [6];
			currentCamID = 6;
		}

		if (Input.GetButtonDown ("Cam7"))
		{
			if (currentCam != null) {
				currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
			}
			cams [7].GetComponent<Camera> ().gameObject.SetActive (true);
			currentCam = cams [7];
			currentCamID = 7;
		}



		//flip cam y axis by btn command
		if (Input.GetKeyDown (KeyCode.F)) {
			Matrix4x4 mat = currentCam.GetComponent<Camera> ().projectionMatrix;
			mat *= Matrix4x4.Scale (new Vector3 (-1, 1, 1));
			currentCam.GetComponent<Camera> ().projectionMatrix = mat;
		}
		*/
	}


	public void SetCams ()
	{
		currentCam = Camera.main;
		currentCamID = 0;

		//inverts y axis of alt cams (hopefully)
		//for (int i = 1; i < 4; i++) {
		for (int i = 0; i < cams.Length; i++) {
			if (NetManager.Instance != null) {
				//if the ship isnt the players, it gets flipped
				if (i != NetManager.Instance.localPlayerID) {
					Matrix4x4 mat = cams [i].GetComponent<Camera> ().projectionMatrix;
					mat *= Matrix4x4.Scale (new Vector3 (-1, 1, 1));
					cams [i].GetComponent<Camera> ().projectionMatrix = mat;
				}	
			}
		}
	}


	public void PlaceAllShips ()
	{
		//Player.Instance.loadFileName = "ship0";
		//Player.Instance.Load ();

		//MADNESS


		Player tmpPlayer = Player.Instance;
		//Debug.Log ("PlayerInstance Acquired");

		tmpPlayer.loadFileName = otherLoadName;


		//for (int i = 0; i < 8; i++) {
		for (int i = 0; i < NetManager.Instance.playerList.Count; i++) {
			currentCamID = i;

			//not anymore -> NetManager.Instance.loadedShips [i][,]
			string tmpString = "ship" + i.ToString ();
			//Debug.Log (tmpString);

			tmpPlayer.loadFileName = tmpString;
			Player.Instance.LoadShip (tmpString);
		}

		currentCamID = 0;
		//currentCamID = NetManager.Instance.localPlayerID;


	}


	/*
	public void PlaceShips ()
	{
		Player tmpPlayer = Player.Instance;
		//Debug.Log ("PlayerInstance Acquired");

		tmpPlayer.loadFileName = otherLoadName;
		//Debug.Log ("changed player string");

		tmpPlayer.Load ();
		//Debug.Log ("Player loaded");
	}
	*/

	public void CorrectCams ()
	{
		int tmpLPID = NetManager.Instance.localPlayerID;

		if (currentCam != null) {
			currentCam.GetComponent<Camera> ().gameObject.SetActive (false);
		}
		cams [tmpLPID].GetComponent<Camera> ().gameObject.SetActive (true);
		currentCam = cams [tmpLPID];
		currentCamID = tmpLPID;
	}
}