namespace ChipTuna
{
    public struct VgmHeader
    {
        public VgmHeaderCommon Common;
        public VgmHeader100 V100;
        public VgmHeader101 V101;
        public VgmHeader110 V110;
        public VgmHeader150 V150;
    }

    public struct VgmHeaderCommon
    {
        public const uint VgmIdentificationOffset = 0x00;
        public const uint EofOffsetOffset = 0x04;
        public const uint VersionOffset = 0x08;

        public const uint VgmIdentificationValue = 0x206d6756;

        public uint VgmIdentification;
        public uint EofOffset;
        public uint Version;
    }

    public struct VgmHeader100
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

    public struct VgmHeader101
    {
        public const uint RateOffset = 0x24;

        public uint Rate;
    }
    
    public struct VgmHeader110
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

    public struct VgmHeader150
    {
        public const uint VGMDataOffsetOffset = 0x34;

        public uint VGMDataOffset;
    }

    public struct VgmHeader151
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

    public struct VgmHeader160
    {
        public byte VolumeModifier;
        public byte LoopBase;
    }

    public struct VgmHeader161
    {
        public uint GameBoyDMGClock;
        public uint NesApuClock;
        public uint MultiPCMClock;
        public uint UPD7759Clock;
        public uint OKIM6258Clock;
        public byte OKIM6258Flags;
        public byte K054539Flags;
        public byte C140ChipType;
        public uint OKIM6295Clock;
        public uint K051649Clock;
        public uint K054539Clock;
        public uint HuC6280Clock;
        public uint C140Clock;
        public uint K053260Clock;
        public uint PokeyClock;
        public uint QSoundClock;
    }

    public struct VgmHeader170
    {
        public uint ExtraHeaderOffset;
    }

}
