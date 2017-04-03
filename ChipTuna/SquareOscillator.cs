using System;

namespace ChipTuna
{
    public class SquareOscillator
    {
        private readonly double _timeStep;
        private float _amplitude = 0.5f;
        private float _frequency = 440;

        private double _time = 0;
        private bool _value;

        public SquareOscillator(uint sampleRate)
        {
            _timeStep = 1.0 / sampleRate;
        }

        public float Amplitude
        {
            get => _amplitude;
            set
            {
                _amplitude = value;
            }
        }

        public float Frequency
        {
            get => _frequency;
            set
            {
                _frequency = value;
            }
        }

        public float Step()
        {
            _time += _timeStep;
            var value = Math.Sin(2 * Math.PI * _frequency * _time);
            var sign = Math.Sign(value);
            _value = sign > 0;

            return GetValue();
        }

        private float GetValue()
        {
            return _value ? _amplitude : -_amplitude;
        }
    }
}
