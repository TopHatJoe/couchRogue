using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour, IPlacable //,ISystem
{
	[SerializeField]
	private int damage;

	//the parent tile
	private TileScript tile;
	//the gridPos
	private Point gridPos;
	public Point GridPos { get { return gridPos; } }

	[SerializeField]
	private string objStr;

	private string saveStr;

	//to fire weapons
	[SerializeField]
	private GameObject ammoSpawn;
	[SerializeField]
	private GameObject ammo;
	public GameObject Ammo { get { return ammo; } }

	[SerializeField]
	private GameObject gunBtnObj;

	private GunBtnScr gunBtn;

	public GameObject targetObj;

	[SerializeField]
	private int powerReq;
	public int PowerReq { get { return powerReq; } }

	private bool isPowered;
	public bool IsPowered { get { return isPowered; } set { HandleCharge (value); isPowered = value; } }

	public bool isCharged;
	private bool hasTarget;

	private float angleTmp;

	[SerializeField]
	private float chargeTime;
	private IEnumerator chargeLoop;

	[SerializeField]
	private GameObject chargeBar;

	private int[] probArr;
	private int probCounter;

	//TMP
	void Start ()
	{
		SetButton ();
		ResetBar ();


		//GunBtnScr.AssignButton (this);

	}


	private void SetButton () {
		if (NetManager.Instance != null) {
			if (gridPos.Z == NetManager.Instance.localPlayerID) {
				GameObject _obj = (GameObject)Instantiate (gunBtnObj);
				_obj.transform.SetParent (LevelManager.Instance.GunBtnPanel.transform);
				gunBtn = _obj.GetComponent <GunBtnScr> ();
				//_obj.transform.SetParent (LevelManager.Instance.GunBtnPanel.transform);
				//gunBtn.GetGun (this);
				gunBtn.Setup (this);
			}
		}
	}


	public void PlaceObj (int _index, Point _gridPos, GameObject _originObj) {
		gridPos = _gridPos;
		tile = LevelManager.Instance.Tiles [gridPos];

		saveStr = (objStr + ",7," + gridPos.X.ToString () + "," + gridPos.Y.ToString ());
		LevelManager.Instance.parameterList.Add (saveStr);

		transform.SetParent (tile.transform);

		//DEBUG
		tile.WeaponPlacable = false;

		GameManager.Instance.Buy ();
	}


	public void RemoveObj () {
		tile.WeaponPlacable = true;

		LevelManager.Instance.parameterList.Remove (saveStr);

		Destroy (gameObject);
	}


	public void DestroyPreviousTarget (GameObject _targetObj) {
		if (targetObj != null) {
			//Destroy (targetObj.transform.parent.gameObject);
			targetObj.transform.parent.gameObject.SetActive (false);
		}

		targetObj = _targetObj;
	}

	public void Fire (float _angle)
	{
		//GameObject _obj = (GameObject)
		//Debug.Log ("FIRE");
		GameObject _obj = Instantiate (ammo, ammoSpawn.transform.position, Quaternion.identity);
		//wtf is this even necessary?
		if (_obj.GetComponent <AmmoScript> () != null) {
			_obj.GetComponent <AmmoScript> ().Fire (targetObj, probArr [probCounter], _angle, gridPos.Z, damage);
		} else {
			Debug.LogError ("NO AMMO!!!");
		}
		probCounter++;

		isCharged = false;
		if (gunBtn != null) {
			gunBtn.ChargeBtn (false);
		}

		HandleCharge (isPowered);


		//may cause harm
		hasTarget = false;
		//StartCoroutine (gunBtn.ChargeWeapon ());


		//also calls coroutine
		//gunBtn.IsCharged = false;


	}

	public void TryFire (float _angle) {
		if (isCharged) {
			Fire (_angle);
		} else {
			hasTarget = true;
			angleTmp = _angle;
		}
	}

	public void FirePower (bool _isPowered) {
		isPowered = _isPowered;
		HandleCharge (_isPowered);

		/*
		if (isPowered) {
			Debug.LogError ("powered Up");
		} else {
			Debug.LogError ("powered Down");
		}
		*/
	}

	private void HandleCharge (bool _isPowered) {
		if (_isPowered) {
			chargeLoop = ChargeLoop ();
			StartCoroutine (chargeLoop);
		} else {
			StopCoroutine (chargeLoop);
			isCharged = false;
			ResetBar ();

			if (gunBtn != null) {
				gunBtn.ChargeBtn (false);
			}
		}
	}

	private IEnumerator ChargeLoop () {
		float _elapsedTime = 0;

		while (_elapsedTime < chargeTime) {
			
			//_elapsedTime += Time.deltaTime;

			_elapsedTime += 0.01f;

			//updates bar
			float _percentage = 100 / chargeTime * _elapsedTime;
			chargeBar.transform.localScale = new Vector3 ((_percentage / 100), 1);

			yield return new WaitForSeconds (0.01f);

			//Debug.Log (_elapsedTime);
		}

		//isCharged = true;
		//gunBtn.ChargeBtn (true);


		if (hasTarget) {
			//yield return new WaitForSeconds (0.01f);

			Fire (angleTmp);
		} else {
			isCharged = true;

			if (gunBtn != null) {
				gunBtn.ChargeBtn (true);
			}
		}

		//Debug.Log ("weaponWasCharged");
	}


	private void ResetBar () {
		chargeBar.transform.localScale = new Vector3 (0, 1);
	}

	//should be called after all ships have been placed and before the match begins
	private void GenerateProbabiltyString () {
		//probabilityArr = new int[512];
		string _probStr = "";

		/*
		for (int i = 0; i < probabilityArr.Length; i++) {
			probabilityArr [i] = Random.Range (1, 100);
		}
		*/
	
		for (int i = 0; i < 512; i++) {
			_probStr += Random.Range (1, 100);

			if (i < 512-1) {
				_probStr += ",";
			}
		}

		//Debug.Log (_probString);

		Vector3 _pos = new Vector3 (gridPos.X, gridPos.Y, gridPos.Z);

		//syncs probabilty string
		NetManager.Instance.SyncProbabilityString (_pos, _probStr);
	}


	public void ReceiveProbStr (string _probStr) {
		string[] _strArr = _probStr.Split (',');
		probArr = new int[_strArr.Length];

		for (int i = 0; i < _strArr.Length; i++) {
			probArr[i] = int.Parse (_strArr[i]);
		}

		Debug.Log (_probStr);
	}

	//debug
	public void StringGen () {
		if (NetManager.Instance != null) {
			GenerateProbabiltyString ();
		}
	}



	/* 
	public void UpdateHealth (int _amount) {
		Debug.Log ("took Damage");
	}
	*/


	//public void OnDamage () {

	//}

	/*
	public void TryFire (float _angle) {
		if (isCharged) {
			Fire (_angle);
		} else {
			//temporarily saves the angle
			angleTmp = _angle;
			hasTarget = true;
		}
	}

	public void CheckForTarget (bool _isCharged) {
		if (_isCharged) {
			if (hasTarget) {
				Fire (angleTmp);
			} else {
				isCharged = true;
			}
		}
	}
	*/


	public GameObject GetOriginObj () {
		return gameObject;
	}

	public void UpdateHealthState (bool _isFullyDamaged, bool _isFullyRepaired) {

	}
}