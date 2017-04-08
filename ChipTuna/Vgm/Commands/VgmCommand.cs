namespace ChipTuna.Vgm.Commands
{
    public abstract class VgmCommand
    {
        protected VgmCommand(byte code)
        {
            Code = code;
        }
        public byte Code { get; }
    }
}
