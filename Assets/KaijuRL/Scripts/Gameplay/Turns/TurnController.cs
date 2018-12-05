using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KaijuRL.UI;

namespace KaijuRL.Actors
{
    [AddComponentMenu("KaijuRL/Actors/Turn Controller")]
    public class TurnController : MonoBehaviour
    {
        #region Actor Management

        private List<Actor> actors = new List<Actor>();

        public void RegisterActor(Actor actor)
        {
            actors.Add(actor);
        }

        public void UnregisterActor(Actor actor)
        {
            actors.Remove(actor);
        }

        public Actor nextActor
        {
            get { return actors.Min(); }
        }

        #endregion

        #region Animation Management

        private List<TurnAnimation> animationsPlaying = new List<TurnAnimation>();

        public void RegisterAnimation(TurnAnimation newAnimation)
        {
            animationsPlaying.Add(newAnimation);
        }

        #endregion

        #region Turn-Taking

        private enum TurnState
        {
            newActor,
            sameActor,
            animating,
        };

        private TurnState state = TurnState.newActor;

        #endregion

        void Update()
        {
            bool stopLoop = false;
            while (!stopLoop)
            {
                switch(state)
                {
                    case TurnState.newActor:
                        while (nextActor.ct > 0)
                            actors.ForEach(x => x.ct -= x.ctSpeed);

                        nextActor.BeginTurn();
                        goto case TurnState.sameActor;

                    case TurnState.sameActor:
                        if (nextActor.TakeTurn() == false)
                        {
                            state = TurnState.sameActor;
                            stopLoop = true;
                        }
                        else
                        {
                            if (animationsPlaying.Count() > 0)
                            {
                                state = TurnState.animating;
                                stopLoop = true;
                            }
                            else
                            {
                                state = TurnState.newActor;
                            }
                        }
                        break;

                    case TurnState.animating:
                        animationsPlaying.RemoveAll(x => x.isDone);
                        if (animationsPlaying.Count() == 0)
                            state = TurnState.newActor;
                        else
                            stopLoop = true;
                        break;
                }
            }
        }
    }
}
