using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaijuRL.Map;
using UnityEngine.Profiling;

namespace KaijuRL.Actors
{
    [AddComponentMenu("KaijuRL/Actors/Enemy Actor")]
    public class EnemyActor : Actor
    {
        public override void TakeTurn()
        {
            Profiler.BeginSample("EnemyActor");
            mapMobile.facing = mapMobile.facing.CW();
            ct = 40;
            Profiler.EndSample();
        }
    }
}
