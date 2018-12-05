using KaijuRL.Map;
using KaijuRL.Actors;
using KaijuRL.UI;
using UnityEngine;

namespace KaijuRL
{
    public static class Controllers
    {
        private static MapController _mapController = null;
        public static MapController map
        {
            get
            {
                if (_mapController == null)
                    _mapController = Object.FindObjectOfType<MapController>();
                return _mapController;
            }
        }

        private static TurnController _turnController = null;
        public static TurnController turn
        {
            get
            {
                if (_turnController == null)
                    _turnController = Object.FindObjectOfType<TurnController>();
                return _turnController;
            }
        }

        private static UIController _uiController = null;
        public static UIController ui
        {
            get
            {
                if (_uiController == null)
                    _uiController = Object.FindObjectOfType<UIController>();
                return _uiController;
            }
        }

    }
}
