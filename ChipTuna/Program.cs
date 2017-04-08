using System.IO;
using System.IO.Compression;
using ChipTuna.IO;
using ChipTuna.Vgm.Commands;
using ChipTuna.Vgm.Reading;
using ChipTuna.Vgm.VersionAbstractionLayer;
using ChipTuna.Wave;

namespace ChipTuna
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var fileInfo = new FileInfo(args[0]);

            using (var stream = new GZipStream(fileInfo.OpenRead(), CompressionMode.Decompress))
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var reader = new StreamSequentialReader(binaryReader);

                    var header = HeaderReader.Read(reader);
                    var commands = CommandsReader.Read(header, reader);

                    var psg = new PsgOscillator();
                    var waveData = CreateWaveData(header.GetSamplesCount());
                    var sampleNumber = 0;
                    var amplitude = 15000f;

                    foreach (var command in commands)
                    {
                        switch (command)
                        {
                            case PsgWriteCommand wc:
                                psg.ApplyCommand(wc.Data);
                                break;
                            case WaitNSamplesCommand wnc:
                                for (uint i = 0; i < wnc.SamplesNumber; i++)
                                {
                                    var value = psg.Step();
                                    waveData.Samples[sampleNumber++] = (short)(amplitude * value);
                                }
                                break;
                        }
                    }

                    using (var outputStream = new FileStream(args[0] + ".wav", FileMode.Create))
                    {
                        using (var writer = new BinaryWriter(outputStream))
                        {
                            WaveWriter.WriteWaveFile(waveData, writer);
                        }
                    }
                }
            }
        }

        private static WaveData<short> CreateWaveData(long sampleCount)
        {
            const uint sampleRate = 44100;
            var data = new short[sampleCount];

            return new WaveData<short>
            {
                SampleRate = sampleRate,
                NumberOfChannels = 1,
                BitsPerSample = 16,

                Samples = data
            };
        }
    }
}
