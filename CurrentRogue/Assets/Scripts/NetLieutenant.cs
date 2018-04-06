using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetLieutenant : NetworkBehaviour 
{
	public void LoadShipsLoop ()
	{
		if (isServer) {
			for (int i = 0; i < NetManager.Instance.playerList.Count; i++) {
				//Debug.Log ("Loading Ship");
				NetManager.Instance.playerList [i].RpcLoadLocalShip ();
			}
		} else {
			//Debug.LogError ("THAT AINT NO SERVER!");
		}
	}

	[ClientRpc]
	public void RpcIni ()
	{
		//Debug.Log ("rpcIni");
		NetManager.Instance.InitializeGame ();
	}
}