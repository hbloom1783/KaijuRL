using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using KaijuRL.Actors;
using KaijuRL.Actors.Actions;
using KaijuRL.Map;

namespace KaijuRL.UI
{
    public class UIController : MonoBehaviour
    {
        #region Camera tracking

        private Stack<GameObject> cameraTracks = new Stack<GameObject>();

        public void CameraTrackPush(GameObject newTrack)
        {
            cameraTracks.Push(newTrack);
        }

        public void CameraTrackPop()
        {
            cameraTracks.Pop();
        }

        public void UpdateCameraTrack()
        {
            if (cameraTracks.Count > 0)
            {
                GameObject target = cameraTracks.Peek();

                Camera.main.transform.position = new Vector3(
                    target.transform.position.x,
                    target.transform.position.y,
                    Camera.main.transform.position.z);
            }
        }

        #endregion

        #region Button management

        public PlayerActionButton GetButton(string buttonName)
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

        public void ConnectButton(string buttonName, ActorAction action)
        {
            PlayerActionButton button = GetButton(buttonName);

            if (button != null)
            {
                button.actionToPerform = action;
            }
        }

        public void DisconnectButton(string buttonName)
        {
            PlayerActionButton button = GetButton(buttonName);

            if (button != null)
            {
                button.actionToPerform = null;
            }
        }


        #endregion

        #region Input routing

        private PlayerActor _playerActor = null;
        public PlayerActor playerActor
        {
            get
            {
                if (_playerActor == null) _playerActor = FindObjectOfType<PlayerActor>();
                return _playerActor;
            }
        }

        private bool _inputActive = false;
        public bool inputActive
        {
            get
            {
                return _inputActive;
            }
            set
            {
                if (_inputActive != value)
                {
                    _inputActive = value;
                    foreach (PlayerActionButton button in FindObjectsOfType<PlayerActionButton>())
                    {
                        button.UpdatePresentation();
                    }
                }
            }
        }

        public void ButtonClick(PlayerActionButton button)
        {
            if (inputActive)
            {
                playerActor.ReceiveAction(button.actionToPerform);
            }
        }

        public void CellClick(MapCell cell)
        {

        }

        #endregion

        void Update()
        {
            UpdateCameraTrack();

            // Quit
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
    }
}
