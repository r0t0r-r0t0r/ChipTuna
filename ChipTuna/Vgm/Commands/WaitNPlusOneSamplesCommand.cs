using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTuna.Vgm.Commands
{
    public class WaitNPlusOneSamplesCommand: WaitNSamplesCommand
    {
        public WaitNPlusOneSamplesCommand(byte code) : base(code, (ushort) ((code & 0x0f) + 1))
        {
        }


    }
}
