namespace ChipTuna.Vgm.Headers
{
    public struct V151HeaderPart
    {
        public const uint SN76489FlagsOffset = 0x2B;

        public byte SN76489Flags;

        public uint SegaPCMClock;
        public uint SegaPCMInterfaceRegister;
        public uint RF5C68Clock;
        public uint YM2203Clock;
        public uint YM2608Clock;
        public uint YM2610BClock;
        public uint YM3812Clock;
        public uint YM3526Clock;
        public uint Y8950Clock;
        public uint YMF262Clock;
        public uint YMF278BClock;
        public uint YMF271Clock;
        public uint YMZ280BClock;
        public uint RF5C164Clock;
        public uint PWMClock;
        public uint AY8910Clock;
        public byte AY8910ChipType;
        public byte AY8910Flags;
        public byte YM2203AndAY8910Flags;
        public byte YM2608AndAY8910Flags;

        public byte LoopModifier;
    }
}