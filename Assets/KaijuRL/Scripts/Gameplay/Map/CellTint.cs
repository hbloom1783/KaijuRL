using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KaijuRL.Extensions;

namespace KaijuRL.Map
{
    [RequireComponent(typeof(MapCell))]
    public class CellTint : MonoBehaviour
    {
        public float cycleTime = 0.25f;
        public Color startColor = Color.white;
        public Color endColor = Color.gray;

        private MapCell _mapCell = null;
        public MapCell mapCell
        {
            get
            {
                if (_mapCell == null) _mapCell = GetComponent<MapCell>();
                return _mapCell;
            }
        }

        private float modTime = 0.0f;
        private Color oldColor;

        void Start()
        {
            oldColor = mapCell.spriteRenderer.color;

            if (mapCell.visibility == Visibility.visible)
            {
                mapCell.spriteRenderer.color = startColor;
            }
            else
            {
                Terminate();
            }
        }

        void CycleEnd()
        {
            Color tempColor = startColor;
            startColor = endColor;
            endColor = tempColor;
        }

        void Update()
        {
            if (mapCell.visibility == Visibility.visible)
            {
                modTime += Time.deltaTime;

                while (modTime > cycleTime)
                {
                    modTime -= cycleTime;
                    CycleEnd();
                }

                mapCell.spriteRenderer.color = Color.Lerp(startColor, endColor, modTime / cycleTime);
            }
        }

        public void Terminate()
        {
            mapCell.spriteRenderer.color = oldColor;
            Destroy(this);
        }
    }
}