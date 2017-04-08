namespace ChipTuna.Wav
{
    public struct DataChunk<T>
    {
        public ChunkHeader Header;
        public T[] Data;
    }
}