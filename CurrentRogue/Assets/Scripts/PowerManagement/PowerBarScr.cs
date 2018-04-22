using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBarScr : MonoBehaviour 
{
	[SerializeField]
	private Image img;


	public void Recolour (Color _colour) {
		img.color = _colour;
	}
}