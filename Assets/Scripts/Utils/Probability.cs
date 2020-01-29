using System;

namespace Utils
{
    /// <summary>
    /// Utility class capable of dealing with small probabilities.
    /// </summary>
    public class Probability
    {
        private Random _random;

        public Probability() => _random = new Random();

        /// <summary>
        /// Returns true with a certain probability.
        /// </summary>
        /// <param name="probability">The probability between 0 and 1 to return true</param>
        /// <returns></returns>
        public bool GetProbability(float probability)
        {
            if (probability < 0 || probability > 1f)
                throw new Exception("Wrong usage of Probability");
            if (Math.Abs(probability) < 0.00001)
                return false;
            if (Math.Abs(probability - 1) < 0.00001)
                return true;

            float percentage = probability * 100;
            int temp = _random.Next(100);
            if (percentage >= 1)
            {
                return temp < percentage;
            }
            else // < 1% -----> new min: 0.01%
            {
                if (temp == 0) // hit the one percent
                {
                    // 1 old percent = 100 new percent
                    return GetProbability(percentage);
                }

                return false;
            }
        }
    }
}