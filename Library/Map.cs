using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Map
    {
        public List<List<int>> MapData { get; set; }

        public Map(MapSize mapSize)
        {
            MapData = new List<List<int>>((int)mapSize);

            for (int i = 0; i < MapData.Capacity; i++)
            {
                var tempList = new List<int>((int)mapSize);
                for (int j = 0; j < tempList.Capacity; j++)
                {
                    tempList.Add(0);
                }
                
                MapData.Add(tempList);
            }
        }
    }
}
