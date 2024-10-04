using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AcidityServer.Server.World
{
    public class Block
    {
        public BlockTypes.BlockType Type { get; set; }

        public Block(BlockTypes.BlockType type)
        {
            Type = type;
        }

        // TODO: Add properties
    }
}