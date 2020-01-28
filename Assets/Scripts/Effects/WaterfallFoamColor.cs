using GameTime;
using UnityEngine;

namespace Effects
{
    public class WaterfallFoamColor : MonoBehaviour
    {
        [SerializeField][Tooltip("Material of the Foam")]
        private Material foam;

        [SerializeField] [Tooltip("The gradient that adjusts the material color")]
        private Gradient foamColor;

        [SerializeField][Tooltip("Clock where the time of day comes from")]
        private Clock clock;

        // Update is called once per frame
        void Update()
        {
            CalculateFoamColor();
        }

        private void CalculateFoamColor() => foam.color = foamColor.Evaluate(clock.TimeOfDay);

    }
}
