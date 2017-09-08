using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Profiling;
using Gamelogic.Grids;
using KaijuRL.Map;
using KaijuRL.Extensions;

namespace KaijuRL.Actors
{
    [AddComponentMenu("KaijuRL/Actors/Player Actor")]
    public class PlayerActor : Actor
    {
        public int visionRange = 5;

        private void UpdatePresentation()
        {
            Profiler.BeginSample("UpdatePresentation");

            Camera.main.transform.position = new Vector3(
                mapMobile.transform.position.x,
                mapMobile.transform.position.y,
                Camera.main.transform.position.z);

            Profiler.EndSample();
        }

        private void UpdateVisibility()
        {
            Profiler.BeginSample("UpdateVisibility");

            Profiler.BeginSample("Clear Old");
            foreach (PointyHexPoint point in mapController.mapGrid.WhereCell(x => x.visibility == Visibility.visible))
            {
                mapController.mapGrid[point].visibility = Visibility.fogOfWar;
            }
            Profiler.EndSample();

            Profiler.BeginSample("Find Mobile");
            PointyHexPoint mobLoc = mapController.WhereIs(mapMobile);
            Profiler.EndSample();

            Profiler.BeginSample("Compute Edge Arc");
            List<PointyHexPoint> visionArc = mapController.Map.GetArc(
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
                    if (mapController.mapGrid.Contains(point))
                        return mapMobile.CanSeeThru(mapController.mapGrid[point]);
                    else
                        return false;
                };

                Profiler.BeginSample("Compute Line");
                List<PointyHexPoint> lineOfSight = mapController.Map.GetLine(mobLoc, arcPoint);
                Profiler.EndSample();

                Profiler.BeginSample("Mark until blocked");
                foreach (PointyHexPoint linePoint in lineOfSight.TakeWhile(CanSee).ToList())
                {
                    mapController.mapGrid[linePoint].visibility = Visibility.visible;
                }
                Profiler.EndSample();

                Profiler.BeginSample("Compute leftovers");
                List<PointyHexPoint> leftovers = lineOfSight.SkipWhile(CanSee).ToList();
                Profiler.EndSample();

                Profiler.BeginSample("Mark leftovers");
                if (leftovers.Count > 0)
                {
                    if (mapController.mapGrid.Contains(leftovers.First()))
                        mapController.mapGrid[leftovers.First()].visibility = Visibility.visible;
                }
                Profiler.EndSample();
            }
            Profiler.EndSample();

            Profiler.EndSample();
        }

        // Use this for initialization
        new void Start()
        {
            base.Start();
            UpdatePresentation();
            UpdateVisibility();
        }

        public override void TakeTurn()
        {
            Profiler.BeginSample("PlayerActor");
            PointyHexPoint oldLoc = mapController.WhereIs(mapMobile);

            Action<MapMobile, PointyHexPoint> TryMove = (mobile, point) =>
            {
                if (mapController.mapGrid.Contains(point) && mobile.CanEnter(mapController.mapGrid[point]))
                {
                    mapController.UnplaceMobile(mobile);
                    mapController.PlaceMobile(mobile, point);
                    UpdatePresentation();

                    ct = 100;
                }
            };
            
            // Turn Left
            if (Input.GetKeyDown(KeyCode.A))
            {
                mapMobile.facing = mapMobile.facing.CCW();
                ct = 40;
            }

            // Turn Right
            else if (Input.GetKeyDown(KeyCode.D))
            {
                mapMobile.facing = mapMobile.facing.CW();
                ct = 40;
            }

            // Forward
            else if (Input.GetKeyDown(KeyCode.W))
            {
                TryMove(mapMobile, oldLoc + mapMobile.facing.Offset());
            }

            // Backward
            else if (Input.GetKeyDown(KeyCode.S))
            {
                TryMove(mapMobile, oldLoc - mapMobile.facing.Offset());
            }

            // Strafe Left
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                TryMove(mapMobile, oldLoc + mapMobile.facing.CCW().Offset());
            }

            // Strafe Right
            else if (Input.GetKeyDown(KeyCode.E))
            {
                TryMove(mapMobile, oldLoc + mapMobile.facing.CW().Offset());
            }

            // Quit
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main Menu");
            }

            if (ct > 0)
            {
                UpdateVisibility();
            }

            Profiler.EndSample();
        }
    }
}