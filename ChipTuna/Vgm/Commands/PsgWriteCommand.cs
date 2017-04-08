namespace ChipTuna.Vgm.Commands
{
    public class PsgWriteCommand: VgmCommand
    {
        public PsgWriteCommand(byte code, byte data)
            : base(code)
        {
            Data = data;
        }

        public byte Data { get; }
    }
}