using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Blocks
{
    class Sand: Block
    {
        public Sand()
        {
            Top = new Vector2(2, 2);
            Bottom = new Vector2(2, 2);
            Left = new Vector2(2, 2);
            Right = new Vector2(2, 2);
            Front = new Vector2(2, 2);
            Back = new Vector2(2, 2);
            Left = new Vector2(2, 2);
            TagsList = new List<Tags>();
        }
    }
}
