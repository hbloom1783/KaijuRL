using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamelogic.Grids;
using Gamelogic.Extensions;

namespace KaijuRL.Map
{
    public enum TerrainType
    {
        grass,
        water,
        mountain,
    }

    public enum Visibility
    {
        darkness,
        fogOfWar,
        visible,
    }

    public enum Audibility
    {
        inaudible,
        audible,
    }

    [AddComponentMenu("KaijuRL/Map/Map Cell")]
    [RequireComponent(typeof(SpriteRenderer))]
    public class MapCell : TileCell
    {
        public Sprite grassSprite;
        public Sprite waterSprite;
        public Sprite mountainSprite;

        #region MobilesPresent

        private List<MapMobile> mobilesPresent = new List<MapMobile>();

        public void AddMobile(MapMobile mobile)
        {
            mobilesPresent.Add(mobile);
            __UpdatePresentation(true);
        }

        public void RemoveMobile(MapMobile mobile)
        {
            mobilesPresent.Remove(mobile);
            __UpdatePresentation(true);
        }

        public bool HasMobile(MapMobile mobile)
        {
            return mobilesPresent.Contains(mobile);
        }

        public int MobileCount()
        {
            return mobilesPresent.Count;
        }

        public bool AnyMobile(Func<MapMobile, bool> predicate)
        {
            return mobilesPresent.Any(predicate);
        }

        public IEnumerable<MapMobile> MobilesWhere(Func<MapMobile, bool> predicate)
        {
            return mobilesPresent.Where(predicate);
        }

        #endregion

        private SpriteRenderer _spriteRenderer = null;
        public SpriteRenderer spriteRenderer
        {
            get
            {
                if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }

        #region Presentation Properties

        private TerrainType _type = TerrainType.grass;
        public TerrainType type
        {
            get
            {
                return _type;
            }

            set
            {
                _type = value;
                __UpdatePresentation(true);
            }
        }

        private Visibility _visibility = Visibility.darkness;
        public Visibility visibility
        {
            get
            {
                return _visibility;
            }

            set
            {
                _visibility = value;
                __UpdatePresentation(true);
            }
        }

        private Audibility _audibility = Audibility.inaudible;
        public Audibility audibility
        {
            get
            {
                return _audibility;
            }

            set
            {
                _audibility = value;
                __UpdatePresentation(true);
            }
        }

        public override void __UpdatePresentation(bool forceUpdate)
        {
            switch (type)
            {
                case TerrainType.grass: spriteRenderer.sprite = grassSprite; break;
                case TerrainType.water: spriteRenderer.sprite = waterSprite; break;
                case TerrainType.mountain: spriteRenderer.sprite = mountainSprite; break;
            }

            switch (visibility)
            {
                case Visibility.darkness: Color = Color.black; break;
                case Visibility.fogOfWar: Color = Color.gray; break;
                case Visibility.visible: Color = Color.white; break;
            }

            mobilesPresent.ForEach(x => x.visibility = visibility);
            mobilesPresent.ForEach(x => x.audibility = audibility);
        }

        #endregion

        #region Sprite passthrough

        public override Color Color
        {
            get
            {
                return spriteRenderer.color;
            }

            set
            {
                spriteRenderer.color = value;
            }
        }

        public override Vector2 Dimensions
        {
            get
            {
                return spriteRenderer.bounds.size;
            }
        }

        public override void SetAngle(float angle)
        {
            spriteRenderer.transform.SetLocalRotationZ(angle);
        }

        public override void AddAngle(float angle)
        {
            spriteRenderer.transform.RotateAroundZ(angle);
        }

        #endregion

        // Use this for initialization
        void Start()
        {
            __UpdatePresentation(true);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
