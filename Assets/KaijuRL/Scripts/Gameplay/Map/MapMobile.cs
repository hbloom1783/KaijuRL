using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
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
        public Sprite fogSprite;

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

        private Canvas _uiCanvas = null;
        public Canvas uiCanvas
        {
            get
            {
                if (_uiCanvas == null) _uiCanvas = GetComponentInChildren<Canvas>();
                return _uiCanvas;
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

        private Visibility _visibility;
        public Visibility visibility
        {
            get
            {
                return _visibility;
            }

            set
            {
                _visibility = value;
                UpdatePresentation();
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
            switch (visibility)
            {
                case Visibility.visible:
                    uiCanvas.enabled = true;
                    spriteRenderer.enabled = true;
                    spriteRenderer.sprite = facingSprites[facing];
                    break;
                case Visibility.fogOfWar:
                    uiCanvas.enabled = true;
                    spriteRenderer.enabled = true;
                    spriteRenderer.sprite = fogSprite;
                    break;
                case Visibility.darkness:
                    uiCanvas.enabled = false;
                    spriteRenderer.enabled = false;
                    break;
            }
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
            if (mapController != null)
                mapController.UnplaceMobile(this);
        }
    }
}
