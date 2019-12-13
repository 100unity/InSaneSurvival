using UnityEngine;

namespace AbstractClasses
{
    
    public abstract class Damageable : MonoBehaviour
    {
        // ------temp for hit animation------
        [Tooltip("The time the object should be marked as hit after being hit")]
        [SerializeField]
        private float hitMarkTime;

        [Tooltip("The MeshRenderer of the graphics object of the player")]
        [SerializeField]
        private MeshRenderer gameObjectRenderer;

        private Material _prevMat;
        private Material _hitMarkerMaterial;
        private float _timer;
        private bool _hit;

        protected virtual void Awake()
        {
            // ------------
            _hitMarkerMaterial = new Material(Shader.Find("Standard"));
            _hitMarkerMaterial.color = Color.red;
            // just put initial mat here
            _prevMat = gameObjectRenderer.material;
            // ------------
        }

        /// <summary>
        /// Changes the objects color back to normal after being hit.
        /// </summary>
        protected void Update()
        {
            if (_hit)
            {
                _timer += Time.deltaTime;

                if (_timer > hitMarkTime)
                {
                    _hit = false;
                    _timer = 0;
                    gameObjectRenderer.material = _prevMat;
                }
            }
        }

        /// <summary>
        /// Marks the player as hit after being hit.
        /// </summary>
        /// <param name="damage">The damage dealt to player</param>
        public virtual void Hit(int damage)
        {
            //-------
            _hit = true;
            gameObjectRenderer.material = _hitMarkerMaterial;
            //-------
        }

        public abstract void Die();
    }
}
