using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KaijuRL.Map
{
    [AddComponentMenu("KaijuRL/Map/Facing Indicator")]
    [RequireComponent(typeof(SpriteRenderer))]
    public class FacingIndicator : MonoBehaviour
    {
        public Sprite neSprite;
        public Sprite eSprite;
        public Sprite seSprite;
        public Sprite swSprite;
        public Sprite wSprite;
        public Sprite nwSprite;

        private SpriteRenderer _spriteRenderer = null;
        public SpriteRenderer spriteRenderer
        {
            get
            {
                if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }

        private Facing _facing = Facing.e;
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

        private void UpdatePresentation()
        {
            if (facing == Facing.ne) spriteRenderer.sprite = neSprite;
            else if (facing == Facing.e) spriteRenderer.sprite = eSprite;
            else if (facing == Facing.se) spriteRenderer.sprite = seSprite;
            else if (facing == Facing.sw) spriteRenderer.sprite = swSprite;
            else if (facing == Facing.w) spriteRenderer.sprite = wSprite;
            else if (facing == Facing.nw) spriteRenderer.sprite = nwSprite;
        }

        // Use this for initialization
        void Start()
        {
            UpdatePresentation();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
