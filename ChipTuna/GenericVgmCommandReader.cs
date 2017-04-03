namespace ChipTuna
{
    public class GenericVgmCommandReader : IVgmCommandReader
    {
        private readonly uint _paramsSize;

        public GenericVgmCommandReader(uint paramsSize)
        {
            _paramsSize = paramsSize;
        }

        public VgmCommand Read(byte code, ISequentialReader reader)
        {
            if (_paramsSize == 0)
                return new GenericVgmCommand(code, new byte[0]);
            else
                return new GenericVgmCommand(code, reader.ReadBytes(_paramsSize));
        }
    }
}
