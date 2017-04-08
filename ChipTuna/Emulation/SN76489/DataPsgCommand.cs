namespace ChipTuna.Emulation.SN76489
{
    public class DataPsgCommand: PsgCommand
    {
        public DataPsgCommand(int data)
        {
            Data = data;
        }

        public int Data { get; }
    }
}