using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaijuRL.Map;
using UnityEngine.Profiling;
using KaijuRL.Actors.Actions;
using KaijuRL.Extensions;

namespace KaijuRL.Actors
{
    [AddComponentMenu("KaijuRL/Actors/Enemy Actor")]
    public class EnemyActor : Actor
    {
        private ActorAction priorityAction = null;
        private List<ActorAction> otherActions = new List<ActorAction>();

        new void Start()
        {
            base.Start();

            priorityAction = GetAction("Move Forward Action");
            otherActions.Add(GetAction("Turn Left Action"));
            otherActions.Add(GetAction("Turn Right Action"));
        }

        public override bool TakeTurn()
        {
            Profiler.BeginSample("EnemyActor");

            ActorAction chosenAction = null;

            if (priorityAction.CanPerform())
            {
                chosenAction = priorityAction;
            }
            else
            {
                chosenAction = otherActions.Where(x => x.CanPerform()).RandomPick();
            }

            chosenAction.Perform();

            Profiler.EndSample();

            return true;
        }

        public override ActorType actorType
        {
            get { return ActorType.ai; }
        }
    }
}
