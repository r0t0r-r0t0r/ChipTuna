using System.IO;

namespace ChipTuna.IO
{
    public class StreamSequentialReader: ISequentialReader
    {
        private readonly BinaryReader _reader;

        public StreamSequentialReader(BinaryReader reader)
        {
            _reader = reader;
        }

        public byte ReadByte()
        {
            BytesRead += 1;
            return _reader.ReadByte();
        }

        public byte[] ReadBytes(uint count)
        {
            BytesRead += count;
            return _reader.ReadBytes((int)count);
        }

        public ushort ReadUInt16()
        {
            BytesRead += 2;
            return _reader.ReadUInt16();
        }

        public uint ReadUInt32()
        {
            BytesRead += 4;
            return _reader.ReadUInt32();
        }

        public void Skip(uint bytesNumber)
        {
            for (uint i = 0; i < bytesNumber; i++)
            {
                ReadByte();
            }
        }

        public uint BytesRead { get; private set; } = 0;
    }
}
