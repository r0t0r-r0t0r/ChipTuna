namespace ChipTuna.Emulation.SN76489
{
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