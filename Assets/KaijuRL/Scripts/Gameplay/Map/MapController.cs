using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Gamelogic.Grids;
using Random = UnityEngine.Random;
using KaijuRL.Extensions;
using System.Collections;

namespace KaijuRL.Map
{
    [System.Serializable]
    public class SpawnEntry
    {
        public MapMobile prefab;
        public int count;
    }

    [AddComponentMenu("KaijuRL/Map/Map Generator")]
    [RequireComponent(typeof(PointyHexTileGridBuilder))]
    public class MapController : GridBehaviour<PointyHexPoint>
    {
        public List<SpawnEntry> spawnTable = new List<SpawnEntry>();

        #region Map Reference

        private IGrid<MapCell, PointyHexPoint> _mapGrid = null;
        public IGrid<MapCell, PointyHexPoint> mapGrid
        {
            get
            {
                if (_mapGrid == null) _mapGrid = Grid.CastValues<MapCell, PointyHexPoint>();

                return _mapGrid;
            }
        }

        public MapCell this[PointyHexPoint loc]
        {
            get
            {
                return (MapCell)Grid[loc];
            }
        }
        
        public PointyHexPoint WhereIs(MapCell cell)
        {
            PointList<PointyHexPoint> pointList = mapGrid.WhereCell(x => x == cell).ToPointList();

            if (pointList.Count == 1)
                return pointList[0];
            else if (pointList.Count > 1)
                throw new ArgumentOutOfRangeException("cell", "Count > 1");
            else
                throw new ArgumentOutOfRangeException("cell", "Count <= 0");
        }

        public PointyHexPoint WhereIs(MapMobile mobile)
        {
            PointList<PointyHexPoint> pointList = mapGrid.WhereCell(x => x.HasMobile(mobile)).ToPointList();

            if (pointList.Count == 1)
                return pointList[0];
            else if (pointList.Count > 1)
                throw new ArgumentOutOfRangeException("mobile", "Count > 1");
            else
                throw new ArgumentOutOfRangeException("mobile", "Count <= 0");
        }

        public MapCell CellAt(PointyHexPoint loc)
        {
            if (InBounds(loc))
                return (MapCell)Grid[loc];
            else
                return null;
        }

        public bool InBounds(PointyHexPoint loc)
        {
            return Grid.Contains(loc);
        }

        #endregion

        #region Mobile Management

        public void PlaceMobile(MapMobile mobile, MapCell cell)
        {
            cell.AddMobile(mobile);
            mobile.transform.position = cell.transform.position;
            mobile.spriteRenderer.sortingOrder = cell.spriteRenderer.sortingOrder + 1;
        }

        public void PlaceMobile(MapMobile mobile, PointyHexPoint point)
        {
            PlaceMobile(mobile, mapGrid[point]);
        }

        public void UnplaceMobile(MapMobile mobile)
        {
            mapGrid[WhereIs(mobile)].RemoveMobile(mobile);
        }

        #endregion

        #region Map Creation

        // After grid instantiates
        public override void InitGrid()
        {
            // Reorder the cells for visual clarity
            foreach (PointyHexPoint point in Grid.CastValues<MapCell, PointyHexPoint>().ToPointList())
            {
                mapGrid[point].spriteRenderer.sortingOrder = -point.Y;
            }

            // Only do map generation while the game is in play
            if (Application.isPlaying)
            {

                // Generate terrain
                PerlinTerrain();

                // Spawn mobiles
                foreach (SpawnEntry spawn in spawnTable)
                {
                    for (int idx = 0; idx < spawn.count; idx++)
                    {
                        MapMobile instance = Instantiate(spawn.prefab);
                        PlaceMobile(instance, mapGrid.WhereCell(instance.CanSpawn).RandomPick());
                        instance.transform.parent = transform;
                    }
                }
            }
        }

        public float waterLine = 0.35f;
        public float treeLine = 0.75f;
        public float perlinScale = 10;

        private void PerlinTerrain()
        {
            float xOffset = Random.Range(0.0f, 10000.0f);
            float yOffset = Random.Range(0.0f, 10000.0f);

            foreach (PointyHexPoint point in mapGrid)
            {
                float perlinX = (Map[point].x * perlinScale) + xOffset;
                float perlinY = (Map[point].y * perlinScale) + yOffset;

                float value = Mathf.PerlinNoise(perlinX, perlinY);

                if (value < waterLine)
                    mapGrid[point].type = TerrainType.water;
                else if (value > treeLine)
                    mapGrid[point].type = TerrainType.mountain;
            }
        }

        #endregion

    }
}
