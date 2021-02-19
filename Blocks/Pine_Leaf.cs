using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Blocks
{
    class Pine_Leaf: Leaf
    {
        public Pine_Leaf() : base()
        {
            Top = new Vector2(3, 1);
            Bottom = new Vector2(3, 1);
            Left = new Vector2(3, 1);
            Right = new Vector2(3, 1);
            Front = new Vector2(3, 1);
            Back = new Vector2(3, 1);
            Left = new Vector2(3, 1);
        }
    }
}
