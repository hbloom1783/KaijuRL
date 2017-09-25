using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KaijuRL.Map;

namespace KaijuRL.Actors
{
    [AddComponentMenu("KaijuRL/Actors/Turn Controller")]
    public class TurnController : MonoBehaviour
    {
        List<Actor> actors = new List<Actor>();

        public Actor WhoseTurn()
        {
            return actors.Min();
        }

        public bool IsPlayerTurn()
        {
            return actors.Min() is PlayerActor;
        }

        private void AdvanceTime()
        {
        }

        void Update()
        {
            bool done = false;

            while (!done)
            {
                while (WhoseTurn().ct > 0)
                    actors.ForEach(x => x.ct -= x.ctSpeed);

                WhoseTurn().TakeTurn();

                if (WhoseTurn().mapMobile.visibility == Visibility.visible)
                    done = true;
            }
        }

        public void RegisterActor(Actor actor)
        {
            actors.Add(actor);
        }

        public void UnregisterActor(Actor actor)
        {
            actors.Remove(actor);
        }
    }
}
