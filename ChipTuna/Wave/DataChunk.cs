namespace ChipTuna.Wave
{
    public struct DataChunk<T>
    {
        public ChunkHeader Header;
        public T[] Data;
    }
}