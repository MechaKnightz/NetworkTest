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

        public Map() { }
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

        public void Draw(SpriteBatch spriteBatch, Texture2D tileset, SpriteFont font)
        {
            for (int i = 0; i < MapData.Count; i++)
            {
                for (int j = 0; j < MapData[i].Count; j++)
                {
                    MapData[i][j].Draw(spriteBatch, tileset, new Vector2(j * TileSize, i * TileSize));
                    if (MapData[i][j].Health < MapData[i][j].HealthMax)
                    {
                        spriteBatch.DrawString(
                            font,
                            MapData[i][j].Health.ToString(),
                            new Vector2(j * TileSize + TileSize / 2, i * TileSize + TileSize / 2) - font.MeasureString(MapData[i][j].Health.ToString()) / 2,
                            Color.White);
                    }
                }
            }
        }

        public static Map GetEmptyMap(MapSize size)
        {
            var map = new Map();

            map.MapSize = size;
            map.MapData = new List<List<ITile>>();

            for (int i = 0; i < (int)size; i++)
            {
                map.MapData.Add(new List<ITile>());

                for (int j = 0; j < (int)size; j++)
                {
                    map.MapData[i].Add(null);
                }
            }

            return map;
        }
    }
}
