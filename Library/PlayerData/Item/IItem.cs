using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Library.Player.Item
{
    public interface IItem : IDrawable
    {
        void Use(float mouseX, float mouseY, PlayerData.Player player);
    }
}
