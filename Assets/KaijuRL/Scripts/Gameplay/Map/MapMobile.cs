using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KaijuRL.Map
{
    [System.Serializable]
    public class FacingSprites : FacingArray<Sprite>
    {

    }

    [AddComponentMenu("KaijuRL/Map/Map Mobile")]
    [RequireComponent(typeof(SpriteRenderer))]
    public class MapMobile : MonoBehaviour
    {
        public bool occupiesSpace = true;

        public FacingSprites facingSprites;

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

        public void UpdatePresentation()
        {
            spriteRenderer.sprite = facingSprites[facing];
        }

        #region MapCell predicates

        public bool CanSpawn(MapCell cell)
        {
            if (occupiesSpace && cell.mobilesPresent.Any(x => x.occupiesSpace))
                return false;
            else
                return cell.type == TerrainType.grass;
        }

        public bool CanEnter(MapCell cell)
        {
            if (occupiesSpace && cell.mobilesPresent.Any(x => x.occupiesSpace))
                return false;
            else
                return cell.type == TerrainType.grass;
        }

        public bool CanSeeThru(MapCell cell)
        {
            return cell.type != TerrainType.mountain;
        }

        #endregion
    }
}
