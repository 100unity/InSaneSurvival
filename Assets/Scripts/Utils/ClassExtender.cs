using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        /// Finds all instances of the given type in the project
        /// </summary>
        /// <typeparam name="T">The type of the instance to be searched for</typeparam>
        /// <returns>All found instances</returns>
        public static T[] GetAllInstances<T>(this Object _) where T : Object
        {
            //All globally unique identifiers of the instances
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            T[] instances = new T[guids.Length];
            for (int i = 0; i < instances.Length; i++)
                instances[i] = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[i]));

            return instances;
        }
        
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
    }
}