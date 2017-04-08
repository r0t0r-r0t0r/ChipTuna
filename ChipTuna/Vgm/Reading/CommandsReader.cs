using System.Collections.Generic;
using ChipTuna.IO;
using ChipTuna.Vgm.Commands;
using ChipTuna.Vgm.Headers;

namespace ChipTuna.Vgm.Reading
{
    public static class CommandsReader
    {
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
                [0x66] = ReadEndOfSoundDataBody
            };
        }

        private static StubCommand ReadNBytesBody(uint n, byte code, ISequentialReader reader) =>
            n == 0 ? new StubCommand(code, new byte[0]) : new StubCommand(code, reader.ReadBytes(n));

        private static StubCommand ReadZeroBytesBody(byte c, ISequentialReader r) => ReadNBytesBody(0, c, r);
        private static StubCommand ReadOneByteBody(byte c, ISequentialReader r) => ReadNBytesBody(1, c, r);
        private static StubCommand ReadTwoBytesBody(byte c, ISequentialReader r) => ReadNBytesBody(2, c, r);
        private static StubCommand ReadThreeBytesBody(byte c, ISequentialReader r) => ReadNBytesBody(3, c, r);
        private static StubCommand ReadFourBytesBody(byte c, ISequentialReader r) => ReadNBytesBody(4, c, r);

        private static PsgWriteCommand ReadPsgWriteCommandBody(byte c, ISequentialReader r) => new PsgWriteCommand(c, r.ReadByte());
        private static WaitNSamplesCommand ReadWaitNSamplesBody(byte c, ISequentialReader r) => new WaitNSamplesCommand(c, r.ReadUInt16());
        private static Wait735SamplesCommand ReadWait735SamplesBody(byte c, ISequentialReader r) => new Wait735SamplesCommand(c);
        private static EndOfSoundDataCommand ReadEndOfSoundDataBody(byte c, ISequentialReader r) => new EndOfSoundDataCommand(c);

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
