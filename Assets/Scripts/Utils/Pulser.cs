using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class Pulser
    {
        // have different frequencies for different effects
        private Dictionary<string, bool> _pulses;

        public Pulser()
        {
            _pulses = new Dictionary<string, bool>();
        }

        /// <summary>
        /// Make a new pulse.
        /// </summary>
        /// <param name="name"></param>
        public void NewPulse(string name)
        {
            _pulses.Add(name, false);
        }

        /// <summary>
        /// Pulse a value with a certain frequency and intensity. If the pulse doesn't already exist, a new pulse is made.
        /// </summary>
        /// <param name="name">The name of the pulse</param>
        /// <param name="valueToBePulsed">The value pulsing should be applied to.</param>
        /// <param name="pulseFrequency">The frequency of the pulse.</param>
        /// <param name="pulseIntensity">The intensity of the pulse.</param>
        /// <returns>The pulsed value.</returns>
        public float Pulse(string name, float valueToBePulsed, float pulseFrequency, float pulseIntensity)
        {
            if (!_pulses.ContainsKey(name))
                NewPulse(name);

            if (_pulses[name])
            {
                valueToBePulsed += Time.deltaTime * pulseFrequency;
                if (valueToBePulsed > pulseIntensity)
                    _pulses[name] = false;
            }
            else
            {
                valueToBePulsed -= Time.deltaTime * (pulseFrequency / 2);
                if (valueToBePulsed <= 0)
                    _pulses[name] = true;
            }
            return valueToBePulsed;
        }
    }
}
