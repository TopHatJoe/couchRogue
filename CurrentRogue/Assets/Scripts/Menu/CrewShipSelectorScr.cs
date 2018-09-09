using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewShipSelectorScr : MonoBehaviour 
{
    [SerializeField]
    private GameObject blocker;

    [SerializeField]
    private Text selectionDisplay;

    private bool isActive = false;
    private string controllerID;
    public string ControllerID { get { return controllerID; } }

    //private bool hasSwitchedUp = false;
    //private bool hasSwitchedDown = false;

    private int selected = 0;
    public int Selected { get { return selected; } }
    //private Button[] buttons;
    private List<ShipInfo> ShipList { get { return CasheScript.Instance.ShipList; } }


    public void Setup (string _controllerID) {
        controllerID = _controllerID;
        isActive = true;

        if (ShipList.Count > 0)
        {
            selectionDisplay.text = (selected + ": " + ShipList[selected].ShipName);
        } else {
            selectionDisplay.text = "no ship"; 
        }

        blocker.SetActive(false);
    }

    private void Update() {
        if (isActive)
        {
            if (ShipList.Count != 0)
            {
                if (Input.GetButtonDown (controllerID + "-s")) {
                    selected++;

                    if (selected >= ShipList.Count)
                    {
                        selected = 0;
                    }

                    //buttons[selected].image.color = Color.cyan;
                    selectionDisplay.text = (selected + ": " + ShipList[selected].ShipName);

                    //hasSwitchedUp = true;
                }

                /*
                if (!hasSwitchedUp)
                {
                    if (Input.GetAxisRaw(controllerID + "-H") > 0.32f)
                    {
                        //buttons[selected].image.color = Color.gray;
                        selected += 1;

                        if (selected >= ShipList.Count)
                        {
                            selected = 0;
                        }
                        else if (selected < 0)
                        {
                            selected = (ShipList.Count - 1);
                        }

                        //buttons[selected].image.color = Color.cyan;
                        selectionDisplay.text = (selected + ": " + ShipList[selected].ShipName);

                        hasSwitchedUp = true;
                    }
                }
                else
                {
                    if (Input.GetAxisRaw(controllerID + "-H") < 0.32f)
                    {
                        hasSwitchedUp = false;
                    }
                }

                if (!hasSwitchedDown)
                {
                    if (Input.GetAxisRaw(controllerID + "-H") < -0.32f)
                    {
                        //buttons[selected].image.color = Color.gray;
                        selected -= 1;

                        if (selected >= ShipList.Count)
                        {
                            selected = 0;
                        }
                        else if (selected < 0)
                        {
                            selected = (ShipList.Count - 1);
                        }

                        //buttons[selected].image.color = Color.cyan;
                        selectionDisplay.text = (selected + ": " + ShipList[selected].ShipName);

                        hasSwitchedDown = true;
                    }
                }
                else
                {
                    if (Input.GetAxisRaw(controllerID + "-H") > -0.5f)
                    {
                        hasSwitchedDown = false;
                    }
                }
                */
            }
        }
    }
}