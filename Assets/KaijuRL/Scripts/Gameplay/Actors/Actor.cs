using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaijuRL.Map;

namespace KaijuRL.Actors
{
    [RequireComponent(typeof(MapMobile))]
    public abstract class Actor : MonoBehaviour, IComparable<Actor>
    {
        public int ctSpeed = 10;

        private MapMobile _mapMobile = null;
        public MapMobile mapMobile
        {
            get
            {
                if (_mapMobile == null) _mapMobile = GetComponent<MapMobile>();
                return _mapMobile;
            }
        }

        private MapController _mapController = null;
        public MapController mapController
        {
            get
            {
                if (_mapController == null) _mapController = GetComponentInParent<MapController>();
                return _mapController;
            }
        }

        private TurnController _turnController = null;
        public TurnController turnController
        {
            get
            {
                if (_turnController == null) _turnController = GetComponentInParent<TurnController>();
                return _turnController;
            }
        }

        public abstract void TakeTurn();

        public int ct = 0;

        public void Start()
        {
            turnController.RegisterActor(this);
        }

        private void OnDestroy()
        {
            turnController.UnregisterActor(this);
        }

        public int CompareTo(Actor other)
        {
            return ct - other.ct;
        }
    }
}
