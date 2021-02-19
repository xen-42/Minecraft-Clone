using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Blocks
{
    class Flower: Block
    {
        public Flower()
        {
            Only = new Vector2(2, 3);
            TagsList = new List<Tags>() { Tags.Flat, Tags.Transparent, Tags.No_Collision };
        }
    }
}

