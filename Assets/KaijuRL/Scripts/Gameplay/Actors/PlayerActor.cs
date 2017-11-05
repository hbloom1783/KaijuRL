using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        private void UpdateVisibility()
        {
            Profiler.BeginSample("UpdateVisibility");

            Profiler.BeginSample("Clear Old");
            foreach (PointyHexPoint point in mapController.mapGrid.WhereCell(x => x.visibility == Visibility.visible))
            {
                mapController.CellAt(point).visibility = Visibility.fogOfWar;
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

        private PlayerActionButton GetButton(string buttonName)
        {
            List<PlayerActionButton> candidates = FindObjectsOfType<PlayerActionButton>()
                .Where(x => x.name == buttonName)
                .ToList();

            if (candidates.Count > 0)
            {
                return candidates.First();
            }
            else
            {
                Debug.Log("Could not find button named " + buttonName);
                return null;
            }
        }

        private ActorAction GetAction(string actionName)
        {
            List<ActorAction> candidates = GetComponentsInChildren<ActorAction>()
                .Where(x => x.name == actionName)
                .ToList();

            if (candidates.Count > 0)
            {
                return candidates.First();
            }
            else
            {
                Debug.Log("Could not find action named " + actionName + " on actor named " + name);
                return null;
            }
        }

        private void ConnectButton(string buttonName, string actionName)
        {
            PlayerActionButton button = GetButton(buttonName);
            ActorAction action = GetAction(actionName);

            if (button != null)
            {
                button.actionToPerform = action;
            }
        }

        private void DisconnectButton(string buttonName)
        {
            PlayerActionButton button = GetButton(buttonName);

            if (button != null)
            {
                button.actionToPerform = null;
            }
        }

        // Use this for initialization
        new void Start()
        {
            base.Start();
            UpdateVisibility();

            // QWEASD
            ConnectButton("Turn Left",     "Turn Left Action");
            ConnectButton("Turn Right",    "Turn Right Action");
            ConnectButton("Move Forward",  "Move Forward Action");
            ConnectButton("Move Backward", "Move Backward Action");
            ConnectButton("Move Left",     "Move Left Action");
            ConnectButton("Move Right",    "Move Right Action");

            // Hotbar
            ConnectButton("Hotbar 1",      "Attack Action");
        }

        private ActorAction chosenAction = null;

        List<CellTint> activeTints = null;

        public void ChooseAction(ActorAction action)
        {
            if (activeTints != null)
            {
                activeTints.ForEach(x => DestroyImmediate(x));
                activeTints = null;
            }

            if (action == null)
            {
                chosenAction = null;
            }
            else if (GetComponentsInChildren<ActorAction>().Contains(action))
            {
                chosenAction = action;

                activeTints = chosenAction.MouseInputArea()
                    .Select(x => mapController[x])
                    .Select(x => x.gameObject.AddComponent<CellTint>())
                    .ToList();
            }
            else
            {
                Debug.Log("Did not contain requested action!");
            }
        }

        private void HandleMouseInput()
        {
            PointyHexPoint mouse = mapController.MousePosition;
            if (chosenAction.MouseInputArea().Contains(mouse))
            {
                chosenAction.AcceptMouseInput(mouse);
            }
        }

        private void DoAction(ActorAction action)
        {
            if (myTurn)
            {
                if (action.CanPerform())
                {
                    action.Perform();
                    UpdateVisibility();
                    UpdateButtons();
                }
            }
        }

        public bool myTurn
        {
            get
            {
                return turnController.WhoseTurn() == this;
            }
        }

        private void UpdateButtons()
        {
            foreach (PlayerActionButton button in FindObjectsOfType<PlayerActionButton>())
            {
                button.UpdatePresentation();
            }
        }
        
        private bool firstRun = true;

        public override void TakeTurn()
        {
            Profiler.BeginSample("PlayerActor");

            // Quit
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main Menu");
            }

            if (firstRun)
            {
                UpdateVisibility();
                UpdateButtons();
            }

            if (chosenAction != null)
            {
                if (chosenAction.NeedsMouseInput())
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        HandleMouseInput();
                    }
                }
                
                if (!chosenAction.NeedsMouseInput())
                {
                    DoAction(chosenAction);
                    ChooseAction(null);
                }
            }

            firstRun = !myTurn;

            Profiler.EndSample();
        }
    }
}