using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipTuna.Sound
{
    public class SinOscillator
    {
        private readonly double _timeStep;

        private double _phase = 0;

        public SinOscillator(uint sampleRate)
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

            return (float) (Amplitude * value);
        }
    }
}
