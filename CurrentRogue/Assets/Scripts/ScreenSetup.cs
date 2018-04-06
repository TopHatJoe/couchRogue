using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSetup : MonoBehaviour 
{
	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private Camera[] otherCameras;


	[SerializeField]
	private GameObject displayField;
	[SerializeField]
	private GameObject gridField;

	[SerializeField]
	private GameObject[] gridFields;

	[SerializeField]
	private GameObject gunBtnField;


	public void Setup ()
	{
		SetCamera ();
		SetDisplayField ();
		GridFieldSetup ();
	}

	private void SetCamera ()
	{
		//let it set up all cams simultaniously...

		//sets the camera to make 1unit = 1px
		mainCamera.GetComponent<Camera> ().orthographicSize = (Screen.height * 5);

		//sets the size of other cams
		//for (int i = 0; i < otherCameras.Length; i++) {
		for (int i = 0; i < LevelManager.Instance.NumOfShips; i++) {
			otherCameras [i].GetComponent<Camera> ().orthographicSize = (Screen.height * 5);
		}

		//debugging
		//float screenTileRatio = (Screen.width / 48);
		//Debug.Log ("screen hight: " + Screen.height + "\nscreen width: " + Screen.width);
		//Debug.Log ("ratio = " + screenTileRatio);

		float screenSize = Screen.width;
		//float numOfTile = 48;
		//float otherScreenTileRatio = (screenSize / numOfTile);
		//Debug.Log ("screen hight: " + Screen.height + "\nscreen width from float: " + screenSize + "\nnumOfTile: " + numOfTile);
		//Debug.Log ("other ratio = " + otherScreenTileRatio);
	}

	private void SetDisplayField ()
	{
		//sets the display field to match the screenRes
		displayField.transform.localScale = new Vector3 (Screen.width, Screen.height, 1);
	}

	private void GridFieldSetup ()
	{
		//the pivot is set to left, the position is changed to the middle-left scene
		//gridField.transform.position = Camera.main.ScreenToWorldPoint(new Vector3 (0, ((Screen.height / 2) - (((Screen.width / 16) * 9) / 2)), 20));
		//the gridfield is set to match the width of the screen, and use that to create a 16:9 ratio
		//gridField.transform.localScale = new Vector3 (Screen.width, ((Screen.width / 16) * 9), 0);

		//for (int i = 0; i < gridFields.Length; i++) {
		for (int i = 0; i < LevelManager.Instance.NumOfShips; i++) {
			gridFields [i].transform.position = otherCameras [i].GetComponent <Camera> ().ScreenToWorldPoint (new Vector3 (0, ((Screen.height / 2) - (((Screen.width / 16) * 9) / 2)), 20));
			gridFields [i].transform.localScale = new Vector3 (Screen.width, ((Screen.width / 16) * 9), 0);
		}
	}
}