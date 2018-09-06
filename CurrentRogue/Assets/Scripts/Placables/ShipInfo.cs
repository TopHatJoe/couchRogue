using UnityEngine;

public class ShipInfo
{
    private string shipName;
    public string ShipName { get { return shipName; } }

    private string type;
    public string Type { get { return type; } }

    private string str;
    public string Str { get { return str; } }

    private int ownerID = 0;
    public int OwnerID { get { return ownerID; } }


    public void SetShipInfo(string _name, string _type, string _str, int _ownerID) {
        shipName = _name;
        type = _type;
        str = _str;
        ownerID = _ownerID;

        Debug.Log("name: " + shipName + ", type: " + type + ", owner: " + ownerID);
    }
}