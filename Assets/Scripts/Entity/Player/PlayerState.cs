using Interfaces;
using Remote;
using UnityEngine;

namespace Entity.Player
{
    
    public class PlayerState : MonoBehaviour, IDamageable
    {
        public delegate void PlayerStateChanged(int newValue);
        
        //Player State values
        [Tooltip("100 - full health, 0 - dead")] [SerializeField] [Range(0, 100)] 
        private int health;

        [Tooltip("100: no hunger, 0: the player is in desperate need for food")] [SerializeField] [Range(0, 100)] 
        private int saturation;

        [Tooltip("100: not thirsty, 0: gazing for a sip of water")] [SerializeField] [Range(0, 100)] 
        private int hydration;


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
        // ----------


        public static event PlayerStateChanged OnPlayerHealthUpdated;
        public static event PlayerStateChanged OnPlayerSaturationUpdated;
        public static event PlayerStateChanged OnPlayerHydrationUpdated;
        public static event PlayerStateChanged OnPlayerDeath;

        private void Awake()
        {
            // ------------
            _hitMarkerMaterial = new Material(Shader.Find("Standard"));
            _hitMarkerMaterial.color = Color.red;
            // just put initial mat here
            _prevMat = gameObjectRenderer.material;
            // ------------
        }

        private void OnEnable()
        {
            RemoteStatusHandler.OnPlayerHealthRemoteUpdate += ChangePlayerHealth;
            RemoteStatusHandler.OnPlayerHydrationRemoteUpdate += ChangePlayerHydration;
            RemoteStatusHandler.OnPlayerSaturationRemoteUpdate += ChangePlayerSaturation;
        }

        private void OnDisable()
        {
            RemoteStatusHandler.OnPlayerHealthRemoteUpdate -= ChangePlayerHealth;
            RemoteStatusHandler.OnPlayerHydrationRemoteUpdate -= ChangePlayerHydration;
            RemoteStatusHandler.OnPlayerSaturationRemoteUpdate -= ChangePlayerSaturation;
        }

        /// <summary>
        /// Changes the objects color back to normal after being hit.
        /// </summary>
        private void Update()
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

        //Interface
        private void ChangePlayerHealth(int changeBy)
        {
            int updatedValue = health + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue < 0)
            {
                updatedValue = 0;
                Die();
            }

            health = updatedValue;
            
            //throws an event with the new health value as a parameter
            OnPlayerHealthUpdated?.Invoke(updatedValue);
        }

        public void ChangePlayerSaturation(int changeBy)
        {
            int updatedValue = saturation + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue < 0)
            {
                updatedValue = 0;
                ChangePlayerHealth(-1);
            }

            saturation = updatedValue;
            OnPlayerSaturationUpdated?.Invoke(updatedValue);
        }

        public void ChangePlayerHydration(int changeBy)
        {
            int updatedValue = hydration + changeBy;
            if (updatedValue > 100) updatedValue = 100;
            else if (updatedValue < 0)
            {
                updatedValue = 0;
                ChangePlayerHealth(-1);
            }

            hydration = updatedValue;
            OnPlayerHydrationUpdated?.Invoke(updatedValue);
        }

        /// <summary>
        /// Does damage to the player.
        /// Marks the player as hit after being hit.
        /// </summary>
        /// <param name="damage">The damage dealt to player</param>
        public void Hit(int damage)
        {
            ChangePlayerHealth(-damage);

            //-------
            _hit = true;
            gameObjectRenderer.material = _hitMarkerMaterial;
            //-------
        }

        public void Die()
        {            
            OnPlayerDeath?.Invoke(1);
            Debug.Log("Player is dead");
        }
    }
}