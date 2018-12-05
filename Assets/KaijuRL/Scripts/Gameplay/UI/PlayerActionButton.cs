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

        private UIController _uiController = null;
        public UIController uiController
        {
            get
            {
                if (_uiController == null) _uiController = GetComponentInParent<UIController>();
                return _uiController;
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
            if (actionToPerform == null)
            {
                image.color = new Color(0, 0, 0, 0);
                button.interactable = false;
                image.sprite = null;
                

                text.CrossFadeAlpha(0, 0, false);
            }
            else
            {
                image.color = new Color(1,1,1,1);
                if (uiController.inputActive == false)
                {
                    button.interactable = false;
                }
                else
                {
                    button.interactable = actionToPerform.CanPerform();
                }
                image.sprite = actionToPerform.icon;
                

                text.CrossFadeAlpha(1, 0, false);
                //text.text = hotkey.ToString();
            }
        }

        public void DoAction()
        {
            if (actionToPerform != null)
            {
                uiController.ButtonClick(this);
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
                uiController.ButtonClick(this);
            }

            Profiler.EndSample();
        }
    }
}
