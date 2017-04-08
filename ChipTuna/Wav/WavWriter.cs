using System;
using System.IO;

namespace ChipTuna.Wav
{
    public static class WavWriter
    {

        public static void Write(Wave<short> wave, BinaryWriter writer)
        {
            Write(wave, writer, (w, s) => w.Write(s));
        }

        public static void Write<T>(Wave<T> wave, BinaryWriter writer,
            Action<BinaryWriter, T> writeSample)
        {
            int bytesPerSample = wave.BitsPerSample / 8;

            const uint fmtSubchunkSize = 16;
            uint dataSubchunkSize = (uint) (wave.Samples.Length * bytesPerSample);
            uint riffChunkSize = 4 + (8 + fmtSubchunkSize) + (8 + dataSubchunkSize);

            var riffChunk = new RiffChunk
            {
                Header = new ChunkHeader
                {
                    ChunkId = WavConstants.RiffChunkId,
                    ChunkSize = riffChunkSize
                },
                Format = WavConstants.RiffFormat
            };
            var fmtChunk = new FmtChunk
            {
                Header = new ChunkHeader
                {
                    ChunkId = WavConstants.FmtChunkId,
                    ChunkSize = fmtSubchunkSize
                },
                AudioFormat = WavConstants.PcmAudioFormat,
                NumberOfChannels = wave.NumberOfChannels,
                SampleRate = wave.SampleRate,
                ByteRate = (uint) (wave.SampleRate * wave.NumberOfChannels * bytesPerSample),
                BlockAlign = (ushort) (wave.NumberOfChannels * bytesPerSample),
                BitsPerSample = wave.BitsPerSample
            };
            var dataChunk = new DataChunk<T>
            {
                Header = new ChunkHeader
                {
                    ChunkId = WavConstants.DataChunkId,
                    ChunkSize = dataSubchunkSize
                },
                Data = wave.Samples
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
}
