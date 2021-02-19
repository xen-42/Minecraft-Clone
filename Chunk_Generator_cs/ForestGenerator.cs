using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone.Chunk_Generator_cs
{
    class ForestGenerator: BaseGenerator
    {
        public override string generate_surface(int height, int x, int y, int z)
        {
            string type;
            if(y == height - 1)
            {
                type = "Grass";
            }
            else if(y < height - 10 || y == 0)
            {
                type = "Stone";
            }
            else if (y < height - 1)
            {
                type = "Dirt";
            }
            else
            {
                type = "Air";
            }
            return type;
        }

        public override void generate_details(Chunk_cs chunk, RandomNumberGenerator rng, int[,] ground_height)
        {
            int tree_width = 2;

            for(int n_tree = 0; n_tree < rng.RandiRange(2,8); n_tree++)
            {
                int pos_x = rng.RandiRange(tree_width, (int)Chunk_cs.DIMENSION.x - tree_width - 1);
                int pos_z = rng.RandiRange(tree_width, (int)Chunk_cs.DIMENSION.z - tree_width - 1);

                int tree_height = rng.RandiRange(4, 8);
                for(int i = 0; i < tree_height; i++)
                {
                    var b = new BlockData();
                    b.create("Log");
                    var x = pos_x;
                    var z = pos_z;
                    var y = ground_height[x, z] + i;
                    chunk._set_block_data(x, y, z, b);
                }

                var min_y = rng.RandiRange(-2, -1);

                var max_y = rng.RandiRange(2, 4);

                for(int dy = min_y; dy < max_y; dy++)
                {
                    var leaf_width = tree_width;
                    if(dy == min_y || dy == max_y - 1)
                    {
                        leaf_width -= 1;
                    }
                    for (int dx = -leaf_width; dx < leaf_width+1; dx++)
                    {
                        for(int dz = -leaf_width; dz < leaf_width + 1; dz++)
                        {
                            var lx = pos_x + dx;
                            var ly = ground_height[pos_x, pos_z] + tree_height + dy;
                            var lz = pos_z + dz;

                            var l =  new BlockData();
                            l.create("Leaf");
                            chunk._set_block_data(lx, ly, lz, l, false);

                        }
                        if (dy == min_y || dy == max_y - 1)
                        {
                            leaf_width -= 1;
                        }
                    }
                }
                for (int n_shrub = 0; n_shrub < rng.RandiRange(6, 10); n_shrub++)
                {
                    var x = rng.RandiRange(0, (int)(Chunk_cs.DIMENSION.x - 1));
                    var z = rng.RandiRange(0, (int)(Chunk_cs.DIMENSION.z - 1));
                    var y = ground_height[x, z];
                    var b = new BlockData();
                    b.create("Tall_Grass");
                    chunk._set_block_data(x, y, z, b, false);
                }
                for (int n_flower = 0; n_flower < rng.RandiRange(4, 6); n_flower++)
                {
                    var x = rng.RandiRange(0, (int)(Chunk_cs.DIMENSION.x - 1));
                    var z = rng.RandiRange(0, (int)(Chunk_cs.DIMENSION.z - 1));
                    var y = ground_height[x, z];
                    var b = new BlockData();
                    b.create("Flower");
                    chunk._set_block_data(x, y, z, b, false);
                }

            }
            return;
        }
    }
}
