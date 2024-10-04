using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcidityServer.Server.World
{
    public class Chunk
    {
        public const int Width = 16;
        public const int Height = 256;
        public const int Depth = 16;
        private Block[,,] blocks;
        public int ChunkX { get; private set; }
        public int ChunkZ { get; private set; }

        public Chunk(int chunkX, int chunkZ)
        {
            ChunkX = chunkX;
            ChunkZ = chunkZ;
            blocks = new Block[Width, Height, Depth];
            InitializeBlocks();
        }

        private void InitializeBlocks()
        {
            // Initialize blocks (e.g., air blocks)
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    for (int z = 0; z < Depth; z++)
                        blocks[x, y, z] = new Block(BlockTypes.BlockType.Air);
        }

        public Block GetBlock(int x, int y, int z)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height || z < 0 || z >= Depth)
                throw new IndexOutOfRangeException("Block coordinates out of range.");
            return blocks[x, y, z];
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height || z < 0 || z >= Depth)
                throw new IndexOutOfRangeException("Block coordinates out of range.");
            blocks[x, y, z] = block;
        }
    }
}
