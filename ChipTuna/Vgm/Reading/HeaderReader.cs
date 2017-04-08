using ChipTuna.IO;
using ChipTuna.Vgm.Headers;

namespace ChipTuna.Vgm.Reading
{
    public static class HeaderReader
    {
        private static void ReadCommon(ISequentialReader reader, ref CommonHeaderPart part)
        {
            part.VgmIdentification = reader.ReadUInt32();
            part.EofOffset = reader.ReadUInt32();
            part.Version = (FormatVersion)reader.ReadUInt32();
        }

        private static void Read100(ISequentialReader reader, ref V100HeaderPart part)
        {
            part.SN76489Clock = reader.ReadUInt32();
            part.YM2413Clock = reader.ReadUInt32();
            part.GD3Offset = reader.ReadUInt32();
            part.TotalNumberOfSamples = reader.ReadUInt32();
            part.LoopOffset = reader.ReadUInt32();
            part.LoopNumberOfSamples = reader.ReadUInt32();
        }

        private static void Read101(ISequentialReader reader, ref V101HeaderPart part)
        {
            part.Rate = reader.ReadUInt32();
        }

        private static void Read110Part1(ISequentialReader reader, ref V110HeaderPart part)
        {
            part.SN76489Feedback = reader.ReadUInt16();
            part.SN76489ShiftRegisterWidth = reader.ReadByte();
        }

        private static void Read151Part1(ISequentialReader reader, ref V151HeaderPart part)
        {
            part.SN76489Flags = reader.ReadByte();
        }

        private static void Read110Part2(ISequentialReader reader, ref V110HeaderPart part)
        {
            part.YM2612Clock = reader.ReadUInt32();
            part.YM2151Clock = reader.ReadUInt32();
        }

        private static void Read150(ISequentialReader reader, ref V150HeaderPart part)
        {
            part.VGMDataOffset = reader.ReadUInt32();
        }

        public static VgmHeader Read(ISequentialReader reader)
        {
            var header = new VgmHeader();
            var unused = new V151HeaderPart();

            ReadCommon(reader, ref header.Common);

            if (header.Common.VgmIdentification != CommonHeaderPart.VgmIdentificationValue)
            {
                throw new VgmException("Vgm magic number does not found");
            }

            var version = header.Common.Version;
            if (version != FormatVersion.V100 &&
                version != FormatVersion.V101 &&
                version != FormatVersion.V110 &&
                version != FormatVersion.V150)
            {
                throw new VgmException("Unsupported version");
            }

            Read100(reader, ref header.V100);

            if (version == FormatVersion.V100)
                return header;

            Read101(reader, ref header.V101);

            if (version == FormatVersion.V101)
                return header;

            Read110Part1(reader, ref header.V110);
            Read151Part1(reader, ref unused);
            Read110Part2(reader, ref header.V110);

            if (version == FormatVersion.V110)
                return header;

            Read150(reader, ref header.V150);
            return header;
        }
    }
}
