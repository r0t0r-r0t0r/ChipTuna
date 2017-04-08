using System;
using System.IO;

namespace ChipTuna.Wave
{
    public static class WaveWriter
    {

        public static void WriteWaveFile(WaveData<short> waveData, BinaryWriter writer)
        {
            WriteWaveFile(waveData, writer, (w, s) => w.Write(s));
        }

        public static void WriteWaveFile<T>(WaveData<T> waveData, BinaryWriter writer,
            Action<BinaryWriter, T> writeSample)
        {
            int bytesPerSample = waveData.BitsPerSample / 8;

            const uint fmtSubchunkSize = 16;
            uint dataSubchunkSize = (uint) (waveData.Samples.Length * bytesPerSample);
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
                NumberOfChannels = waveData.NumberOfChannels,
                SampleRate = waveData.SampleRate,
                ByteRate = (uint) (waveData.SampleRate * waveData.NumberOfChannels * bytesPerSample),
                BlockAlign = (ushort) (waveData.NumberOfChannels * bytesPerSample),
                BitsPerSample = waveData.BitsPerSample
            };
            var dataChunk = new DataChunk<T>
            {
                Header = new ChunkHeader
                {
                    ChunkId = WaveConstants.DataChunkId,
                    ChunkSize = dataSubchunkSize
                },
                Data = waveData.Samples
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
