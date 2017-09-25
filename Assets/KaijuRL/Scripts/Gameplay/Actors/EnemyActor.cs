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
        public override void TakeTurn()
        {
            Profiler.BeginSample("EnemyActor");

            GetComponentsInChildren<ActorAction>().Where(x => x.CanPerform()).RandomPick().Perform();

            Profiler.EndSample();
        }
    }
}
