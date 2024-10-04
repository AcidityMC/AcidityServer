using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcidityServer.Server.World
{
    public class World
    {
        private Dictionary<(int, int), Chunk> chunks = new Dictionary<(int, int), Chunk>();

        public Chunk GetChunk(int x, int z)
        {
            var key = (x, z);
            if (!chunks.ContainsKey(key))
            {
                chunks[key] = new Chunk(x, z);
            }
            return chunks[key];
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            int chunkX = x >> 4; // Equivalent to x / 16
            int chunkZ = z >> 4; // Equivalent to z / 16
            Chunk chunk = GetChunk(chunkX, chunkZ);
            chunk.SetBlock(x & 15, y, z & 15, block);
        }

        public Block GetBlock(int x, int y, int z)
        {
            int chunkX = x >> 4;
            int chunkZ = z >> 4;
            Chunk chunk = GetChunk(chunkX, chunkZ);
            return chunk.GetBlock(x & 15, y, z & 15);
        }

        // TODO: Implement saving, world generation, etc.
        // Refer to MCP source code for this.
    }
}
