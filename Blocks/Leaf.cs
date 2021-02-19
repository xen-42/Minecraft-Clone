using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Blocks
{
    class Leaf: Block
    {
        public Leaf()
        {
            Top = new Vector2(2, 1);
            Bottom = new Vector2(2, 1);
            Left = new Vector2(2, 1);
            Right = new Vector2(2, 1);
            Front = new Vector2(2, 1);
            Back = new Vector2(2, 1);
            Left = new Vector2(2, 1);
            TagsList = new List<Tags>() {Tags.Transparent };
        }
    }
}
