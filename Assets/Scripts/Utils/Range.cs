using System;

namespace Utils
{
    /// <summary>
    /// Little struct for defining a float Range.
    /// </summary>
    [Serializable]
    public struct Range
    {
        public float min;
        public float max;

        public override string ToString() => $"Min: {min}; Max: {max}";
    }
}