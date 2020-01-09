using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.ElementInteraction;

namespace Utils
{
    public static class ClassExtender
    {
        /// <summary>
        /// Checks if the layer masks has the given layer checked
        /// </summary>
        /// <param name="layerMask">The layer mask that defines possible layers</param>
        /// <param name="layer">The layer to be checked</param>
        /// <returns>Whether the layer mask has the layer checked</returns>
        public static bool Contains(this LayerMask layerMask, int layer) => layerMask == (layerMask | (1 << layer));

        /// <summary>
        /// RayCast on UI-Level.
        /// <para>Finds the next element (of T) using the provided GraphicRaycaster and PointerEventData (position of it)</para>
        /// </summary>
        /// <param name="graphicRaycaster">The GraphicRaycaster used for casting the ray</param>
        /// <param name="eventData">The EventData for the raycast e.g. mouse click</param>
        /// <param name="elementToIgnore">An element to be ignored, can be null</param>
        /// <typeparam name="T">The type to look for</typeparam>
        /// <returns></returns>
        public static T FindUIElement<T>(this GraphicRaycaster graphicRaycaster, PointerEventData eventData,
            T elementToIgnore = null) where T : class
        {
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(eventData, results);
            foreach (RaycastResult result in results)
            {
                if (!result.gameObject.TryGetComponent(out T element))
                    continue;
                // Ignore elementToIgnore
                if (elementToIgnore == element)
                    continue;
                return element;
            }

            return null;
        }

        /// <summary>
        /// Checks if an object is visible from camera's perspective.
        /// </summary>
        /// <param name="renderer">The renderer of the gameobject to be checked</param>
        /// <param name="camera">The camera to use</param>
        /// <returns></returns>
        public static bool InFrustum(this Renderer renderer, Camera camera)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }

        /// <summary>
        /// Switches the given two elements in the hierarchy of the grid.
        /// </summary>
        /// <param name="first">The first element to be swapped with the second</param>
        /// <param name="second">The second element to be swapped with the first</param>
        public static void SwitchElements(this GridLayoutGroup _, GameObject first, GameObject second)
        {
            int firstPos = first.transform.GetSiblingIndex();
            int secondPos = second.transform.GetSiblingIndex();
            
            first.transform.SetSiblingIndex(secondPos);
            second.transform.SetSiblingIndex(firstPos);
        }
    }
}