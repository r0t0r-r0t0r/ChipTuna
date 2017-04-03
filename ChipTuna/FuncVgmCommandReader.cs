using System;

namespace ChipTuna
{
    public class FuncVgmCommandReader : IVgmCommandReader
    {
        private readonly Func<byte, ISequentialReader, VgmCommand> _func;

        public FuncVgmCommandReader(Func<byte, ISequentialReader, VgmCommand> func)
        {
            _func = func;
        }

        public VgmCommand Read(byte code, ISequentialReader reader)
        {
            return _func(code, reader);
        }
    }
}
