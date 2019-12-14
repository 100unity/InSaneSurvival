using System;

namespace Utils
{
    public class Probability
    {
        private Random _random;

        public Probability()
        {
            _random = new Random();
        }

        /// <summary>
        /// Returns true with a certain probability.
        /// </summary>
        /// <param name="probability">The probability to return true</param>
        /// <returns></returns>
        public bool GetProbability(float probability)
        {
            if (probability < 0 || probability > 1f)
                throw new Exception("Wrong usage of Probability");
            if (probability == 0)
                return false;
            if (probability == 1)
                return true;

            float percentage = probability * 100;
            int temp = _random.Next(100);
            if (percentage >= 1)
            {
                return temp < percentage;
            }
            else // < 1% -----> min: 0.01%
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