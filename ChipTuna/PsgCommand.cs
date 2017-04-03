namespace ChipTuna
{
    public class PsgCommand
    {
    }

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

    public class DataPsgCommand: PsgCommand
    {
        public DataPsgCommand(int data)
        {
            Data = data;
        }

        public int Data { get; }
    }

    public enum PsgChannel
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3
    }

    public enum PsgLatchType
    {
        ToneOrNoise = 0,
        Volume = 1
    }

    public static class PsgCommandUtils
    {
        public static PsgCommand Create(byte data)
        {
            if ((data & 0b1000_0000) != 0)
            {
                var channel = (data & 0b0110_0000) >> 5;
                var latchType = (data & 0b0001_0000) != 0 ? PsgLatchType.Volume : PsgLatchType.ToneOrNoise;
                var fourLowBits = data & 0b0000_1111;
                return new LatchPsgCommand((PsgChannel)channel, latchType, fourLowBits);
            }
            else
            {
                return new DataPsgCommand(data & 0b0011_1111);
            }
        }
    }
}
