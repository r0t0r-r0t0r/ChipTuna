namespace ChipTuna.Vgm.Headers
{
    public struct V110HeaderPart
    {
        public const uint SN76489FeedbackOffset = 0x28;
        public const uint SN76489ShiftRegisterWidthOffset = 0x2A;

        public const uint YM2612ClockOffset = 0x2C;
        public const uint YM2151ClockOffset = 0x30;

        public ushort SN76489Feedback;
        public byte SN76489ShiftRegisterWidth;

        public uint YM2612Clock;
        public uint YM2151Clock;
    }
}