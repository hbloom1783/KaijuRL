using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
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
        public float moveTime = 1.0f;

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
            Vector3 oldPos = actor.mapMobile.transform.position;
            Vector3 newPos = Controllers.map.CellAt(destination).transform.position;

            PointyHexPoint dst = destination;
            Controllers.map.UnplaceMobile(actor.mapMobile);
            Controllers.map.PlaceMobile(actor.mapMobile, dst);
            actor.ct += actor.mapMobile.CostToEnter(Controllers.map.CellAt(dst));

            if (actor.mapMobile.visibility == Visibility.visible)
            {
                MoveAnimation newAnimation = actor.mapMobile.gameObject.AddComponent<MoveAnimation>();
                newAnimation.oldPos = oldPos;
                newAnimation.newPos = newPos;
                newAnimation.maxTime = moveTime;

                Controllers.turn.RegisterAnimation(newAnimation);
            }
        }

        public override bool CanPerform()
        {
            Profiler.BeginSample("MoveAction.CanPerform");

            Profiler.BeginSample("Contains");
            if (!Controllers.map.InBounds(destination)) return false;
            Profiler.EndSample();

            Profiler.BeginSample("Lookup");
            MapCell destCell = Controllers.map.CellAt(destination);
            Profiler.EndSample();

            Profiler.EndSample();

            return actor.mapMobile.CanEnter(destCell) && (actor.mapMobile.CostToEnter(destCell) > 0);
        }

        public override bool NeedsMouseInput
        {
            get
            {
                return false;
            }
        }
    }
}
