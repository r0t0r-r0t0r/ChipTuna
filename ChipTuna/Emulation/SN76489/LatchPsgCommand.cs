namespace ChipTuna.Emulation.SN76489
{
    public class LatchPsgCommand: PsgCommand
    {
        public LatchPsgCommand(PsgChannel channel, PsgLatchType type, int fourLowBits)
        {
            Channel = channel;
            Type = type;
            FourLowBits = fourLowBits;
        }

        public PsgChannel Channel { get; }
        public PsgLatchType Type { get; }
        public int FourLowBits { get; }
    }
}