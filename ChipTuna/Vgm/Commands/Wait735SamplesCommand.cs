namespace ChipTuna.Vgm.Commands
{
    public class Wait735SamplesCommand: WaitNSamplesCommand
    {
        public Wait735SamplesCommand(byte code)
            : base(code, 735)
        {
        }
    }
}