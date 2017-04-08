using System;
using ChipTuna.Vgm.Headers;

namespace ChipTuna.Vgm.VersionAbstractionLayer
{
    public static class VgmHeaderUtils
    {
        public static FormatVersion GetFormatVersion(this VgmHeader header) => header.Common.Version;
        public static long GetSamplesCount(this VgmHeader header) => header.V100.TotalNumberOfSamples;

        public static DevicePresence GetDevicePresence(this VgmHeader header, DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.SN76489:
                    return header.V100.SN76489Clock == 0 ? DevicePresence.Absent : DevicePresence.Present;
                case DeviceType.YM2612:
                    if (header.Common.Version <= FormatVersion.V101)
                    {
                        return header.V100.YM2413Clock == 0 ? DevicePresence.Absent : DevicePresence.ProbablyPresent;
                    }
                    else
                    {
                        return header.V110.YM2612Clock == 0 ? DevicePresence.Absent : DevicePresence.Present;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(deviceType), deviceType, null);
            }
        }

        public static SN76489Settings GetSN76489Settings(this VgmHeader header)
        {
            if (GetDevicePresence(header, DeviceType.SN76489) == DevicePresence.Absent)
            {
                throw new InvalidOperationException();
            }

            var clock = header.V100.SN76489Clock;
            var feedbackMask = header.Common.Version <= FormatVersion.V101
                ? 0x0009
                : header.V110.SN76489Feedback;

            var shiftRegisterWidth = header.Common.Version <= FormatVersion.V101
                ? 16
                : header.V110.SN76489ShiftRegisterWidth;

            return new SN76489Settings
            {
                Clock = (int) clock,
                FeedbackMask = feedbackMask,
                ShiftRegisterWidth = shiftRegisterWidth
            };
        }

        public static YM2612Settings GetYM2612Settings(this VgmHeader header)
        {
            if (GetDevicePresence(header, DeviceType.YM2612) == DevicePresence.Absent)
            {
                throw new InvalidOperationException();
            }

            var clock = header.Common.Version <= FormatVersion.V101 ?
                header.V100.YM2413Clock : header.V110.YM2612Clock;

            return new YM2612Settings
            {
                Clock = (int) clock
            };
        }
    }
}
