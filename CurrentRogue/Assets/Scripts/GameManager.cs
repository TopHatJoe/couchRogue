using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
	public Buttons ClickedBtn { get; set; }

	[SerializeField]
	private Text moneyTxt;

	[SerializeField]
	private GameObject dragHandler;

	private int money;
	public int Money 
	{ 
		get 
		{ 
			return money; 
		} 
		set 
		{ 
			this.money = value;
			this.moneyTxt.text = value.ToString() + " MONEY";
		} 
	}

	//[SerializeField]
	//private GameObject mainUI;



	void Start ()
	{
		Money = 10000000;
		//Money = 500;

		//if (CasheScript.Instance.CouchMode) {
		//	mainUI.SetActive (false);
		//}
	}

	void Update ()
	{
		HandleEscape ();
	}

	public void PickRoom (Buttons Btn)
	{
		if (Money >= Btn.Price)
		{
			this.ClickedBtn = Btn;
			Hover.Instance.Activate (Btn.Sprite);

			//disables dragBox img for placement
			if (dragHandler != null) {
				dragHandler.SetActive (false);
			}
		}
	}

	public void Buy()
	{
		if (Money >= PlacementManager.Instance.Price) 
		{
			Money -= PlacementManager.Instance.Price;
			//solves the deact-sprite-link-bug but isn't optimal
			Hover.Instance.Deactivate ();

			//reactivates dragbox img
			if (dragHandler != null) {
				dragHandler.SetActive (true);
			}
		}
		//ClickedBtn = null;
	}

	public void Sell ()
	{
		if (Money >= PlacementManager.Instance.Price) {
			Money += PlacementManager.Instance.Price;
		}
	}

	private void HandleEscape ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Hover.Instance.Deactivate ();

			//reactivates dragbox img
			if (dragHandler != null) {
				dragHandler.SetActive (true);
			}
		}
	}
}