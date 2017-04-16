using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChipTuna.Emulation.SN76489;
using ChipTuna.Vgm.Commands;

namespace ChipTuna.Rendering
{
    public static class Renderer
    {
        public static IEnumerable<float> Render(IEnumerable<VgmCommand> commands)
        {
            var psg = new PsgOscillator();

            foreach (var command in commands)
            {
                switch (command)
                {
                    case PsgWriteCommand wc:
                        psg.ApplyCommand(wc.Data);
                        break;
                    case WaitNSamplesCommand wnc:
                        for (uint i = 0; i < wnc.SamplesNumber; i++)
                        {
                            yield return psg.Step();
                        }
                        break;
                    case YM2612Port0Address2AWriteThenWaitNSamplesCommand wwc:
                        for (uint i = 0; i < wwc.SamplesNumber; i++)
                        {
                            yield return psg.Step();
                        }
                        break;
                }
            }
        }
    }
}
