using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;
using KaijuRL.Actors;
using KaijuRL.Actors.Actions;

namespace KaijuRL.UI
{
    [AddComponentMenu("KaijuRL/UI/Player Action Button")]
    [RequireComponent(typeof(Button))]
    public class PlayerActionButton : MonoBehaviour
    {
        private PlayerActor _playerActor = null;
        public PlayerActor playerActor
        {
            get
            {
                if (_playerActor == null) _playerActor = FindObjectOfType<PlayerActor>();
                return _playerActor;
            }
        }

        private Button _button = null;
        public Button button
        {
            get
            {
                if (_button == null) _button = GetComponent<Button>();
                return _button;
            }
        }

        private Text _text = null;
        public Text text
        {
            get
            {
                if (_text == null) _text = GetComponentInChildren<Text>();
                return _text;
            }
        }

        private Image _image = null;
        public Image image
        {
            get
            {
                if (_image == null) _image = GetComponent<Image>();
                return _image;
            }
        }

        private ActorAction _actionToPerform = null;
        public ActorAction actionToPerform
        {
            get
            {
                return _actionToPerform;
            }

            set
            {
                _actionToPerform = value;
                UpdatePresentation();
            }
        }

        public void UpdatePresentation()
        {
            if (actionToPerform != null)
            {
                image.sprite = actionToPerform.icon;
                //text.text = hotkey.ToString();

                if (playerActor.myTurn == false)
                {
                    button.interactable = false;
                }
                else
                {
                    button.interactable = actionToPerform.CanPerform();
                }
            }
            else
            {
                button.interactable = false;
            }
        }

        public void DoAction()
        {
            if (actionToPerform != null)
            {
                playerActor.ChooseAction(actionToPerform);
            }
        }

        // Use this for initialization
        void Start()
        {
            UpdatePresentation();
        }

        public KeyCode hotkey;

        void Update()
        {
            Profiler.BeginSample("PlayerActionButton " + name);
            
            if (Input.GetKeyDown(hotkey) && (button.interactable))
            {
                DoAction();
            }

            Profiler.EndSample();
        }
    }
}
