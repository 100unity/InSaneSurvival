using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            Instance = this;
        }
    }
    #endregion

    [SerializeField] private GameObject player;

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

}
