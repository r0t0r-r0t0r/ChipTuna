namespace ChipTuna.Vgm.Commands
{
    public class Wait882SamplesCommand: WaitNSamplesCommand
    {
        public Wait882SamplesCommand(byte code)
            : base(code, 882)
        {
        }
    }
}