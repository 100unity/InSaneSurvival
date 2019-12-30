using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
   
    public void Shoot()
    {
       AudioManager.Instance.Play("shoot");
    }

    public void FadeInLoop()
    {
        AudioManager.Instance.FadeIn("testloop", 10f);
    }
    
    public void FadeOutLoop()
    {
        AudioManager.Instance.FadeOut("testloop", 10f);
    }
}
