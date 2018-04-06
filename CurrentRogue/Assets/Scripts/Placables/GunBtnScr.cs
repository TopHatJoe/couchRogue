using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GunBtnScr : MonoBehaviour, IPointerClickHandler
{
	//private SpriteRenderer spriteRenderer;
	private Image objImg;
	private bool isPowered = false;
	public bool IsPowered { get { return isPowered; } }
	private bool isCharged = false;

	/*
	private bool isCharged = false;
	public bool IsCharged { get { return isCharged; } 
		set { 
			isCharged = value;  
			if (!isCharged) {
				StartCoroutine (ChargeWeapon ());
				Debug.Log ("weapon not yet ready");
			}
		} 
	}
	*/

	private static int counter;
	//private static int counter1;

	//private static List <GunBtnScr> gunBtns;
	//private static GunBtnScr[] gunBtns = new GunBtnScr[5];
	private WeaponScript weapon;

	[SerializeField]
	private int powerReq;
	public int PowerReq { get { return powerReq; } }

	//debug
	private bool notSynced = true;


	public void Setup (WeaponScript _wpScr)
	{
		//transform.SetParent (LevelManager.Instance.GunBtnPanel.transform.GetChild (counter));
		transform.SetParent (LevelManager.Instance.GunBtnPanel.transform);

		objImg = gameObject.GetComponent <Image> ();
		objImg.color = Color.gray;

		counter++;

		weapon = _wpScr;

		powerReq = weapon.PowerReq;

		//PowerManager.Instance.UpdatePowerDistribution2 (powerReq, 1);
		PowerManager.Instance.UpdateSystemCapacity (1, powerReq);
		PowerManager.Instance.GetGunBtn (this);
	}


	//private void OnMouseOver ()
	public void OnPointerClick (PointerEventData _eventData)
	{

		if (_eventData.button == PointerEventData.InputButton.Left) {
			GunLogic (true);
		} else if (_eventData.button == PointerEventData.InputButton.Right) {
			GunLogic (false);
		}

		//outsource logic to separate function
		/*
		if (_eventData.button == PointerEventData.InputButton.Left) {
			if (!isPowered) {
				TryPowerUp ();

				//debug
				if (notSynced) {
					weapon.StringGen ();
					notSynced = false;
				}
			} else {
				Target ();
			}
		} else if (_eventData.button == PointerEventData.InputButton.Right) {
			TryPowerDown ();
		}
		*/
	}


	public void GunLogic (bool _powerUp) {
		if (_powerUp) {
			if (!isPowered) {
				TryPowerUp ();

				//debug
				if (notSynced) {
					weapon.StringGen ();
					notSynced = false;
				}
			} else {
				Target ();
			}
		} else {
			TryPowerDown ();
		}
	}


	private void Target () {
		Buttons _btn = gameObject.GetComponent <Buttons> ();
		GameManager.Instance.PickRoom (_btn);
		_btn.SetPrefab ();
		//gives the placementMngr the position of the gun
		PlacementManager.Instance.GunPoint = weapon.GridPos;
	}


	public void ChargeBtn (bool _isCharged) {
		isCharged = _isCharged;

		if (isPowered) {
			if (_isCharged) {
				objImg.color = Color.green;
			} else {
				objImg.color = Color.white;
			}
		} else {
			objImg.color = Color.grey;
		}
	}


	public void TryPowerUp () {
		//debug //fixes IDontGenerateThere//BUG
		if (notSynced) {
			weapon.StringGen ();
			notSynced = false;
		}


		if (ReactorScript.DirectPower (powerReq)) {
			//Debug.Log ("powering up");

			//sync powerstate
			if (NetManager.Instance != null) {
				NetManager.Instance.SyncPowerState (weapon.GridPos, 1, true);

				/*
						objImg.color = Color.white;
						powered = true;
					} else {
						objImg.color = Color.white;
						powered = true;
						*/
			}

			//still needs to do this, since the btn statements cant be synced
			objImg.color = Color.white;
			isPowered = true;

			//sghalfefgyu
			PowerManager.Instance.RoutePower (1, powerReq);
		} else {
			Debug.Log ("not enough power");
		}
	}

	public void TryPowerDown () {
		if (isPowered) {
			//frees the power

			//sync powerstate
			if (NetManager.Instance != null) {
				NetManager.Instance.SyncPowerState (weapon.GridPos, 1, false);
			}

			ReactorScript.RedirectPower (powerReq);
			isPowered = false;

			PowerManager.Instance.RoutePower (1, -powerReq);
		} else {
			Debug.Log ("already powered down");
		}
	}


	/*
	public IEnumerator ChargeWeapon () {
		isCharged = false;
		objImg.color = Color.white;
		yield return new WaitForSeconds (3);
		isCharged = true;
		weapon.isCharged = isCharged;
		objImg.color = Color.green;

		//if has target: fire! //should be called via bool sync.
		weapon.CheckForTarget (isCharged);
	}
	*/

	/*
	private void OnMouseOver ()
	{
		if (Input.GetMouseButtonDown (0)) {
			if (!ready) {
				if (!powered) {
					spriteRenderer.color = Color.white;
					powered = true;
				} else {
					spriteRenderer.color = Color.green;
					ready = true;
				}
			} else {
				weapon.Fire ();
				ready = false;
				//powered = false;
				spriteRenderer.color = Color.white;


			}
		}

		if (Input.GetMouseButtonDown (1)) {
			spriteRenderer.color = Color.gray;
			powered = false;
			powered = false;
		}
		//Debug.Log ("meep");
	}
	*/


	/*
	public void Setup (WeaponScript _wpScr) {
		weapon = _wpScr;
	}


	public static void AssignButton (WeaponScript _wpScr) {
		if (counter1 < gunBtns.Length) {
			gunBtns [counter1].gameObject.SetActive (true);
			gunBtns [counter1].GetGun (_wpScr);
			counter1++;
		}
	}


	public void GetGun (WeaponScript _wpScr) {
		weapon = _wpScr;
	}
	*/
}