using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class does not really implement the stats (like health etc) YET.
/// </summary>
public class EnemyState : MonoBehaviour, IDamageable
{
    [Tooltip("The time the object should be marked as hit after being hit")]
    [SerializeField]
    private float hitMarkTime;

    [Tooltip("The MeshRenderer of the graphics object of the player")]
    [SerializeField]
    private MeshRenderer _gameObjectRenderer;

    private Material _prevMat;
    private Material _hitMarkerMaterial;

    private float _timer;
    private bool _hit;

    private void Awake()
    {
        _hitMarkerMaterial = new Material(Shader.Find("Standard"));
        _hitMarkerMaterial.color = Color.red;
        _prevMat = _gameObjectRenderer.material;
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
                _gameObjectRenderer.material = _prevMat;
            }
        }   
    }
    
    /// <summary>
    /// Mark the object as hit after being hit.
    /// </summary>
    /// <param name="damage">not implemented</param>
    public void Hit(int damage)
    {
        _hit = true;
        _gameObjectRenderer.material = _hitMarkerMaterial;
    }
    public void Die()
    {
        throw new System.NotImplementedException();
    }
}
