using System;
using UnityEngine;
using static Utils.Enums;

namespace Utils
{
    [Serializable]
    public class Stat
    {
        [Tooltip("The type of the stat")]
        [SerializeField]
        public StatType Type;

        [Tooltip("The stat boundary for sanity impact. At this value, the stat doesn't influence sanity (neither positively nor negatively)")]
        [SerializeField]
        public int Boundary;

        [Tooltip("The severity at which the stat influences sanity")]
        [SerializeField]
        public float Severity;

        // the rate at which the stat influences sanity
        public float Rate;
    }

}
