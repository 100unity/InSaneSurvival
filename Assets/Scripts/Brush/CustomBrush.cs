using UnityEngine;

namespace Brush
{
    public class CustomBrush : MonoBehaviour
    {
        [Tooltip("Prefabs to be spawned randomly.")]
        [SerializeField]
        private GameObject[] prefabs;

        [Tooltip("Maximum subtraction from base scale.")]
        [SerializeField]
        private float minScale;

        [Tooltip("Maximum addition to base scale.")]
        [SerializeField]
        private float maxScale;

        [Tooltip("The parents' names (numbers are appended automatically)")]
        [SerializeField]
        private string parentNaming;

        [Tooltip("Scale y only.")]
        [SerializeField]
        private bool justScaleY;

        public GameObject[] Prefabs => prefabs;
        public float MinScale => minScale;
        public float MaxScale => maxScale;
        public string ParentNaming => parentNaming;
        public bool JustScaleY => justScaleY;
    }
}
