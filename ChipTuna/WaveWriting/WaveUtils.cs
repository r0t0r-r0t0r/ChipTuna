using System;
using System.IO;
using System.Text;

namespace ChipTuna.WaveWriting
{
    public static class WaveUtils
    {
        public static uint StringToId(string str)
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

        public static void WriteWave<T>(Wave<T> wave, BinaryWriter writer, Action<BinaryWriter, T> writeSample)
        {
            int bytesPerSample = wave.BitsPerSample / 8;

            const uint fmtSubchunkSize = 16;
            uint dataSubchunkSize = (uint)(wave.Data.Length * bytesPerSample);
            uint riffChunkSize = 4 + (8 + fmtSubchunkSize) + (8 + dataSubchunkSize);

            var riffChunk = new RiffChunk
            {
                Header = new ChunkHeader
                {
                    ChunkId = WaveConstants.RiffChunkId,
                    ChunkSize = riffChunkSize
                },
                Format = WaveConstants.RiffFormat
            };
            var fmtChunk = new FmtChunk
            {
                Header = new ChunkHeader
                {
                    ChunkId = WaveConstants.FmtChunkId,
                    ChunkSize = fmtSubchunkSize
                },
                AudioFormat = WaveConstants.PcmAudioFormat,
                NumberOfChannels = wave.NumberOfChannels,
                SampleRate = wave.SampleRate,
                ByteRate = (uint)(wave.SampleRate * wave.NumberOfChannels * bytesPerSample),
                BlockAlign = (ushort)(wave.NumberOfChannels * bytesPerSample),
                BitsPerSample = wave.BitsPerSample
            };
            var dataChunk = new DataChunk<T>
            {
                Header = new ChunkHeader
                {
                    ChunkId = WaveConstants.DataChunkId,
                    ChunkSize = dataSubchunkSize
                },
                Data = wave.Data
            };

            WriteChunkHeader(writer, riffChunk.Header);
            writer.Write(riffChunk.Format);

            WriteChunkHeader(writer, fmtChunk.Header);
            writer.Write(fmtChunk.AudioFormat);
            writer.Write(fmtChunk.NumberOfChannels);
            writer.Write(fmtChunk.SampleRate);
            writer.Write(fmtChunk.ByteRate);
            writer.Write(fmtChunk.BlockAlign);
            writer.Write(fmtChunk.BitsPerSample);

            WriteChunkHeader(writer, dataChunk.Header);
            foreach (var sample in dataChunk.Data)
            {
                writeSample(writer, sample);
            }
        }

        private static void WriteChunkHeader(BinaryWriter writer, ChunkHeader header)
        {
            writer.Write(header.ChunkId);
            writer.Write(header.ChunkSize);
        }
    }

    public struct ChunkHeader
    {
        public uint ChunkId;
        public uint ChunkSize;
    }

    public struct RiffChunk
    {
        public ChunkHeader Header;
        public uint Format;
    }

    public struct FmtChunk
    {
        public ChunkHeader Header;
        public ushort AudioFormat;
        public ushort NumberOfChannels;
        public uint SampleRate;
        public uint ByteRate;
        public ushort BlockAlign;
        public ushort BitsPerSample;
    }

    public struct DataChunk<T>
    {
        public ChunkHeader Header;
        public T[] Data;
    }

    public static class WaveConstants
    {
        public static readonly uint RiffChunkId = WaveUtils.StringToId("RIFF");
        public static readonly uint RiffFormat = WaveUtils.StringToId("WAVE");
        public static readonly uint FmtChunkId = WaveUtils.StringToId("fmt ");
        public static readonly uint DataChunkId = WaveUtils.StringToId("data");
        public const ushort PcmAudioFormat = 1;
    }

    public struct Wave<T>
    {
        public uint SampleRate;
        public ushort NumberOfChannels;
        public ushort BitsPerSample;

        public T[] Data;
    }
}
