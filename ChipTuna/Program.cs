using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using ChipTuna.WaveWriting;

namespace ChipTuna
{
    class Program
    {
        static void Main(string[] args)
        {
            //var fileInfo = new FileInfo(Path.Combine(VgmPath, FileName));
            var fileInfo = new FileInfo(args[0]);

            using (var stream = CreateStream(fileInfo))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var sequentialReader = new StreamSequentialReader(reader);

                    var header = ReadHeader(sequentialReader);
                    var vgmDataOffset = GetVGMDataAbsoluteOffset(header);
                    sequentialReader.SkipToAbsoluteOffset(vgmDataOffset);

                    var commandReader = new VgmCommandReader();
                    var commands = GetCommands(sequentialReader, commandReader);

                    var psg = new PsgOscillator();
                    var wave = CreateWave(header.V100.TotalNumberOfSamples);
                    var sampleNumber = 0;
                    var amplitude = 15000f;

                    foreach (var command in commands)
                    {
                        switch (command)
                        {
                            case PsgWriteVgmCommand wc:
                                psg.ApplyCommand(wc.Data);
                                break;
                            case WaitNSamplesVgmCommand wnc:
                                for (uint i = 0; i < wnc.SamplesNumber; i++)
                                {
                                    var value = psg.Step();
                                    wave.Data[sampleNumber++] = (short)(amplitude * value);
                                }
                                break;
                            case Wait735SamplesVgmCommand w735c:
                                for (uint i = 0; i < 735; i++)
                                {
                                    var value = psg.Step();
                                    wave.Data[sampleNumber++] = (short)(amplitude * value);
                                }
                                break;
                        }
                    }

                    using (var outputStream = new FileStream(args[0] + ".wav", FileMode.Create))
                    {
                        using (var writer = new BinaryWriter(outputStream))
                        {
                            WaveUtils.WriteWave(wave, writer, (w, s) => w.Write(s));
                        }
                    }
                }
            }
        }

        private static Wave<short> CreateWave(uint sampleCount)
        {
            const uint sampleRate = 44100;
            var data = new short[sampleCount];

            return new Wave<short>
            {
                SampleRate = sampleRate,
                NumberOfChannels = 1,
                BitsPerSample = 16,

                Data = data
            };
        }

        private static Stream CreateStream(FileInfo fileInfo)
        {
            return new GZipStream(fileInfo.OpenRead(), CompressionMode.Decompress);

            using (var fileStream = fileInfo.OpenRead())
            {
                using (var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
                {
                    var memoryStream = new MemoryStream();
                    gzipStream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    return memoryStream;
                }
            }
        }

        private static VgmHeader ReadHeader(ISequentialReader reader)
        {
            var header = new VgmHeader();
            var unused = new VgmHeader151();

            VgmHeaderReader.ReadCommon(reader, ref header.Common);

            if (header.Common.VgmIdentification != VgmHeaderCommon.VgmIdentificationValue)
            {
                throw new VgmException("Vgm magic number does not found");
            }

            var version = (VgmFormatVersion)header.Common.Version;
            if (version != VgmFormatVersion.V100 &&
                version != VgmFormatVersion.V101 &&
                version != VgmFormatVersion.V110 &&
                version != VgmFormatVersion.V150)
            {
                throw new VgmException("Unsupported version");
            }

            VgmHeaderReader.Read100(reader, ref header.V100);

            if (version == VgmFormatVersion.V100)
                return header;

            VgmHeaderReader.Read101(reader, ref header.V101);

            if (version == VgmFormatVersion.V101)
                return header;

            VgmHeaderReader.Read110Part1(reader, ref header.V110);
            VgmHeaderReader.Read151Part1(reader, ref unused);
            VgmHeaderReader.Read110Part2(reader, ref header.V110);

            if (version == VgmFormatVersion.V110)
                return header;

            VgmHeaderReader.Read150(reader, ref header.V150);
            return header;
        }

        private static uint GetVGMDataAbsoluteOffset(VgmHeader header)
        {
            const uint vgmDataOffsetPriorToV150 = 0x40;

            if (header.V150.VGMDataOffset == 0)
                return vgmDataOffsetPriorToV150;
            else
            {
                var relativeOffset = header.V150.VGMDataOffset;
                return relativeOffset + VgmHeader150.VGMDataOffsetOffset;
            }
        }

        private static IEnumerable<VgmCommand> GetCommands(ISequentialReader sequentialReader, IVgmCommandReader commandReader)
        {
            VgmCommand command;
            do
            {
                var code = sequentialReader.ReadByte();
                command = commandReader.Read(code, sequentialReader);
                yield return command;
            } while (!(command is EndOfSoundDataVgmCommand));
        }
    }
}
