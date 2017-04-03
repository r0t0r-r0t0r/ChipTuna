namespace ChipTuna
{
    public static class VgmHeaderReader
    {
        public static void ReadCommon(ISequentialReader reader, ref VgmHeaderCommon header)
        {
            header.VgmIdentification = reader.ReadUInt32();
            header.EofOffset = reader.ReadUInt32();
            header.Version = reader.ReadUInt32();
        }

        public static void Read100(ISequentialReader reader, ref VgmHeader100 header)
        {
            header.SN76489Clock = reader.ReadUInt32();
            header.YM2413Clock = reader.ReadUInt32();
            header.GD3Offset = reader.ReadUInt32();
            header.TotalNumberOfSamples = reader.ReadUInt32();
            header.LoopOffset = reader.ReadUInt32();
            header.LoopNumberOfSamples = reader.ReadUInt32();
        }

        public static void Read101(ISequentialReader reader, ref VgmHeader101 header)
        {
            header.Rate = reader.ReadUInt32();
        }

        public static void Read110Part1(ISequentialReader reader, ref VgmHeader110 header)
        {
            header.SN76489Feedback = reader.ReadUInt16();
            header.SN76489ShiftRegisterWidth = reader.ReadByte();
        }

        public static void Read151Part1(ISequentialReader reader, ref VgmHeader151 header)
        {
            header.SN76489Flags = reader.ReadByte();
        }

        public static void Read110Part2(ISequentialReader reader, ref VgmHeader110 header)
        {
            header.YM2612Clock = reader.ReadUInt32();
            header.YM2151Clock = reader.ReadUInt32();
        }

        public static void Read150(ISequentialReader reader, ref VgmHeader150 header)
        {
            header.VGMDataOffset = reader.ReadUInt32();
        }
    }
}
