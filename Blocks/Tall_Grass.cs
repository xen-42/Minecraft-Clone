using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Blocks
{
    class Tall_Grass: Block
    {
        public Tall_Grass()
        {
            Only = new Vector2(1, 3);
            TagsList = new List<Tags>() { Tags.Flat, Tags.Transparent, Tags.No_Collision };
        }
    }
}
