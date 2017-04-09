using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTuna.Vgm.Commands
{
    public class DataBlockCommand: VgmCommand
    {
        public DataBlockCommand(byte code, byte type, uint size) : base(code)
        {
            Type = type;
            Size = size;
        }

        public byte Type { get; }
        public uint Size { get; }
    }
}
