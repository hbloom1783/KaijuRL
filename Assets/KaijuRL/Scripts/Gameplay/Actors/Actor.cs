using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaijuRL.Map;

namespace KaijuRL.Actors
{
    [RequireComponent(typeof(MapMobile))]
    public abstract class Actor : MonoBehaviour
    {
        private MapMobile _mapMobile = null;
        public MapMobile mapMobile
        {
            get
            {
                if (_mapMobile == null) _mapMobile = GetComponent<MapMobile>();
                return _mapMobile;
            }
        }

        public abstract bool TakeTurn();
    }
}
