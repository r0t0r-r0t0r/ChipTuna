namespace ChipTuna.Vgm.Headers
{
    public struct V100HeaderPart
    {
        public const uint SN76489ClockOffset = 0x0C;
        public const uint YM2413ClockOffset = 0x10;
        public const uint GD3OffsetOffset = 0x14;
        public const uint TotalNumberOfSamplesOffset = 0x18;
        public const uint LoopOffsetOffset = 0x1C;
        public const uint LoopNumberOfSamplesOffset = 0x20;

        public uint SN76489Clock;
        public uint YM2413Clock;
        public uint GD3Offset;
        public uint TotalNumberOfSamples;
        public uint LoopOffset;
        public uint LoopNumberOfSamples;
    }
}