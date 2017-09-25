using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Gamelogic.Grids;
using Gamelogic.Extensions;

namespace KaijuRL.Map
{
    [System.Serializable]
    public class FacingSprites : FacingArray<Sprite>
    {
        // Intentionally blank
    }

    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class MapMobile : MonoBehaviour
    {
        public bool occupiesSpace = true;

        public FacingSprites facingSprites;

        private MapController _mapController = null;
        public MapController mapController
        {
            get
            {
                if (_mapController == null) _mapController = GetComponentInParent<MapController>();
                return _mapController;
            }
        }

        private SpriteRenderer _spriteRenderer = null;
        public SpriteRenderer spriteRenderer
        {
            get
            {
                if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }

        private Facing _facing;
        public Facing facing
        {
            get
            {
                return _facing;
            }

            set
            {
                _facing = value;
                UpdatePresentation();
            }
        }
        
        public Visibility visibility
        {
            get
            {
                return mapController.mapGrid[mapController.WhereIs(this)].visibility;
            }
        }

        public PointyHexPoint location
        {
            get
            {
                return mapController.WhereIs(this);
            }
        }

        public void UpdatePresentation()
        {
            spriteRenderer.sprite = facingSprites[facing];
        }

        #region Abstract functions

        public abstract bool CanSpawn(MapCell cell);

        public abstract bool CanEnter(MapCell cell);

        public abstract bool CanSeeThru(MapCell cell);

        public abstract int CostToEnter(MapCell cell);

        public abstract bool IsHostile(MapMobile other);

        #endregion

        public int startingHp = 100;

        public ObservedValue<int> hp = null;

        public void Start()
        {
            hp = new ObservedValue<int>(startingHp);
        }

        public void OnDestroy()
        {
            mapController.UnplaceMobile(this);
        }
    }
}
