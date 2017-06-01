using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Map
    {
        public const int TileSize = 32;

        public List<List<int>> MapData { get; set; }
        public MapSize MapSize { get; set; }

        public Map(MapSize mapSize)
        {
            MapData = new List<List<int>>((int)mapSize);
            MapSize = mapSize;

            GenerateMap(mapSize);
        }

        private void GenerateMap(MapSize mapSize)
        {
            for (int i = 0; i < MapData.Capacity; i++)
            {
                var tempList = new List<int>((int)mapSize);
                for (int j = 0; j < tempList.Capacity; j++)
                {
                    if(i >= 32) tempList.Add(1);
                    else tempList.Add(0);
                }

                MapData.Add(tempList);
            }
        }
    }
}
