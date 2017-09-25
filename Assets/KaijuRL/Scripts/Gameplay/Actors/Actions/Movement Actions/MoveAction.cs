using System.Collections.Generic;
using UnityEngine;
using KaijuRL.Map;
using Gamelogic.Grids;

namespace KaijuRL.Actors.Actions
{
    public enum MoveDirection
    {
        foward,
        backward,
        frontLeft,
        frontRight,
        backLeft,
        backRight,
    }

    [AddComponentMenu("KaijuRL/Actors/Actions/Move Action")]
    public class MoveAction : ActorAction
    {
        public MoveDirection moveDirection = MoveDirection.foward;

        private PointyHexPoint destination
        {
            get
            {
                switch (moveDirection)
                {
                    case MoveDirection.foward: return mobLoc + mobFacing.Offset();
                    case MoveDirection.backward: return mobLoc - mobFacing.Offset();
                    case MoveDirection.frontLeft: return mobLoc + mobFacing.CCW().Offset();
                    case MoveDirection.frontRight: return mobLoc + mobFacing.CW().Offset();
                    case MoveDirection.backLeft: return mobLoc - mobFacing.CW().Offset();
                    case MoveDirection.backRight: return mobLoc - mobFacing.CCW().Offset();
                    default: return mobLoc;
                }
            }
        }

        public override void Perform()
        {
            PointyHexPoint dst = destination;
            actor.mapController.UnplaceMobile(actor.mapMobile);
            actor.mapController.PlaceMobile(actor.mapMobile, dst);
            actor.ct += actor.mapMobile.CostToEnter(actor.mapController.mapGrid[destination]);
        }

        public override bool CanPerform()
        {
            return actor.mapController.mapGrid.Contains(destination) &&
                actor.mapMobile.CanEnter(actor.mapController.mapGrid[destination]) &&
                (actor.mapMobile.CostToEnter(actor.mapController.mapGrid[destination]) > 0);
        }

        public override IEnumerable<PointyHexPoint> MouseInputArea()
        {
            List<PointyHexPoint> result = new List<PointyHexPoint>();

            if (CanPerform()) result.Add(destination);

            return result;
        }

        public override void AcceptMouseInput(PointyHexPoint input)
        {
            Perform();
        }
    }
}
