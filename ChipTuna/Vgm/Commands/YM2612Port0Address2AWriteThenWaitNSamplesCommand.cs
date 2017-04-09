using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTuna.Vgm.Commands
{
    public class YM2612Port0Address2AWriteThenWaitNSamplesCommand: VgmCommand
    {
        public YM2612Port0Address2AWriteThenWaitNSamplesCommand(byte code) : base(code)
        {
            SamplesNumber = (ushort) (code & 0x0f);
        }

        public ushort SamplesNumber { get; }
    }
}
