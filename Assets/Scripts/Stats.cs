using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is for testing the attack system implementation and does not have to be implemented like this.
/// </summary>
public class Stats : MonoBehaviour, IDamageable
{
    [Tooltip("The time the object should be marked as hit after being hit")]
    [SerializeField]
    private float hitMarkTime;
    
    // ------temp for hit animation------
    private MeshRenderer _gameObjectRenderer;
    private Material _prevMat;
    private Material _hitMarkerMaterial;
    // ----------

    private float _timer;
    private bool _hit;

    private void Awake()
    {
        _gameObjectRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        _hitMarkerMaterial = new Material(Shader.Find("Standard"));
        _hitMarkerMaterial.color = Color.red;
        // just put initial mat here
        _prevMat = _gameObjectRenderer.material;
    }

    /// <summary>
    /// Changes the objects color back to normal after being hit.
    /// </summary>
    private void Update()
    {
        if (_hit)
            _timer += Time.deltaTime;

        if (_timer > hitMarkTime)
        {
            _hit = false;
            _timer = 0;
            _gameObjectRenderer.material = _prevMat;
        }
    }
    
    /// <summary>
    /// Mark the object as hit after being hit.
    /// </summary>
    /// <param name="damage">not implemented</param>
    public void Hit(int damage)
    {
        _hit = true;

        //_prevMat = _gameObjectRenderer.material;
        _gameObjectRenderer.material = _hitMarkerMaterial;
    }
    public void Die()
    {
        throw new System.NotImplementedException();
    }
}
