using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DeathScene : MonoBehaviour
{
    [SerializeField] private Image whitePanel;
    [SerializeField] private GameObject playerCharacter;

    // Start is called before the first frame update

    void Start()
    {
        StartCoroutine( FadeOut(2.5f));
        playerCharacter.GetComponent<Animator>().SetBool ("isDead", true);
    }

    // fades out white panel
    private IEnumerator FadeOut(float fadeDuration)
    {
        float currentTime = 0f;
        Color color = whitePanel.color;
            
        while (currentTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            color = new Color(color.r, color.g, color.b, alpha);
            whitePanel.color = color;
            currentTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(fadeDuration);
        
    }
}
