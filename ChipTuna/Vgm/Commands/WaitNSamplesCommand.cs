namespace ChipTuna.Vgm.Commands
{
    public class WaitNSamplesCommand: VgmCommand
    {
        public WaitNSamplesCommand(byte code, ushort samplesNumber)
            : base(code)
        {
            SamplesNumber = samplesNumber;
        }

        public ushort SamplesNumber { get; }
    }
}