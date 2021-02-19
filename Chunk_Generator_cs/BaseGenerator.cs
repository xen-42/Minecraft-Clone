using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftClone;
using Godot;

namespace MinecraftClone.Chunk_Generator_cs
{
    class BaseGenerator
    {
        public virtual string generate_surface(int height, int x, int y, int z)
        {
            if( y == 0)
            {
                return "Stone";
            }
            else
            {
                return "Air";
            }
        }

        public virtual void generate_details(Chunk_cs chunk, RandomNumberGenerator rng, int[,] ground_height)
        {
            return;
        }
    }
}
