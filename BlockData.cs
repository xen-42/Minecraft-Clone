using System;
using System.Collections.Generic;
using MinecraftClone.Blocks;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftClone
{
    public class BlockData
    {
        public bool Transparent = false;
        public string type = "Dirt";

        public static Dictionary<string, Block> block_types = GetDictBlockTypes();

        static Dictionary<string, Block> GetDictBlockTypes()
        {
            Dictionary<string, Block> blockdict = new Dictionary<string, Block>();
            blockdict["Air"] = new Air();
            blockdict["Cactus"] = new Cactus();
            blockdict["Dirt"] = new Dirt();
            blockdict["Grass"] = new Grass();
            blockdict["Leaf"] = new Leaf();
            blockdict["Log"] = new Log();
            blockdict["Pine_Leaf"] = new Pine_Leaf();
            blockdict["Sand"] = new Sand();
            blockdict["Snow"] = new Snow();
            blockdict["Stone"] = new Stone();
            blockdict["Tall_Grass"] = new Tall_Grass();
            blockdict["Wood"] = new Wood();
            blockdict["Flower"] = new Flower();

            return blockdict;
        }
        static private void Create_Blocktypes()
        {
            block_types["Dirt"] = new Dirt();
            block_types["Air"] = new Air();
            block_types["Cactus"] = new Cactus();
            block_types["Grass"] = new Grass();
            block_types["Leaf"] = new Leaf();
            block_types["Log"] = new Log();
            block_types["Pine_Leaf"] = new Pine_Leaf();
            block_types["Sand"] = new Sand();
            block_types["Snow"] = new Snow();
            block_types["Tall_Grass"] = new Tall_Grass();
            block_types["Wood"] = new Wood();
            block_types["Stone"] = new Stone();
            block_types["Flower"] = new Flower();
        }
        public BlockData()
        {
            Create_Blocktypes();
        }

        public void create(string t)
        {
            if(block_types == null)
            {
                block_types = GetDictBlockTypes();
            }
            type = t;
            Transparent = block_types[t].TagsList.Contains(Tags.Transparent);
        }
    }
}
