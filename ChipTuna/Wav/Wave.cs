namespace ChipTuna.Wav
{
    public class Wave<TSample>
    {
        public uint SampleRate;
        public ushort NumberOfChannels;
        public ushort BitsPerSample;

        public TSample[] Samples;
    }
}