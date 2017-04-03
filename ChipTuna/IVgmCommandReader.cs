using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTuna
{
    public interface IVgmCommandReader
    {
        VgmCommand Read(byte code, ISequentialReader reader);
    }
}
