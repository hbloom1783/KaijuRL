using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KaijuRL.Map
{
    [AddComponentMenu("KaijuRL/Map/Enemy Mobile")]
    public class EnemyMobile : MapMobile
    {
        private Slider _hpBar = null;
        public Slider hpBar
        {
            get
            {
                if (_hpBar == null) _hpBar = uiCanvas.GetComponentInChildren<Slider>();
                return _hpBar;
            }
        }

        public override bool CanSpawn(MapCell cell)
        {
            if (occupiesSpace && cell.AnyMobile(x => x.occupiesSpace))
                return false;
            else
                return cell.type == TerrainType.grass;
        }

        public override bool CanEnter(MapCell cell)
        {
            if (occupiesSpace && cell.AnyMobile(x => x.occupiesSpace))
                return false;
            else
                return cell.type == TerrainType.grass;
        }

        public override bool CanSeeThru(MapCell cell)
        {
            return cell.type != TerrainType.mountain;
        }

        public override int CostToEnter(MapCell cell)
        {
            switch (cell.type)
            {
                case TerrainType.grass: return 100;
                case TerrainType.water: return 200;
                default: return 0;
            }
        }

        public override bool IsHostile(MapMobile other)
        {
            return !(other is EnemyMobile);
        }

        public new void Start()
        {
            base.Start();

            hp.OnValueChange += () =>
            {
                if (hp.Value <= 0) Die();
                else
                {
                    hpBar.value = hp.Value / (float)startingHp;
                }
            };
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
