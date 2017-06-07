using System.Collections.Generic;
using Library.Tiles;
using MapMaker.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library
{
    public class Map
    {
        public const int TileSize = 32;

        public List<List<ITile>> MapData { get; set; }
        public MapSize MapSize { get; set; }
        public bool Dirty { get; set; }

        public Map(MapSize mapSize)
        {
            MapData = new List<List<ITile>>((int)mapSize);
            MapSize = mapSize;
            Dirty = false;

            GenerateMap(mapSize);
        }

        private void GenerateMap(MapSize mapSize)
        {
            for (int i = 0; i < MapData.Capacity; i++)
            {
                var tempList = new List<ITile>((int)mapSize);
                for (int j = 0; j < tempList.Capacity; j++)
                {
                    if (i > 32) tempList.Add(new Dirt());
                    else if(i == 32 && j == 0) tempList.Add(new Dirt());
                    else tempList.Add(new Air());
                }

                MapData.Add(tempList);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tileset)
        {
            for (int i = 0; i < MapData.Count; i++)
            {
                for (int j = 0; j < MapData[i].Count; j++)
                {
                    MapData[i][j].Draw(spriteBatch, tileset, new Vector2(j * TileSize, i * TileSize));
                }
            }
        }
    }
}
