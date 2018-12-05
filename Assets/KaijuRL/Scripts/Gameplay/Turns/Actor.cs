using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using KaijuRL.Map;
using KaijuRL.Actors.Actions;
using KaijuRL.UI;

namespace KaijuRL.Actors
{
    public enum ActorType
    {
        player,
        ai,
    }

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
        
        public virtual void BeginTurn() { }
        public abstract bool TakeTurn();

        public abstract ActorType actorType
        {
            get;
        }

        public ActorAction GetAction(string actionName)
        {
            List<ActorAction> candidates = GetComponentsInChildren<ActorAction>()
                .Where(x => x.name == actionName)
                .ToList();

            if (candidates.Count > 0)
            {
                return candidates.First();
            }
            else
            {
                Debug.Log("Could not find action named " + actionName + " on actor named " + name);
                return null;
            }
        }

        public int ct = 0;

        public void Start()
        {
            Controllers.turn.RegisterActor(this);
        }

        private void OnDestroy()
        {
            if (Controllers.turn != null)
                Controllers.turn.UnregisterActor(this);
        }

        public int CompareTo(Actor other)
        {
            return ct - other.ct;
        }
    }
}
