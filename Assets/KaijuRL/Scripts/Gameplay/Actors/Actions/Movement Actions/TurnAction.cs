using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaijuRL.Map;
using Gamelogic.Grids;

namespace KaijuRL.Actors.Actions
{
    public enum TurnDirection
    {
        left,
        right,
    }

    [AddComponentMenu("KaijuRL/Actors/Actions/Turn Action")]
    public class TurnAction : ActorAction
    {
        public TurnDirection turnDirection;

        public override void Perform()
        {
            switch (turnDirection)
            {
                case TurnDirection.left:  actor.mapMobile.facing = actor.mapMobile.facing.CCW(); break;
                case TurnDirection.right: actor.mapMobile.facing = actor.mapMobile.facing.CW(); break;
            }

            actor.ct += cost;
        }
        
        public override bool CanPerform()
        {
            return true;
        }

        public override bool NeedsMouseInput()
        {
            return false;
        }

        public override IEnumerable<PointyHexPoint> MouseInputArea()
        {
            return new List<PointyHexPoint>();
        }

        public override void AcceptMouseInput(PointyHexPoint input)
        {
            // No possible mouse input
        }
    }
}
