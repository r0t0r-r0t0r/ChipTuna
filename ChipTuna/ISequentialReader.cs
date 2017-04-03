namespace ChipTuna
{
    public interface ISequentialReader
    {
        uint ReadUInt32();
        ushort ReadUInt16();
        byte ReadByte();
        byte[] ReadBytes(uint count);
        void Skip(uint bytesNumber);
        uint BytesRead { get; }
    }
}
