using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lobbyScript : MonoBehaviour {
    [SerializeField]
    private GameObject shipPanel;
    [SerializeField]
    private NetManager netMngr;
    [SerializeField]
    private GameObject shipInfo;


    public void GetShipType (Text _shipName) {
        netMngr.GetShipType(_shipName);

        GameObject _obj = (GameObject)Instantiate(shipInfo);
        lobbyShipInfo _infoScr = _obj.GetComponent<lobbyShipInfo>();
        _infoScr.Setup(_shipName.text);

        _obj.transform.SetParent(shipPanel.transform);
    }



    public void LoadShipInfo (Text _shipName) {
        //netMngr.GetShipType(_shipName);

        //get type
        string _shipType = Player.Instance.LoadShipType(_shipName.text);
        //get shipStr
        string _shipStr = Player.Instance.LoadShip(_shipName.text);

        netMngr.SendShipInfo(_shipName.text, _shipType, _shipStr);


        GameObject _obj = (GameObject)Instantiate(shipInfo);
        lobbyShipInfo _infoScr = _obj.GetComponent<lobbyShipInfo>();
        _infoScr.Setup(_shipName.text);

        _obj.transform.SetParent(shipPanel.transform);
    }
}