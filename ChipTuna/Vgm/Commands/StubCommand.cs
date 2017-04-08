namespace ChipTuna.Vgm.Commands
{
    public class StubCommand: VgmCommand
    {
        public StubCommand(byte code, byte[] data)
            : base(code)
        {
            Data = data;
        }

        public byte[] Data { get; }
    }
}