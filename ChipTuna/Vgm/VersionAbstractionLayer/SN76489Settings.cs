namespace ChipTuna.Vgm.VersionAbstractionLayer
{
    public class SN76489Settings
    {
        public int Clock;
        public int FeedbackMask;
        public int ShiftRegisterWidth;

        //TODO: this flags are used in 1.51 format version but now we process only 1.50 headers
//        public bool Frequency0Is0x400;
//        public bool NegateOutput;
//        public bool Stereo;
//        public bool EightClockDivider;
    }
}
