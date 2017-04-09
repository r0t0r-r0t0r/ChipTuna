using System.Collections.Generic;
using ChipTuna.IO;
using ChipTuna.Vgm.Commands;
using ChipTuna.Vgm.Headers;

namespace ChipTuna.Vgm.Reading
{
    public static class CommandsReader
    {
        private const byte EndOfSoundDataCode = 0x66;

        private static readonly IReadOnlyDictionary<byte, CommandBodyReader> Map;

        static CommandsReader()
        {
            Map = new Dictionary<byte, CommandBodyReader>
            {
                [0x4F] = ReadOneByteBody,
                [0x50] = ReadPsgWriteCommandBody,
                [0x51] = ReadTwoBytesBody,
                [0x52] = ReadTwoBytesBody,
                [0x53] = ReadTwoBytesBody,
                [0x54] = ReadTwoBytesBody,
                [0x55] = ReadTwoBytesBody,
                [0x56] = ReadTwoBytesBody,
                [0x57] = ReadTwoBytesBody,
                [0x58] = ReadTwoBytesBody,
                [0x59] = ReadTwoBytesBody,
                [0x5A] = ReadTwoBytesBody,
                [0x5B] = ReadTwoBytesBody,
                [0x5C] = ReadTwoBytesBody,
                [0x5D] = ReadTwoBytesBody,
                [0x5E] = ReadTwoBytesBody,
                [0x5F] = ReadTwoBytesBody,
                [0x61] = ReadWaitNSamplesBody,
                [0x62] = ReadWait735SamplesBody,
                [0x63] = ReadZeroBytesBody,
                [0x64] = ReadThreeBytesBody,
                [EndOfSoundDataCode] = ReadEndOfSoundDataBody,
                [0x67] = ReadDataBlockBody,
                [0x68] = ReadElevenBytesBody,

                [0x70] = ReadWaitNPlusOneSamplesBody,
                [0x71] = ReadWaitNPlusOneSamplesBody,
                [0x72] = ReadWaitNPlusOneSamplesBody,
                [0x73] = ReadWaitNPlusOneSamplesBody,
                [0x74] = ReadWaitNPlusOneSamplesBody,
                [0x75] = ReadWaitNPlusOneSamplesBody,
                [0x76] = ReadWaitNPlusOneSamplesBody,
                [0x77] = ReadWaitNPlusOneSamplesBody,
                [0x78] = ReadWaitNPlusOneSamplesBody,
                [0x79] = ReadWaitNPlusOneSamplesBody,
                [0x7A] = ReadWaitNPlusOneSamplesBody,
                [0x7B] = ReadWaitNPlusOneSamplesBody,
                [0x7C] = ReadWaitNPlusOneSamplesBody,
                [0x7D] = ReadWaitNPlusOneSamplesBody,
                [0x7E] = ReadWaitNPlusOneSamplesBody,
                [0x7F] = ReadWaitNPlusOneSamplesBody,

                [0x80] = ReadYM2612WriteAndWaitBody,
                [0x81] = ReadYM2612WriteAndWaitBody,
                [0x82] = ReadYM2612WriteAndWaitBody,
                [0x83] = ReadYM2612WriteAndWaitBody,
                [0x84] = ReadYM2612WriteAndWaitBody,
                [0x85] = ReadYM2612WriteAndWaitBody,
                [0x86] = ReadYM2612WriteAndWaitBody,
                [0x87] = ReadYM2612WriteAndWaitBody,
                [0x88] = ReadYM2612WriteAndWaitBody,
                [0x89] = ReadYM2612WriteAndWaitBody,
                [0x8A] = ReadYM2612WriteAndWaitBody,
                [0x8B] = ReadYM2612WriteAndWaitBody,
                [0x8C] = ReadYM2612WriteAndWaitBody,
                [0x8D] = ReadYM2612WriteAndWaitBody,
                [0x8E] = ReadYM2612WriteAndWaitBody,
                [0x8F] = ReadYM2612WriteAndWaitBody,

                //DAC Stream Control Write
                [0x90] = ReadFourBytesBody,
                [0x91] = ReadFourBytesBody,
                [0x92] = ReadFiveBytesBody,
                [0x93] = ReadTenBytesBody,
                [0x94] = ReadOneByteBody,
                [0x95] = ReadFourBytesBody,

                [0xA0] = ReadTwoBytesBody,
                [0xB0] = ReadTwoBytesBody,
                [0xB1] = ReadTwoBytesBody,
                [0xB2] = ReadTwoBytesBody,
                [0xB3] = ReadTwoBytesBody,
                [0xB4] = ReadTwoBytesBody,
                [0xB5] = ReadTwoBytesBody,
                [0xB6] = ReadTwoBytesBody,
                [0xB7] = ReadTwoBytesBody,
                [0xB8] = ReadTwoBytesBody,
                [0xB9] = ReadTwoBytesBody,
                [0xBA] = ReadTwoBytesBody,
                [0xBB] = ReadTwoBytesBody,

                [0xC0] = ReadThreeBytesBody,
                [0xC1] = ReadThreeBytesBody,
                [0xC2] = ReadThreeBytesBody,
                [0xC3] = ReadThreeBytesBody,
                [0xC4] = ReadThreeBytesBody,

                [0xD0] = ReadThreeBytesBody,
                [0xD1] = ReadThreeBytesBody,
                [0xD2] = ReadThreeBytesBody,
                [0xD3] = ReadThreeBytesBody,
                [0xD4] = ReadThreeBytesBody,

                [0xE0] = ReadFourBytesBody
            };
        }

        private static StubCommand ReadNBytesBody(uint n, byte code, ISequentialReader reader) =>
            n == 0 ? new StubCommand(code, new byte[0]) : new StubCommand(code, reader.ReadBytes(n));

        private static StubCommand ReadZeroBytesBody(byte c, ISequentialReader r) => ReadNBytesBody(0, c, r);
        private static StubCommand ReadOneByteBody(byte c, ISequentialReader r) => ReadNBytesBody(1, c, r);
        private static StubCommand ReadTwoBytesBody(byte c, ISequentialReader r) => ReadNBytesBody(2, c, r);
        private static StubCommand ReadThreeBytesBody(byte c, ISequentialReader r) => ReadNBytesBody(3, c, r);
        private static StubCommand ReadFourBytesBody(byte c, ISequentialReader r) => ReadNBytesBody(4, c, r);
        private static StubCommand ReadFiveBytesBody(byte c, ISequentialReader r) => ReadNBytesBody(5, c, r);
        private static StubCommand ReadTenBytesBody(byte c, ISequentialReader r) => ReadNBytesBody(10, c, r);
        private static StubCommand ReadElevenBytesBody(byte c, ISequentialReader r) => ReadNBytesBody(11, c, r);

        private static PsgWriteCommand ReadPsgWriteCommandBody(byte c, ISequentialReader r) => new PsgWriteCommand(c, r.ReadByte());
        private static WaitNSamplesCommand ReadWaitNSamplesBody(byte c, ISequentialReader r) => new WaitNSamplesCommand(c, r.ReadUInt16());
        private static Wait735SamplesCommand ReadWait735SamplesBody(byte c, ISequentialReader r) => new Wait735SamplesCommand(c);
        private static EndOfSoundDataCommand ReadEndOfSoundDataBody(byte c, ISequentialReader r) => new EndOfSoundDataCommand(c);
        private static WaitNPlusOneSamplesCommand ReadWaitNPlusOneSamplesBody(byte c, ISequentialReader r) => new WaitNPlusOneSamplesCommand(c);

        private static YM2612Port0Address2AWriteThenWaitNSamplesCommand ReadYM2612WriteAndWaitBody(byte c,
            ISequentialReader r) => new YM2612Port0Address2AWriteThenWaitNSamplesCommand(c);

        private static DataBlockCommand ReadDataBlockBody(byte c, ISequentialReader r)
        {
            var compatibility = r.ReadByte();

            if (compatibility != EndOfSoundDataCode)
                throw new VgmException($"Data block command code 0x{c:X2} must be folowed by 0x{EndOfSoundDataCode:X2}");

            var type = r.ReadByte();
            var size = r.ReadUInt32();

            r.Skip(size);

            return new DataBlockCommand(c, type, size);
        }

        public static VgmCommand ReadOne(ISequentialReader reader)
        {
            var code = reader.ReadByte();
            if (Map.TryGetValue(code, out var commandReader))
            {
                return commandReader(code, reader);
            }
            else
            {
                throw new VgmException("Unknown command");
            }
        }

        public static IEnumerable<VgmCommand> Read(VgmHeader header, ISequentialReader reader)
        {
            var vgmDataOffset = GetVGMDataAbsoluteOffset(header);
            reader.SkipToAbsoluteOffset(vgmDataOffset);
            return ReadSequence(reader);
        }

        private static uint GetVGMDataAbsoluteOffset(VgmHeader header)
        {
            const uint vgmDataOffsetPriorToV150 = 0x40;

            if (header.V150.VGMDataOffset == 0)
                return vgmDataOffsetPriorToV150;
            else
            {
                var relativeOffset = header.V150.VGMDataOffset;
                return relativeOffset + V150HeaderPart.VGMDataOffsetOffset;
            }
        }

        private static IEnumerable<VgmCommand> ReadSequence(ISequentialReader reader)
        {
            VgmCommand command;
            do
            {
                command = ReadOne(reader);
                yield return command;
            } while (!(command is EndOfSoundDataCommand));
        }
    }
}
