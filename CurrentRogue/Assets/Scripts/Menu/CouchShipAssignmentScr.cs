using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouchShipAssignmentScr : MonoBehaviour 
{
    [SerializeField]
    private CrewShipSelectorScr[] selectorScrArr;
    //private string[][] crewSelectStr;
    private List<List<string>> crewAssignments = new List<List<string>> ();

    private int numOfLocalPlayers;

    private void Start() {
        SelectorSetup();
    }

    private void SelectorSetup() {
        //a list would probably do the trick...
        Dictionary<int, string> _ctrlDict = CasheScript.Instance.CtrlDict;

        int _i = 0;
        foreach (var _ctrl in _ctrlDict) {
            //Debug.Log("oi! " + _i);
            selectorScrArr[_i].Setup(_ctrl.Value);
            _i++;
        }

        numOfLocalPlayers = _i;
    }

    private void OnDestroy()
    {
        CollectInputs();
    }

    private void CollectInputs () {
        for (int i = 0; i < CasheScript.Instance.ShipList.Count; i++) {
            crewAssignments.Add(new List<string>());
        }

        for (int i = 0; i < numOfLocalPlayers; i++) {
            crewAssignments[selectorScrArr[i].Selected].Add(selectorScrArr[i].ControllerID);
        }

        CasheScript.Instance.GetCrewAssignments(crewAssignments);
    }
}