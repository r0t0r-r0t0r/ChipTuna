using System;
using System.Collections.Generic;

namespace ChipTuna
{
    public class VgmCommandReader : IVgmCommandReader
    {
        private readonly Dictionary<byte, IVgmCommandReader> _map;

        public VgmCommandReader()
        {
            var zeroParamsReader = new GenericVgmCommandReader(0);
            var oneParamReader = new GenericVgmCommandReader(1);
            var twoParamsReader = new GenericVgmCommandReader(2);
            var threeParamsReader = new GenericVgmCommandReader(3);
            var fourParamsReader = new GenericVgmCommandReader(4);

            _map = new Dictionary<byte, IVgmCommandReader>
            {
                [0x4F] = oneParamReader,
                [0x50] = new FuncVgmCommandReader((c, r) => new PsgWriteVgmCommand(c, r.ReadByte())),
                [0x51] = twoParamsReader,
                [0x52] = twoParamsReader,
                [0x53] = twoParamsReader,
                [0x54] = twoParamsReader,
                [0x55] = twoParamsReader,
                [0x56] = twoParamsReader,
                [0x57] = twoParamsReader,
                [0x58] = twoParamsReader,
                [0x59] = twoParamsReader,
                [0x5A] = twoParamsReader,
                [0x5B] = twoParamsReader,
                [0x5C] = twoParamsReader,
                [0x5D] = twoParamsReader,
                [0x5E] = twoParamsReader,
                [0x5F] = twoParamsReader,
                [0x61] = new FuncVgmCommandReader((c, r) => new WaitNSamplesVgmCommand(c, r.ReadUInt16())),
                [0x62] = new FuncVgmCommandReader((c, r) => new Wait735SamplesVgmCommand(c)),
                [0x63] = zeroParamsReader,
                [0x64] = threeParamsReader,
                [0x66] = new FuncVgmCommandReader((c, r) => new EndOfSoundDataVgmCommand(c))
            };
        }

        public VgmCommand Read(byte code, ISequentialReader reader)
        {
            if (_map.TryGetValue(code, out var commandReader))
            {
                return commandReader.Read(code, reader);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(code));
            }
        }
    }
}
