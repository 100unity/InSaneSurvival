using Entity.Player.Sanity;
using System;
using UnityEngine;

namespace Utils
{
    [Serializable]
    public class Stat
    {
        [Tooltip("The type of the stat")]
        [SerializeField]
        public SanityController.StatType Type;

        [Tooltip("The stat boundary for sanity impact. At this value, the stat doesn't influence sanity (neither positively nor negatively)")]
        [SerializeField]
        public int Boundary;

        [Tooltip("The severity at which the stat influences sanity")]
        [SerializeField]
        public float Severity;

        // the rate at which the stat influences sanity
        [HideInInspector]
        public float Rate;
    }

}
