﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedBayScr : MonoBehaviour, ISystem {
    [SerializeField]
    //healthPointsPerSecond
    private int hpPS;

    private RoomScript room;

    //private bool isPowered;

    //[SerializeField]
    private SystemScript sysScr;
    private HealthScript hScr;

    private Point gridPos;

    private int playerID;

    private ShipScript ship;

    [SerializeField]
    private int powerReq;
    //improtant for "isThere enough power?" checks
    private int fullPwrReq = 0;

    [SerializeField]
    //the amount by which the evasiveness is hightened
    private int componentCapacity;

    private bool isPowered = false;
    //public bool IsPowered { get { return isPowered; } }

    private int systemType = 4;

    //private bool isDamaged = false;
    //private bool isLocal = false;

    private ShipPowerMngr pwrMngr;
    private MedBayScr originMedBayScr;
    private bool isOrigin = false;


    void Start()
    {
        //there might be a less convoluted way to do this ^^
        room = transform.parent.parent.GetChild(0).GetChild(0).GetComponent<RoomScript>().OriginObj.GetComponent<RoomScript>();

        //temporary mesure...
        fullPwrReq = powerReq;

        Setup();
    }

    private void Setup()
    {
        sysScr = GetComponent<SystemScript>();
        gridPos = sysScr.GridPos;
        playerID = gridPos.Z;

        ship = LevelManager.Instance.Ships[playerID].GetComponent<ShipScript>();
        pwrMngr = ship.GetComponent<ShipPowerMngr>();

        hScr = sysScr.GetOriginObj().GetComponent<HealthScript>();

        pwrMngr.PowerSetup(systemType, powerReq);

       
        originMedBayScr = GetOriginMedBay();
        if (this == originMedBayScr) {
            isOrigin = true;
        }
       
        //originEngScr.fullPwrReq += powerReq;

        if (isOrigin) {
            pwrMngr.AddToSysScrList(systemType, sysScr);

            StartCoroutine(HealRoutine());
        }
    }




    public void SyncedPower(bool _isPowered)
    {
        isPowered = _isPowered;
    }

  

    public void UpdateHealthState(bool _isFullyDamaged, bool _isFullyRepaired) {
        Debug.Log("engine: " + gridPos.X + ", " + gridPos.Y);

        if (_isFullyDamaged) {
            //PowerManager.Instance.DamageSystem (systemType, -powerReq);

            if (isPowered) {
                ReceivePowerUpdate(false);
            }

            pwrMngr.ApplyHealthState(systemType, powerReq, isPowered, this);
            //isPowered = false;
        }

        if (_isFullyRepaired) {
            pwrMngr.ApplyHealthState(systemType, -powerReq, isPowered, this);
        }
    }



    //to all
    public void ReceivePowerUpdate(bool _isPowered)
    {
        if (isOrigin) {
            if (isPowered) {
                SystemScript _sysScr = sysScr.GetOriginObj().GetComponent<SystemScript>();
                _sysScr.UpdatePowerState(false);
            } else {
                if (!hScr.IsFullyDamaged) {
                    if (pwrMngr.EnoughPower(fullPwrReq)) {
                        //try power up
                        SystemScript _sysScr = sysScr.GetOriginObj().GetComponent<SystemScript>();
                        _sysScr.UpdatePowerState(true);
                    } else {
                        Debug.LogError("not enough power");
                    }
                }
            }
        }
        else
        {
            originMedBayScr.ReceivePowerUpdate(_isPowered);
        }
    }

    public void UpdatePowerState(bool _isPowered)
    {
        //at this point we know theres enough power and can power down or up
        if (isPowered) {
            //try power down
            pwrMngr.PowerDistribution(systemType, -powerReq, this);
            pwrMngr.UpdateReactor(powerReq);

            ship.IncreaseEvasionChance(-componentCapacity);

            isPowered = false;
        } else {
            pwrMngr.PowerDistribution(systemType, powerReq, this);

            ship.IncreaseEvasionChance(componentCapacity);

            isPowered = true;
        }

        sysScr.IsPowered = isPowered;
    }

    public bool IsPowered()
    {
        return isPowered;
    }

    private MedBayScr GetOriginMedBay()
    {
        MedBayScr _medBayScr = sysScr.GetOriginObj().GetComponent<MedBayScr>();
        return _medBayScr;
    }


    private IEnumerator HealRoutine () {
        while (true) {
            yield return new WaitForSeconds(1f);
            if (isPowered) {
                room.HurtPresentHScr(hpPS);
            }
        }
    }
}