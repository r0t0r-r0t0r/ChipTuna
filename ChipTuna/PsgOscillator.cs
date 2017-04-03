using System;

namespace ChipTuna
{
    public class PsgOscillator
    {
        private const uint SampleRate = 44100;

        private readonly SquareOscillator[] _oscillators = new SquareOscillator[]
        {
            new SquareOscillator(SampleRate),
            new SquareOscillator(SampleRate),
            new SquareOscillator(SampleRate),
            new SquareOscillator(SampleRate)
        };

        private readonly int[] _tones = new int[4];
        private readonly int[] _volumes = new int[4];
        private readonly LinearFeedbackShiftRegister _lfsr = new LinearFeedbackShiftRegister();

        private PsgLatchType _latchType = PsgLatchType.ToneOrNoise;
        private PsgChannel _latchChannel = PsgChannel.Zero;
        private int _prevNoiseSign = 1;
        private bool _lastLfsrValue = false;

        public void ApplyCommand(byte commandCode)
        {
            var command = PsgCommandUtils.Create(commandCode);

            switch (command)
            {
                case LatchPsgCommand lc:
                    _latchType = lc.Type;
                    _latchChannel = lc.Channel;
                    WriteFourLowBits(lc.FourLowBits);
                    UpdateLatchFrequencyAndVolume();
                    break;
                case DataPsgCommand dc:
                    WriteHighBits(dc.Data);
                    UpdateLatchFrequencyAndVolume();
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void WriteFourLowBits(int data)
        {
            var ch = (int)_latchChannel;

            if (IsLatchToneChannel())
            {
                if (_latchType == PsgLatchType.ToneOrNoise)
                {
                    ref int tone = ref _tones[ch];
                    tone &= 0b0000_0011_1111_0000;
                    tone |= data;
                }
                else
                {
                    _volumes[ch] = data & 0b1111;
                }
            }
            else
            {
                if (_latchType == PsgLatchType.ToneOrNoise)
                {
                    ref int tone = ref _tones[ch];
                    switch (data & 0b0011)
                    {
                        case 0:
                            tone = 0x10;
                            break;
                        case 1:
                            tone = 0x20;
                            break;
                        case 2:
                            tone = 0x40;
                            break;
                        case 3:
                            tone = _tones[(int)PsgChannel.Two];
                            break;
                    }

                    if ((data & 0b0100) != 0)
                    {
                        _lfsr.Reset(LfsrMode.Noise);
                    }
                    else
                    {
                        _lfsr.Reset(LfsrMode.Periodic);
                    }
                }
                else
                {
                    _volumes[ch] = data & 0b1111;
                }
            }
        }

        private void WriteHighBits(int data)
        {
            var ch = (int)_latchChannel;

            if (IsLatchToneChannel())
            {
                if (_latchType == PsgLatchType.ToneOrNoise)
                {
                    ref int tone = ref _tones[ch];
                    tone &= 0b0000_0000_0000_1111;
                    tone |= data << 4;
                }
                else
                {
                    _volumes[ch] = data & 0b1111;
                }
            }
            else
            {
                if (_latchType == PsgLatchType.ToneOrNoise)
                {
                    ref int tone = ref _tones[ch];
                    switch (data & 0b0011)
                    {
                        case 0:
                            tone = 0x10;
                            break;
                        case 1:
                            tone = 0x20;
                            break;
                        case 2:
                            tone = 0x40;
                            break;
                        case 3:
                            tone = _tones[(int)PsgChannel.Two];
                            break;
                    }
                    if ((data & 0b0100) != 0)
                    {
                        _lfsr.Reset(LfsrMode.Noise);
                    }
                    else
                    {
                        _lfsr.Reset(LfsrMode.Periodic);
                    }
                }
                else
                {
                    _volumes[ch] = data & 0b1111;
                }
            }
        }

        private bool IsLatchToneChannel() =>
            _latchChannel == PsgChannel.Zero || _latchChannel == PsgChannel.One || _latchChannel == PsgChannel.Two;

        private int[] _volumeTable =
        {
            32767, 26028, 20675, 16422, 13045, 10362,  8231,  6568,
            5193,  4125,  3277,  2603,  2067,  1642,  1304,     0
        };

        private void UpdateLatchFrequencyAndVolume()
        {
            var ch = (int)_latchChannel;

            const int clock = 3579545;
            const int divider = 16;
            var _halfWaveCounter = _tones[ch];

            var frequency = 0f;

            if (_halfWaveCounter != 0)
                frequency = (float)((double)clock / (2 * divider * _halfWaveCounter));

            var volume = _volumeTable[_volumes[ch]] / 32767f;

            _oscillators[ch].Amplitude = volume;

            if (IsLatchToneChannel())
            {
                _oscillators[ch].Frequency = frequency;
            }
            else
            {
                _oscillators[ch].Frequency = frequency / 2;
            }

        }

        public float Step()
        {
            var result = 0f;

            for (var i = 0; i < _oscillators.Length; i++)
            {
                var osc = _oscillators[i];

                float oscVal;
                if (i <= 2)
                {
                    oscVal = osc.Step();
                }
                else
                {
                    var currNoiseSign = Math.Sign(osc.Step());
                    if (currNoiseSign != _prevNoiseSign)
                    {
                        _lastLfsrValue = _lfsr.Next();
                    }
                    _prevNoiseSign = currNoiseSign;

                    oscVal = _lastLfsrValue ? osc.Amplitude : -osc.Amplitude;
                }
                result += oscVal;
            }
            return result;
        }
    }
}
