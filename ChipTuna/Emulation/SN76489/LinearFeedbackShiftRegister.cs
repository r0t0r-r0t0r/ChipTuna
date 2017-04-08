namespace ChipTuna.Emulation.SN76489
{
    public class LinearFeedbackShiftRegister
    {
        private const ushort InitialState = 0b1000_0000_0000_0000;

        private ushort _state = InitialState;
        private LfsrMode _mode = LfsrMode.Noise;

        public void Reset(LfsrMode mode)
        {
            _state = InitialState;
            _mode = mode;
        }

        public bool Next()
        {
            var output = _state & 0b0000_0001;
            //var tappedBit1 = IntToBool(_state & 0b0000_0001);
            //var tappedBit2 = IntToBool(_state & 0b0000_1000);
            //var input = BoolToInt(tappedBit1 ^ tappedBit2) << 15;
            var input = BoolToInt(GetInput()) << 15;
            var newState = (_state >> 1) | input;
            _state = (ushort)newState;

            return IntToBool(output);
        }

        private  bool GetInput()
        {
            if (_mode == LfsrMode.Noise)
            {
                var tappedBit1 = IntToBool(_state & 0b0000_0001);
                var tappedBit2 = IntToBool(_state & 0b0000_1000);
                return tappedBit1 ^ tappedBit2;
            }
            else
            {
                return IntToBool(_state & 0b0000_0001);
            }
        }

        private static bool IntToBool(int value)
        {
            return value != 0;
        }

        private static int BoolToInt(bool value)
        {
            return value ? 1 : 0;
        }
    }

    public enum LfsrMode
    {
        Noise,
        Periodic
    }
}
