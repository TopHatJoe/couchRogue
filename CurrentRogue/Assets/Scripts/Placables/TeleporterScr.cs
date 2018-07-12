using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterScr : MonoBehaviour, ISystem
{
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

    private int systemType = 5;

    //private bool isDamaged = false;
    //private bool isLocal = false;

    private ShipPowerMngr pwrMngr;
    private TeleporterScr originTeleporterScr;
    private bool isOrigin = false;

    private RoomScript room;



    void Start()
    {
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

        //ship.IncreaseEvasionChance (componentCapacity);

        pwrMngr.PowerSetup(systemType, powerReq);

        originTeleporterScr = GetOriginTelScr();
        if (this == originTeleporterScr)
        {
            isOrigin = true;
        }

        originTeleporterScr.fullPwrReq += powerReq;

        if (isOrigin)
        {
            pwrMngr.AddToSysScrList(systemType, sysScr);
        }

        RoomScript _room = transform.parent.parent.GetChild(0).GetChild(0).GetComponent<RoomScript>();
        room = _room.GetOriginObj().GetComponent<RoomScript>();
    }



    public void SyncedPower(bool _isPowered)
    {
        isPowered = _isPowered;

        if (isPowered)
        {
            ship.IncreaseEvasionChance(componentCapacity);
        }
        else
        {
            ship.IncreaseEvasionChance(-componentCapacity);
        }
    }



    public void UpdateHealthState(bool _isFullyDamaged, bool _isFullyRepaired)
    {
        Debug.Log("engine: " + gridPos.X + ", " + gridPos.Y);

        if (_isFullyDamaged)
        {
            //PowerManager.Instance.DamageSystem (systemType, -powerReq);

            if (isPowered)
            {
                ReceivePowerUpdate(false);
            }

            pwrMngr.ApplyHealthState(systemType, powerReq, isPowered, this);
            //isPowered = false;
        }

        if (_isFullyRepaired)
        {
            //PowerManager.Instance.DamageSystem (systemType, powerReq);
            pwrMngr.ApplyHealthState(systemType, -powerReq, isPowered, this);
        }
    }



    //to all
    public void ReceivePowerUpdate(bool _isPowered)
    {
        if (isOrigin)
        {
            if (isPowered)
            {
                SystemScript _sysScr = sysScr.GetOriginObj().GetComponent<SystemScript>();
                _sysScr.UpdatePowerState(false);
            }
            else
            {
                if (!hScr.IsFullyDamaged)
                {
                    if (pwrMngr.EnoughPower(fullPwrReq))
                    {
                        //try power up
                        SystemScript _sysScr = sysScr.GetOriginObj().GetComponent<SystemScript>();
                        _sysScr.UpdatePowerState(true);
                    }
                    else
                    {
                        Debug.LogError("not enough power");
                    }
                }
            }
        }
        else
        {
            originTeleporterScr.ReceivePowerUpdate(_isPowered);
        }
    }

    public void UpdatePowerState(bool _isPowered)
    {
        //at this point we know theres enough power and can power down or up
        if (isPowered)
        {
            //try power down
            pwrMngr.PowerDistribution(systemType, -powerReq, this);
            pwrMngr.UpdateReactor(powerReq);

            //ship.IncreaseEvasionChance(-componentCapacity);

            isPowered = false;
        }
        else
        {
            pwrMngr.PowerDistribution(systemType, powerReq, this);

            //ship.IncreaseEvasionChance(componentCapacity);

            isPowered = true;
        }

        sysScr.IsPowered = isPowered;
    }


    private void SystemAction (bool _isPowered) {
        if (_isPowered) {
            ship.IncreaseEvasionChance(componentCapacity);
        } else {
            ship.IncreaseEvasionChance(-componentCapacity);
        }

        isPowered = _isPowered;
    }


    private TeleporterScr GetOriginTelScr()
    {
        TeleporterScr _telScr = sysScr.GetOriginObj().GetComponent<TeleporterScr>();
        return _telScr;
    }

    public bool IsPowered() {
        return isPowered;
    }

    public void Teleport (Point _point, bool _from) {
        if (isPowered) {
            List<CouchCrewScript> _crewList = new List<CouchCrewScript>();

            if (_from) { 
                List<HealthScript> _hList = room.GetAllHScr();
                //List<CouchCrewScript> _crewList = new List<CouchCrewScript>();
                foreach (var _hScr in _hList)
                {
                    //_crewList.Add(_hScr.GetComponent<CrewScript>());
                    CouchCrewScript _crew = _hScr.GetComponent<CouchCrewScript>();
                    _crewList.Add(_crew);
                    //_crew.Teleport(_point); might've caused issues?
                }
            } else {
                RoomScript _room = LevelManager.Instance.Tiles[_point].transform.GetChild(0).GetChild(0).GetComponent<RoomScript>();
                RoomScript _roomOrig = _room.GetOriginObj().GetComponent<RoomScript>();
                List<HealthScript> _hList = _room.GetAllHScr();
                //List<CouchCrewScript> _crewList = new List<CouchCrewScript>();
                _point = room.GridPos;

                foreach (var _hScr in _hList)
                {
                    //_crewList.Add(_hScr.GetComponent<CrewScript>());
                    CouchCrewScript _crew = _hScr.GetComponent<CouchCrewScript>();
                    _crewList.Add(_crew);
                    //_crew.Teleport(_point); might've caused issues?
                }
            }

            foreach (var _crew in _crewList)
            {
                _crew.Teleport(_point);
            }

        } else {
            Debug.LogError("porter aint powered!");
        }
    }
}