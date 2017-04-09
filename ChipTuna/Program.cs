using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using ChipTuna.IO;
using ChipTuna.Emulation.SN76489;
using ChipTuna.Vgm.Commands;
using ChipTuna.Vgm.Headers;
using ChipTuna.Vgm.Reading;
using ChipTuna.Vgm.VersionAbstractionLayer;
using ChipTuna.Wav;

namespace ChipTuna
{
    internal delegate void VgmFileProcessor(string fileName, VgmHeader header, IEnumerable<VgmCommand> commands);

    public class Program
    {
        public static void Main(string[] args)
        {
            var vgmFileName = args[0];

            ProcessVgm(vgmFileName, CalculateCommandsStatistics);
            ProcessVgm(vgmFileName, RenderVgmToWav);
        }

        private static void ProcessVgm(string fileName, VgmFileProcessor processor)
        {
            var fileInfo = new FileInfo(fileName);

            using (var stream = new GZipStream(fileInfo.OpenRead(), CompressionMode.Decompress))
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var reader = new StreamSequentialReader(binaryReader);

                    var header = HeaderReader.Read(reader);
                    var commands = CommandsReader.Read(header, reader);

                    processor(fileName, header, commands);
                }
            }
        }

        private static void CalculateCommandsStatistics(string fileName, VgmHeader header, IEnumerable<VgmCommand> commands)
        {
            var stat = commands.GroupBy(x => x.Code)
                .Select(x => new {Count = x.Count(), Code = x.Key})
                .OrderBy(x => x.Code)
                .ToList();

            File.WriteAllLines(fileName + ".txt", stat.Select(x => $"0x{x.Code:X2}: {x.Count}"));
        }

        private static void RenderVgmToWav(string fileName, VgmHeader header, IEnumerable<VgmCommand> commands)
        {
            var psg = new PsgOscillator();
            var wave = CreateWave(header.GetSamplesCount());
            var sampleNumber = 0;
            var amplitude = 15000f;

            void Wait(uint samplesNumber)
            {
                for (uint i = 0; i < samplesNumber; i++)
                {
                    var value = psg.Step();
                    wave.Samples[sampleNumber++] = (short) (amplitude * value);
                }
            }

            foreach (var command in commands)
            {
                switch (command)
                {
                    case PsgWriteCommand wc:
                        psg.ApplyCommand(wc.Data);
                        break;
                    case WaitNSamplesCommand wnc:
                        Wait(wnc.SamplesNumber);
                        break;
                    case YM2612Port0Address2AWriteThenWaitNSamplesCommand wwc:
                        Wait(wwc.SamplesNumber);
                        break;
                }
            }

            using (var outputStream = new FileStream(fileName + ".wav", FileMode.Create))
            {
                using (var writer = new BinaryWriter(outputStream))
                {
                    WavWriter.Write(wave, writer);
                }
            }
        }

        private static Wave<short> CreateWave(long sampleCount)
        {
            const uint sampleRate = 44100;
            var data = new short[sampleCount];

            return new Wave<short>
            {
                SampleRate = sampleRate,
                NumberOfChannels = 1,
                BitsPerSample = 16,

                Samples = data
            };
        }
    }
}
