using UnityEngine;

/// <summary>
/// Currently objects are rendered also behind walls, so the desired effect, 
/// that NPCs do spawn as long as the player can't see them (so in camera frustum but behind walls as well), is not achieved.
/// So this is not used, currently.
/// </summary>
public class RenderChecker : MonoBehaviour
{
    [Tooltip("The renderer that renders the real mesh")]
    [SerializeField]
    private MeshRenderer _meshRenderer;

    private float _spawnRenderTimer;
    private bool _isNew;
    

    private void OnEnable()
    {
        _spawnRenderTimer = 0;
        _isNew = true;
    }
    
    private void Update()
    {
        _spawnRenderTimer += Time.deltaTime;
    }

    /// <summary>
    /// Destroy the gameobject if it is immediately rendered after spawning. 
    /// Otherwise activate real mesh renderer.
    /// </summary>
    private void OnWillRenderObject()
    {
        if (_isNew)
        {
            _isNew = false;
            if (_spawnRenderTimer < 0.1)
            {
                Destroy(transform.gameObject);
            }
            else
            {
                _meshRenderer.enabled = true;
            }
        }
    }
}
