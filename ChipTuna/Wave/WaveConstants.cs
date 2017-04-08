using System;
using System.Text;

namespace ChipTuna.Wave
{
    public static class WaveConstants
    {
        public static readonly uint RiffChunkId = StringToId("RIFF");
        public static readonly uint RiffFormat = StringToId("WAVE");
        public static readonly uint FmtChunkId = StringToId("fmt ");
        public static readonly uint DataChunkId = StringToId("data");
        public const ushort PcmAudioFormat = 1;

        private static uint StringToId(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            var bytes = Encoding.ASCII.GetBytes(str);

            if (bytes.Length != 4)
                throw new ArgumentException("String must contain exactly 4 acii chars", nameof(str));

            uint id = bytes[0];

            id |= (uint)bytes[1] << 8;
            id |= (uint)bytes[2] << 16;
            id |= (uint)bytes[3] << 24;

            return id;
        }
    }
}