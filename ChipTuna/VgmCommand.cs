namespace ChipTuna
{
    public abstract class VgmCommand
    {
        public VgmCommand(byte code)
        {
            Code = code;
        }
        public byte Code { get; }
    }

    public class GenericVgmCommand: VgmCommand
    {
        public GenericVgmCommand(byte code, byte[] data)
            : base(code)
        {
            Data = data;
        }

        public byte[] Data { get; }
    }

    public class PsgWriteVgmCommand: VgmCommand
    {
        public PsgWriteVgmCommand(byte code, byte data)
            : base(code)
        {
            Data = data;
        }

        public byte Data { get; }
    }

    public class WaitNSamplesVgmCommand: VgmCommand
    {
        public WaitNSamplesVgmCommand(byte code, ushort samplesNumber)
            : base(code)
        {
            SamplesNumber = samplesNumber;
        }

        public ushort SamplesNumber { get; }
    }

    public class Wait735SamplesVgmCommand: VgmCommand
    {
        public Wait735SamplesVgmCommand(byte code)
            : base(code)
        {
        }
    }

    public class EndOfSoundDataVgmCommand: VgmCommand
    {
        public EndOfSoundDataVgmCommand(byte code)
            : base(code)
        {
        }
    }

    public enum VgmCommandType
    {
        GameGearPSGStereo,
        PSG,
        YM2413,
        YM2612Port0,
        YM2612Port1,
        YM2151,
        YM2203,
        YM2608Port0,
        YM2608Port1,
        YM2610Port0,
        YM2610Port1,
        YM3812,
        YM3526,
        Y8950,
        YMZ280B,
        YMF262Port0,
        YMF262Port1,
        WaitNSamples,
        Wait735Samples,
        Wait882Samples,
        OverrideLengthOf0x62Or0x63,
        EndOfSoundData,
        DataBlock,
        PcmRamWrite,
        WaitNPlusOneSamples,
        YM2612Port0Address2AAndWait,
        DacStreamControlWrite,
    }
}
