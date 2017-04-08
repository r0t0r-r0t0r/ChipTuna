using ChipTuna.IO;
using ChipTuna.Vgm.Commands;

namespace ChipTuna.Vgm.Reading
{
    internal delegate VgmCommand CommandBodyReader(byte code, ISequentialReader reader);
}