using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using KaijuRL.Map;
using Gamelogic.Grids;

namespace KaijuRL.Actors.Actions
{
    [AddComponentMenu("KaijuRL/Actors/Actions/Attack Action")]
    public class AttackAction : ActorAction
    {
        private MapMobile target = null;
        public override void Perform()
        {
            if ((target == null) && (MouseInputArea().Count() == 1))
            {
                AcceptMouseInput(MouseInputArea().First());
            }
            else if (target != null)
            {
                target.hp.Value -= 50;
                actor.ct += cost;
            }
        }

        private List<PointyHexPoint> attackRange
        {
            get
            {
                List<PointyHexPoint> result = new List<PointyHexPoint>();

                result.Add(mobLoc + mobFacing.Offset());
                result.Add(mobLoc + mobFacing.CW().Offset());
                result.Add(mobLoc + mobFacing.CCW().Offset());

                return result;
            }
        }

        public override bool CanPerform()
        {
            return MouseInputArea().Count() > 0;
        }

        public override IEnumerable<PointyHexPoint> MouseInputArea()
        {
            return attackRange
                .Where(x => actor.mapController.mapGrid[x]
                    .AnyMobile(actor.mapMobile.IsHostile));
        }

        public override void AcceptMouseInput(PointyHexPoint input)
        {
            target = actor.mapController.mapGrid[input].MobilesWhere(actor.mapMobile.IsHostile).First();
            Perform();
            target = null;
        }
    }
}
