using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Utils
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Get a pseudo-unique ID for a game object. Uses the object's transform and the
        /// scene it is in to calculate a hash which can be used to identify the object.
        /// </summary>
        /// <param name="gameObject">The game object to get the ID for</param>
        /// <returns>A pseudo-unique string ID</returns>
        public static string GetId(this GameObject gameObject)
        {
            Transform transform = gameObject.transform;
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = transform.lossyScale;

            StringBuilder builder = new StringBuilder();
            
            builder.Append(position.x);
            builder.Append(position.y);
            builder.Append(position.z);

            builder.Append(rotation.x);
            builder.Append(rotation.y);
            builder.Append(rotation.z);
            builder.Append(rotation.w);

            builder.Append(scale.x);
            builder.Append(scale.y);
            builder.Append(scale.z);

            builder.Append(gameObject.scene);

            HashAlgorithm algorithm = SHA256.Create();
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));

            return hash.Aggregate(string.Empty, (current, x) => current + $"{x:x2}");
        }
    }
}