namespace ChipTuna.Vgm.Headers
{
    public struct CommonHeaderPart
    {
        public const uint VgmIdentificationOffset = 0x00;
        public const uint EofOffsetOffset = 0x04;
        public const uint VersionOffset = 0x08;

        public const uint VgmIdentificationValue = 0x206d6756;

        public uint VgmIdentification;
        public uint EofOffset;
        public FormatVersion Version;
    }
}