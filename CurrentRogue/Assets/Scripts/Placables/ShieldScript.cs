using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour 
{
	/* 010518
	[SerializeField]
	private GameObject[] shieldLayer;

	private int playerID;

	//[SerializeField]
	private SpriteRenderer sprRenderer;
	//private Sprite spr;

	[SerializeField]
	//should be defined by the system
	private int maxShieldHP;
	//the max when taking damage in consideration
	//private int shieldHP;
	private int power;
	public int Power { get { return power; } 
		set { if (NetManager.Instance != null) {
				NetManager.Instance.SyncShield (value, playerID);
			} power = value; 
		} 
	}

	private int maxPower = 8;
	//the current hp
	private int currentHP;

	//255 divided by hp.
	private int alphaUnit;

	[SerializeField]
	private int chargeTime;

	private int powerReq = 2;
	//private bool isPowered;

	private IEnumerator chargeShield;
	*/

	//maximum amount a shield can be powered by.
	private int maxCapacity = 8;
	//the amount of hp the shield
	private int currentCapacity;
	//the maximum shield hp is the lower of the above...

	//the current shield hp
	private int currentAmount;

	private SpriteRenderer sprRenderer;
	private float alphaUnit;

	[SerializeField]
	private float chargeTime;



	public void Setup (int _ID) {
		//Debug.LogError ("shield stuff called");

		sprRenderer = gameObject.GetComponent <SpriteRenderer> ();

		GameObject _ship = transform.parent.gameObject;
		GameObject _field = _ship.transform.GetChild (1).gameObject;

		transform.position = _field.transform.position;
		float _scale0 = (_field.transform.localScale.x * 16 / 1920);
		float _scale1 = (_field.transform.localScale.y * 16 / 1080);

		transform.localScale = new Vector3 (_scale0, _scale1);

		alphaUnit = (1f / maxCapacity);
		Debug.Log ("shield blah " + (alphaUnit));

		UpdateShieldAlpha ();

		StartCoroutine (ChargeShield ());
	}

	public void IncreaseCapacity (int _amount) {
		currentCapacity += _amount;

		if (_amount < 0) {
			if (currentAmount > currentCapacity) {
				currentAmount = currentCapacity;
				UpdateShieldAlpha ();
			}
		}
	}

	public void RemoteShield (int _amount) {
		//is called by cmd stuff... 
	}

	private void UpdateShieldAlpha () {
		Color _color = sprRenderer.color;
		_color.a = (currentAmount * alphaUnit);

		sprRenderer.color = _color;
		//Debug.Log ("shield alpha blah " + (sprRenderer.color.a));
		//Debug.Log ("shield current blah " + (currentCapacity));
	}

	private IEnumerator ChargeShield () {
		while (true) {
			if (currentAmount < maxCapacity && currentAmount < currentCapacity) {
				float _elapsedTime = 0;
				while (_elapsedTime < chargeTime) {
					_elapsedTime += 0.2f;
					yield return new WaitForSeconds (0.2f);
				}

				if (currentAmount < maxCapacity && currentAmount < currentCapacity) {
					currentAmount++;
					UpdateShieldAlpha ();
				} else {
					Debug.Log ("power was reduced");
				}
			}

			yield return new WaitForSeconds (0.32f);
		}
	}

	/* 010518
	public void Setup (int _ID) {
		//transform.position = _pos;
		playerID = _ID;

		//spr = gameObject.GetComponent <Sprite> ();
		sprRenderer = gameObject.GetComponent <SpriteRenderer> ();

		/* 220418
		if (maxShieldHP != 0) {
			//currentHP = maxShieldHP;
			currentHP = 0;
			AdjustAlpha ();

			//alphaUnit = Mathf.RoundToInt (255 / currentHP);
			alphaUnit = Mathf.RoundToInt (255 / maxShieldHP);
		} else {
			Debug.LogError ("shieldHP == 0!");
		}
		////

		GameObject _ship = transform.parent.gameObject;
		GameObject _field = _ship.transform.GetChild (1).gameObject;

		transform.position = _field.transform.position;
		//transform.localScale = (_field.transform.localScale * 16 / 1920);
		//float _scale = (Screen.width / shield.GetComponent <SpriteRenderer> ().bounds.size.x);
		float _scale0 = (_field.transform.localScale.x * 16 / 1920);
		float _scale1 = (_field.transform.localScale.y * 16 / 1080);
		//shield.transform.localScale = new Vector3 (_scale, _scale, _scale); 

		transform.localScale = new Vector3 (_scale0, _scale1);

		/*
		if (NetManager.Instance != null) {
			if (playerID == NetManager.Instance.localPlayerID) {
				PowerManager.Instance.GetShield (this);
				PowerManager.Instance.UpdateSystemCapacity (0, maxPower);
			}
		} else {
			Debug.LogError ("NetManager is null!");
		}
		////

		//Debug.LogError ("loopTIme!"); 
		chargeShield = ChargeShield ();
		StartCoroutine (chargeShield);
	}



	private void OnTriggerEnter2D (Collider2D _col) {
		if (currentHP > 0) {
			AmmoScript _ammo = _col.GetComponent <AmmoScript> ();
			//Destroy (_ammo.gameObject);

			if (_ammo.PlayerID == playerID) {
				Debug.Log ("'ello");
			} else {
				currentHP -= _ammo.Damage;

				Destroy (_ammo.gameObject);

				/* 220418
				int _ID = _ammo.PlayerID;

				Destroy (_ammo.gameObject);
				currentHP--;

				AdjustAlpha ();
				////

				/*
				Color _colour = sprRenderer.color;
				float _tmp1 = alphaUnit * shieldHP;
				float _tmp2 = _tmp1 / 255;
				_colour.a = _tmp2;
				sprRenderer.color = _colour;
				////
			}
		}

		/*
		//if (_ID == playerID) {
			//print ("hows it hangin?");
		//} else {
			Destroy (_ammo.gameObject);

			shieldHP--;
			
		Debug.Log (shieldHP + ", " + alphaUnit);

		Color _colour = sprRenderer.material.color;
		//Color _colour = sprRenderer.color;
			


		//_colour.a = alphaUnit * shieldHP;
		float _a = (alphaUnit * shieldHP) / 255;

		float _tmp1 = alphaUnit * shieldHP;
		float _tmp2 = _tmp1 / 255;

		Debug.Log (alphaUnit + " * " + shieldHP + " = " + alphaUnit * shieldHP);
		Debug.Log (alphaUnit * shieldHP + " / " + 255 + " = " + alphaUnit * shieldHP / 255);
		Debug.Log (_tmp2);

		_colour.a = _tmp2;

		Debug.Log (_a);

		//sprRenderer.color = new Color (1, 1, 1, _a);
		sprRenderer.material.color = _colour;
		//}
		////
	}

	//could be put in update()
	private IEnumerator ChargeShield () {
		while (true) {
			//tmp
			if (currentHP < power / 2) {
				float _elapsedTime = 0;
				while (_elapsedTime < chargeTime) {
					_elapsedTime += 0.1f;
					yield return new WaitForSeconds (0.1f);
				}

				if (currentHP < power / 2) {
					currentHP++;
					AdjustAlpha ();
				} else {
					Debug.Log ("power was reduced");
				}
			

				Debug.Log (power + ", HP: " + currentHP);
			} 

			/*
			if (currentHP > power / 2) {
				currentHP--;
				AdjustAlpha ();
			}
			////

			yield return new WaitForSeconds (0.32f);
		}
	}


	private void AdjustAlpha () {
		Color _colour = sprRenderer.color;
		float _tmp1 = alphaUnit * currentHP;
		float _tmp2 = _tmp1 / 255;
		_colour.a = _tmp2;

		//Debug.Log (_tmp2);

		sprRenderer.color = _colour;
	}

	public void SetMax (int _amount)
	{
		maxShieldHP += _amount;
	}

	public void RemoteShield (int _power) {
		power = _power;
		AdjustAlpha ();

		PowerCheckLoop ();
		/*
		while (true) {
			if (currentHP > power / 2) {
				//if (chargeShield != null) {
				//	StopCoroutine (chargeShield);
				//}

				currentHP--;
				AdjustAlpha ();
				//} else if (currentHP = power / 2) {
				//if (chargeShield != null) {
				//	StopCoroutine (chargeShield);
				//}

				//	break;
			} else {
				break;
			}
		}
		////
	}

	private void PowerCheckLoop () {
		while (true) {
			if (currentHP > power / 2) {
				currentHP--;
				AdjustAlpha ();
			} else {
				break;
			}
		}
	}


	/*
	public void TryPowerUp () {
		if (power + 2 <= maxPower) {
			if (ReactorScript.DirectPower (powerReq)) {
				Debug.Log ("pre: " + power + ", HP: " + currentHP);


				//Debug.Log ("powering up");

				//sync powerstate
				//if (NetManager.Instance != null) {
				//	NetManager.Instance.SyncPowerState (weapon.GridPos, true);
				//}

				//still needs to do this, since the btn statements cant be synced
				//objImg.color = Color.white;

				//isPowered = true;
				Power += 2;

				//sghalfefgyu
				PowerManager.Instance.RoutePower (0, powerReq);

				Debug.Log ("post: " + power + ", HP: " + currentHP);

			} else {
				Debug.Log ("not enough power");
			}
		}
	}

	public void TryPowerDown () {
		if (power - 2 >= 0) {
			Debug.Log ("pre: " + power + ", HP: " + currentHP);


			//sync powerstate
			//if (NetManager.Instance != null) {
			//	NetManager.Instance.SyncPowerState (weapon.GridPos, false);
			//}

			ReactorScript.RedirectPower (powerReq);
			//isPowered = false;
			Power -= 2;

			PowerCheckLoop ();
			/*
			while (true) {
				if (currentHP > power / 2) {
					//if (chargeShield != null) {
					//	StopCoroutine (chargeShield);
					//}

					currentHP--;
					AdjustAlpha ();
				//} else if (currentHP = power / 2) {
					//if (chargeShield != null) {
					//	StopCoroutine (chargeShield);
					//}

				//	break;
				} else {
					break;
				}
			}
			////

			PowerManager.Instance.RoutePower (0, -powerReq);

			Debug.Log ("post: " + power + ", HP: " + currentHP);

		} else {
			Debug.Log ("already powered down");
		}
	}
	*/

	/*
	private void OnTriggerStay (Collider2D _col) {
		print ("old Col!");
	}
	*/
}