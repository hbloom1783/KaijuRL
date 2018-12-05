using UnityEngine;

namespace KaijuRL.Actors
{
    public class DummyAnimation : TurnAnimation
    {
        public override bool isDone
        {
            get
            {
                if (Time.time >= endTime)
                    Debug.Log("Ending sleep.");

                return Time.time >= endTime;
            }
        }

        float endTime = 0.0f;

        void Start()
        {
            Debug.Log("Beginning sleep.");

            endTime = Time.time + .1f;
        }
    }
}
