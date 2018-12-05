using UnityEngine;

namespace KaijuRL.Actors
{
    public abstract class TurnAnimation : MonoBehaviour
    {
        public abstract bool isDone
        {
            get;
        }
    }
}
