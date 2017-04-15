using System;

namespace ChipTuna.Sound
{
    public class SquareOscillator
    {
        private readonly double _timeStep;

        private double _phase = 0;
        private bool _value;

        public SquareOscillator(uint sampleRate)
        {
            _timeStep = 1.0 / sampleRate;
        }

        public float Amplitude { get; set; } = 0.5f;

        public float Frequency { get; set; } = 440;

        public float Step()
        {
            double deltaPhase = 2 * Math.PI * Frequency * _timeStep;
            _phase += deltaPhase;
            if (_phase > 2 * Math.PI)
                _phase -= 2 * Math.PI;

            var value = Math.Sin(_phase);
            var sign = Math.Sign(value);
            _value = sign > 0;

            return GetValue();
        }

        private float GetValue()
        {
            return _value ? Amplitude : -Amplitude;
        }
    }
}
