using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KaijuRL.Map
{
    [AddComponentMenu("KaijuRL/Map/Map Mobile")]
    [RequireComponent(typeof(SpriteRenderer))]
    public class MapMobile : MonoBehaviour
    {
        public bool occupiesSpace = true;

        private SpriteRenderer _spriteRenderer = null;
        public SpriteRenderer spriteRenderer
        {
            get
            {
                if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }

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

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
