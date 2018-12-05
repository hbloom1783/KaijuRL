using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Gamelogic.Grids;
using KaijuRL.Map;
using KaijuRL.Extensions;
using KaijuRL.Actors.Actions;
using KaijuRL.UI;

namespace KaijuRL.Actors
{
    [AddComponentMenu("KaijuRL/Actors/Player Actor")]
    public class PlayerActor : Actor
    {
        public int visionRange = 5;
        public int hearingRange = 3;

        #region Visibility

        private void UpdateVisibility()
        {
            Profiler.BeginSample("UpdateVisibility");

            Profiler.BeginSample("Clear Old");
            foreach (PointyHexPoint point in Controllers.map.mapGrid.WhereCell(x => x.visibility == Visibility.visible))
            {
                Controllers.map.CellAt(point).visibility = Visibility.fogOfWar;
            }
            Profiler.EndSample();

            Profiler.BeginSample("Find Mobile");
            PointyHexPoint mobLoc = mapMobile.location;
            Profiler.EndSample();

            Profiler.BeginSample("Compute Edge Arc");
            List<PointyHexPoint> visionArc = Controllers.map.Map.GetArc(
                mobLoc,
                mapMobile.facing.CCW(2),
                mapMobile.facing.CW(2),
                visionRange,
                visionRange);
            Profiler.EndSample();

            Profiler.BeginSample("Raytrace");
            foreach (PointyHexPoint arcPoint in visionArc)
            {
                Func<PointyHexPoint, bool> CanSee = point =>
                {
                    if (Controllers.map.mapGrid.Contains(point))
                        return mapMobile.CanSeeThru(Controllers.map.mapGrid[point]);
                    else
                        return false;
                };

                Profiler.BeginSample("Compute Line");
                List<PointyHexPoint> lineOfSight = Controllers.map.Map.GetLine(mobLoc, arcPoint);
                Profiler.EndSample();

                Profiler.BeginSample("Mark until blocked");
                foreach (PointyHexPoint linePoint in lineOfSight.TakeWhile(CanSee).ToList())
                {
                    Controllers.map.mapGrid[linePoint].visibility = Visibility.visible;
                }
                Profiler.EndSample();

                Profiler.BeginSample("Compute leftovers");
                List<PointyHexPoint> leftovers = lineOfSight.SkipWhile(CanSee).ToList();
                Profiler.EndSample();

                Profiler.BeginSample("Mark leftovers");
                if (leftovers.Count > 0)
                {
                    if (Controllers.map.mapGrid.Contains(leftovers.First()))
                        Controllers.map.mapGrid[leftovers.First()].visibility = Visibility.visible;
                }
                Profiler.EndSample();
            }
            Profiler.EndSample();

            Profiler.EndSample();
        }

        private void UpdateAudibility()
        {
            Profiler.BeginSample("UpdateAudibility");

            Profiler.BeginSample("Find Mobile");
            PointyHexPoint mobLoc = mapMobile.location;
            Profiler.EndSample();
            
            Profiler.BeginSample("Measure Distances");
            foreach (var x in Controllers.map.mapGrid)
            {
                if (x.DistanceFrom(mobLoc) <= hearingRange)
                {
                    Controllers.map.mapGrid[x].audibility = Audibility.audible;
                }
                else
                {
                    Controllers.map.mapGrid[x].audibility = Audibility.inaudible;
                }
            }
            Profiler.EndSample();

            Profiler.EndSample();
        }

        #endregion
        
        // Use this for initialization
        new void Start()
        {
            base.Start();
            UpdateVisibility();
            UpdateAudibility();

            // QWEASD
            Controllers.ui.ConnectButton("Turn Left",     GetAction("Turn Left Action"));
            Controllers.ui.ConnectButton("Turn Right",    GetAction("Turn Right Action"));
            Controllers.ui.ConnectButton("Move Forward",  GetAction("Move Forward Action"));
            Controllers.ui.ConnectButton("Move Backward", GetAction("Move Backward Action"));
            Controllers.ui.ConnectButton("Move Left",     GetAction("Move Left Action"));
            Controllers.ui.ConnectButton("Move Right",    GetAction("Move Right Action"));

            // Hotbar
            Controllers.ui.ConnectButton("Hotbar 1",      GetAction("Attack Action"));
        }
        
        private bool firstRun;

        public override void BeginTurn()
        {
            base.BeginTurn();
            firstRun = true;
            chosenAction = null;
            Controllers.ui.CameraTrackPush(gameObject);
            Controllers.ui.inputActive = true;
        }

        private ActorAction chosenAction = null;
        public void ReceiveAction(ActorAction actionName)
        {
            chosenAction = actionName;
        }

        public override bool TakeTurn()
        {
            Profiler.BeginSample("PlayerActor");

            bool result = false;

            if (firstRun)
            {
                UpdateVisibility();
                UpdateAudibility();
                firstRun = false;
            }
            else if (chosenAction != null)
            {
                chosenAction.Perform();
                UpdateVisibility();
                UpdateAudibility();
                //Controllers.ui.UpdateCameraTrack();
                Controllers.ui.CameraTrackPop();
                Controllers.ui.inputActive = false;
                result = true;
            }

            Profiler.EndSample();

            return result;
        }

        public override ActorType actorType
        {
            get { return ActorType.player; }
        }
    }
}