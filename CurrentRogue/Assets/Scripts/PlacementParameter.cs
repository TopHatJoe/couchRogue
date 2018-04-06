	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//see point for reference
public struct PlacementParameter
{
	//x coordinate
	int pPX { get; set; }
	//y coordinate
	int pPY { get; set; }
	//objType
	int pPT { get; set; }
	//onjReference
	int pPR { get; set; }
	//onjWidth
	int pPW { get; set; }

	//s for saved
	public PlacementParameter (int sPPX, int sPPY, int sPPT, int sPPR, int sPPW = 0)
	{
		pPX = sPPX;
		pPY = sPPY;
		pPT = sPPT;
		pPR = sPPR;
		pPW = sPPW;
	}
}