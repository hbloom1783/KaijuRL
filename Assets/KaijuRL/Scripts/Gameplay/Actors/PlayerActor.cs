using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Gamelogic.Grids;
using KaijuRL.Map;
using KaijuRL.Extensions;

namespace KaijuRL.Actors
{
    [AddComponentMenu("KaijuRL/Actors/Player Actor")]
    public class PlayerActor : Actor
    {
        public int visionRange = 5;

        public Facing facing
        {
            get
            {
                return facingIndicator.facing;
            }

            set
            {
                facingIndicator.facing = value;
                UpdatePresentation();
            }
        }

        private MapController _mapController = null;
        public MapController mapController
        {
            get
            {
                if (_mapController == null) _mapController = GetComponentInParent<MapController>();
                return _mapController;
            }
        }

        private FacingIndicator _facingIndicator = null;
        public FacingIndicator facingIndicator
        {
            get
            {
                if (_facingIndicator == null) _facingIndicator = GetComponentInChildren<FacingIndicator>();
                return _facingIndicator;
            }
        }

        private void UpdatePresentation()
        {
            Camera.main.transform.position = new Vector3(
                mapMobile.transform.position.x,
                mapMobile.transform.position.y,
                Camera.main.transform.position.z);

            if ((facing == Facing.sw) || (facing == Facing.w) || (facing == Facing.nw))
                mapMobile.spriteRenderer.flipX = true;
            else
                mapMobile.spriteRenderer.flipX = false;
        }

        private void UpdateVisibility()
        {
            foreach (PointyHexPoint point in mapController.mapGrid.WhereCell(x => x.visibility == Visibility.visible))
            {
                mapController.mapGrid[point].visibility = Visibility.fogOfWar;
            }

            PointyHexPoint mobLoc = mapController.WhereIs(mapMobile);

            /*List<MapCell> visionArc = mapController.DrawArc(
                mobLoc,
                facing.CCW(2),
                facing.CW(2),
                visionRange,
                visionRange);*/

            List<PointyHexPoint> visionArc = mapController.Map.GetArc(
                mobLoc,
                facing.CCW(2),
                facing.CW(2),
                visionRange,
                visionRange);

            foreach(PointyHexPoint arcPoint in visionArc)
            {
                Func<PointyHexPoint, bool> CanSee = point =>
                {
                    if (mapController.mapGrid.Contains(point))
                        return mapMobile.CanSeeThru(mapController.mapGrid[point]);
                    else
                        return false;
                };

                List<PointyHexPoint> lineOfSight = mapController.Map.GetLine(mobLoc, arcPoint);

                foreach (PointyHexPoint linePoint in lineOfSight.TakeWhile(CanSee).ToList())
                {
                    mapController.mapGrid[linePoint].visibility = Visibility.visible;
                }

                List<PointyHexPoint> leftovers = lineOfSight.SkipWhile(CanSee).ToList();

                if (leftovers.Count > 0)
                {
                    if (mapController.mapGrid.Contains(leftovers.First()))
                        mapController.mapGrid[leftovers.First()].visibility = Visibility.visible;
                }
                
            }

            /*foreach (MapCell arcCell in visionArc)
            {
                PointyHexPoint cellLoc = mapController.WhereIs(arcCell);

                mapController.Map.GetLine(mobLoc, cellLoc);

                List<MapCell> lineOfSight = mapController.DrawLine(mobLoc, cellLoc);
                lineOfSight.Remove(cell);

                if (lineOfSight.TrueForAll(mapMobile.CanSeeThru))
                {
                    cell.visibility = Visibility.visible;
                }

                foreach(MapCell cell in lineOfSight.TakeWhile(mapMobile.CanSeeThru))
                {
                    cell.visibility = Visibility.visible;
                }
            }*/
        }

        // Use this for initialization
        void Start()
        {
            UpdatePresentation();
            UpdateVisibility();
        }

        // Update is called once per frame
        void Update()
        {
            TakeTurn();
        }

        public override bool TakeTurn()
        {
            PointyHexPoint oldLoc = mapController.WhereIs(mapMobile);

            bool result = false;

            Action<MapMobile, PointyHexPoint> TryMove = (mobile, point) =>
            {
                if (mapController.mapGrid.Contains(point) && mobile.CanEnter(mapController.mapGrid[point]))
                {
                    mapController.UnplaceMobile(mobile);
                    mapController.PlaceMobile(mobile, point);
                    UpdatePresentation();

                    result = true;
                }
            };
            
            // Turn Left
            if (Input.GetKeyDown(KeyCode.A))
            {
                facing = facing.CCW();
                result = true;
            }

            // Turn Right
            else if (Input.GetKeyDown(KeyCode.D))
            {
                facing = facing.CW();
                result = true;
            }

            // Forward
            else if (Input.GetKeyDown(KeyCode.W))
            {
                TryMove(mapMobile, oldLoc + facing.Offset());
            }

            // Backward
            else if (Input.GetKeyDown(KeyCode.S))
            {
                TryMove(mapMobile, oldLoc - facing.Offset());
            }

            // Strafe Left
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                TryMove(mapMobile, oldLoc + facing.CCW().Offset());
            }

            // Strafe Right
            else if (Input.GetKeyDown(KeyCode.E))
            {
                TryMove(mapMobile, oldLoc + facing.CW().Offset());
            }

            // Quit
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main Menu");
            }

            if (result == true)
            {
                UpdateVisibility();
            }

            return result;
        }
    }
}