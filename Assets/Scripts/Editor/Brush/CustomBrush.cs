using UnityEngine;
using Utils;

namespace Editor.Brush
{
    public class CustomBrush : MonoBehaviour
    {
        [Tooltip("Prefabs to be spawned randomly.")] [SerializeField]
        private GameObject[] prefabs;

        [Tooltip("The parents' names (numbers are appended automatically).")] [SerializeField]
        private string parentNaming;

        [Tooltip(
            "Max subtraction and max addition from/to the base scale of X and Z axis. (Terms min and max are slightly confusing)")]
        [SerializeField]
        private Range scaleXZ;

        [Tooltip(
            "Max subtraction and max addition from/to the base scale of Y axis. (Terms min and max are slightly confusing)")]
        [SerializeField]
        private Range scaleY;

        [Tooltip("Scale Y only.")] [SerializeField]
        private bool justScaleY;

        [Tooltip("Placed objects are trees.")] [SerializeField]
        private bool isTree;

        public GameObject[] Prefabs => prefabs;
        public Range ScaleXZ => scaleXZ;
        public Range ScaleY => scaleY;
        public string ParentNaming => parentNaming;
        public bool JustScaleY => justScaleY;
        public bool IsTree => isTree;
    }
}