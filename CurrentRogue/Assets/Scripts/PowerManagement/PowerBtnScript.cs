using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PowerBtnScript : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private int sysType;

	private int powerUsage;
	//powerCapacity == barList.count;

	//overall capacity
	private int maxPowerCapacity;
	//[SerializeField]
	private int damage;
	//current capacity //maxCap - dmg
	private int powerCapacity; //{ get { return maxPowerCapacity - damage; } }
	private List <GameObject> barList = new List <GameObject> ();
	[SerializeField]
	private GameObject barPanel;
	[SerializeField]
	private GameObject powerBar;

	//weapon with req 3 would be a chunk of 3
	//private List <int> powerChunks = new List <int> ();
	private List <GunBtnScr> gunBtnList = new List <GunBtnScr> (); 

	private List <EngineScript> engineList = new List <EngineScript> ();

	//private List <ShieldScript> shield = new List <ShieldScript> (); 
	private ShieldScript shield;


	public void UpdateBars (int _amount) {
		if ((powerUsage + _amount) <= powerCapacity) {
			powerUsage += _amount;

			for (int i = 0; i < powerCapacity; i++) {
				barList [i].GetComponent <Image> ().color = Color.grey;
			}

			for (int i = 0; i < powerUsage; i++) {
				barList [i].GetComponent <Image> ().color = Color.green;
			}
		} else {
			Debug.LogError ("not enough capacity!");
		}
	}

	public void IncreaseMaxCapacity (int _amount) {
		for (int i = 0; i < _amount; i++) {
			GameObject _obj = (GameObject) Instantiate (powerBar, barPanel.transform);
			barList.Add (_obj);

			_obj.GetComponent <Image> ().color = Color.grey;

			//number of bars
			maxPowerCapacity++;
			//number of available bars
			powerCapacity++;
		}
	}

	//updates number of available powerBars and powersDown if necessary
	public void UpdateCurrentCapacity (int _amount) {
		Debug.Log ("updatrin cap | amount: " + _amount);

		//negate all UI damage
		int y = barList.Count - 1;
		for (int x = 0; x < damage; x++) {
			//Debug.Log ("damage: " + damage + " | x: " + x + " | y: " + y + " | y-x: " + (y - x)); 

			//grey because the damaged ones cant be powered anyways
			barList [y - x].GetComponent <Image> ().color = Color.grey;
		}

		powerCapacity += _amount;
		damage -= _amount;

		//UI feedback Dmg //if () should check if the entire thing is down already
		if (damage <= barList.Count) {
			//int y = barList.Count - 1;
			for (int x = 0; x < damage; x++) {
				//Debug.Log ("damage: " + damage + " | x: " + x + " | y: " + y + " | y-x: " + (y - x)); 
				barList [y - x].GetComponent <Image> ().color = Color.red;
			}
		} else {
			Debug.Log ("system already erradicated");
		}
		///UI feedback Dmg

		while (powerUsage > powerCapacity) {
			UncheckChunk ();
		}

		//if _amount is negative, update UI
		if (_amount < 0) {
			
		}
	}

	//gets btn input
	public void OnPointerClick (PointerEventData _eventData)
	{
		Debug.Log ("Pre: maxCap: " + maxPowerCapacity + " | currentCap: " + powerCapacity + " | used: " + powerUsage);

		if (_eventData.button == PointerEventData.InputButton.Left) {
			//Debug.Log ("clicked!");
			CheckChunk ();
		} else if (_eventData.button == PointerEventData.InputButton.Right) {
			//Debug.Log ("right clicked!");
			UncheckChunk ();
		}

		Debug.Log ("Post: maxCap: " + maxPowerCapacity + " | currentCap: " + powerCapacity + " | used: " + powerUsage);
		//Debug.Log ("not clicked!");

	}

	//power up?
	private void CheckChunk () {

		//Debug.Log ("check!");

		//shield
		if (sysType == 0) {
			//shield.Power += 2;
			shield.TryPowerUp ();

		//weapon
		} else if (sysType == 1) {
			//gunBtnList.Count 
			//Debug.Log ("chunk!");



			//what is the lowest non powered chunk?
			//int _x = 0;
			for (int i = 0; i < gunBtnList.Count; i++) {
				
				if (gunBtnList [i].IsPowered) {
					//_x += gunBtnList [i].PowerReq;
					//Debug.Log ("nay!");

				} else {
					gunBtnList [i].TryPowerUp ();
					//Debug.Log ("yay!");
					break;
				}
			}

		//engine
		} else if (sysType == 2) {
			for (int i = 0; i < engineList.Count; i++) {
				if (engineList [i].IsPowered) {
					
				} else {
					engineList [i].TryPowerUp ();
					break;
				}
			}
		}
	}

	//power down?
	private void UncheckChunk () {
		//shield
		if (sysType == 0) {
			shield.TryPowerDown ();


		//weapon
		} else if (sysType == 1) {
			for (int i = gunBtnList.Count; i > 0; i--) {

				if (!gunBtnList [i-1].IsPowered) {

				} else {
					gunBtnList [i-1].TryPowerDown ();
					break;
				}
			}
		}

		//engines
		else if (sysType == 2) {
			for (int i = engineList.Count; i > 0; i--) {

				if (!engineList [i-1].IsPowered) {

				} else {
					engineList [i-1].TryPowerDown ();
					break;
				}
			}
		}
	}


	public void GetGunBtns (GunBtnScr _gunBtn) {
		gunBtnList.Add (_gunBtn);
	}

	public void GetShield (ShieldScript _shield) {
		//shieldSysList.Add (_shieldSys);
		shield = _shield;
	}

	public void GetEngines (EngineScript _engine) {
		engineList.Add (_engine);
	}


	public void RemoteShutdown () {
		UncheckChunk ();
	}

	public bool HasCapacity (int _amount) {
		int _capacity = maxPowerCapacity - (powerUsage + damage + _amount);
		Debug.Log ("max: " + maxPowerCapacity + " | used: " + powerUsage + " | dmg: " + damage + " | amount: " + _amount + " || x: " + _capacity);
		if (_capacity < 0) {
			return false;
		} else {
			return true;
		}
	}


	public void KeyWeapon (int _index, bool _value) {
		if (gunBtnList.Count > _index) {
			gunBtnList [_index].GunLogic (_value);
		} else {
			Debug.LogError ("not nuff gunz fool!");
		}
	}
}