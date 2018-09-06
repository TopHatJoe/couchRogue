using UnityEngine;
using UnityEngine.UI;


public class lobbyShipInfo : MonoBehaviour 
{
    [SerializeField]
    private Text shipNameText; 

    public void Setup (string _shipName) {
        shipNameText.text = _shipName;
    }

    public void RemoveShip () {
        Debug.Log("bye!");
        Destroy(gameObject);
    }
}