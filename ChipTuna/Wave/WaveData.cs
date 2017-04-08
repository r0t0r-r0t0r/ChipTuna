namespace ChipTuna.Wave
{
    public class WaveData<TSample>
    {
        public uint SampleRate;
        public ushort NumberOfChannels;
        public ushort BitsPerSample;

        public TSample[] Samples;
    }
}