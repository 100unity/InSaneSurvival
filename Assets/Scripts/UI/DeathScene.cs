using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class DeathScene : MonoBehaviour
{
    [SerializeField] private FadeElement whiteLight;
    [SerializeField] private GameObject playerCharacter;

    // Start is called before the first frame update
    void Start()
    {
        whiteLight.FadeOut(2.5f);
        playerCharacter.GetComponent<Animator>().SetBool ("isDead", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
