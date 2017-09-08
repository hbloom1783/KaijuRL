using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KaijuRL.Actors
{
    [AddComponentMenu("KaijuRL/Actors/Turn Controller")]
    public class TurnController : MonoBehaviour
    {
        List<Actor> actors = new List<Actor>();

        void Update()
        {
            while (actors.Min().ct > 0)
                actors.ForEach(x => x.ct -= x.ctSpeed);

            actors.Min().TakeTurn();
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
