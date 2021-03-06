﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Gamelogic.Grids;
using KaijuRL.Map;

namespace KaijuRL.Actors.Actions
{
    public abstract class ActorAction : MonoBehaviour
    {
        private Actor _actor = null;
        public Actor actor
        {
            get
            {
                if (_actor == null) _actor = GetComponentInParent<Actor>();

                return _actor;
            }
        }

        protected PointyHexPoint mobLoc
        {
            get
            {
                return actor.mapMobile.location;
            }
        }

        protected Facing mobFacing
        {
            get
            {
                return actor.mapMobile.facing;
            }
        }

        public Sprite icon;
        public int cost = 100;

        public abstract bool CanPerform();
        public abstract void Perform();

        public abstract bool NeedsMouseInput { get; }
        public virtual IEnumerable<PointyHexPoint> MouseInputArea()
        {
            return new List<PointyHexPoint>();
        }
        public virtual void AcceptMouseInput(PointyHexPoint input) { }
    }
}
