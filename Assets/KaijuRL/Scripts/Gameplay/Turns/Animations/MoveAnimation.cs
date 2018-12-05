using UnityEngine;
using System.Collections;
using KaijuRL.UI;

namespace KaijuRL.Actors
{
    public class MoveAnimation : TurnAnimation
    {
        private bool _isDone = false;
        public override bool isDone
        {
            get
            {
                return _isDone;
            }
        }

        private UIController _uiController = null;
        public UIController uiController
        {
            get
            {
                if (_uiController == null) _uiController = GetComponentInParent<UIController>();
                return _uiController;
            }
        }

        IEnumerator LerpPosition()
        {
            if (isPlayer) uiController.CameraTrackPush(gameObject);

            float timeElasped = 0.0f;

            while (timeElasped < maxTime)
            {
                timeElasped += Time.deltaTime;

                transform.position = Vector3.Lerp(oldPos, newPos, timeElasped / maxTime);

                yield return null;
            }

            _isDone = true;

            if (isPlayer) uiController.CameraTrackPush(gameObject);
        }

        public Vector3 oldPos;
        public Vector3 newPos;
        public float maxTime;

        private bool isPlayer;

        void Start()
        {
            isPlayer = GetComponent<Actor>().actorType == ActorType.player;
            StartCoroutine(LerpPosition());
        }
    }
}
