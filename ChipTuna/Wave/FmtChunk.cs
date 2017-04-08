namespace ChipTuna.Wave
{
    public struct FmtChunk
    {
        public ChunkHeader Header;
        public ushort AudioFormat;
        public ushort NumberOfChannels;
        public uint SampleRate;
        public uint ByteRate;
        public ushort BlockAlign;
        public ushort BitsPerSample;
    }
}